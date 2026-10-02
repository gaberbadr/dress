using Application.Features.Products.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Application.Common.Interfaces;
using ErrorOr;
using MediatR;
using Zero.Core.Specification;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ErrorOr<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ErrorOr<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();
            var categoryRepo = _unitOfWork.Repository<Category, int>();

            var spec = new BaseSpecifications<Product, int>(p => p.Id == request.Id);
            var product = await productRepo.GetWithSpecficationAsync(spec);

            if (product == null)
            {
                return Error.NotFound("Product.NotFound", "Product not found.");
            }

            var category = await categoryRepo.GetAsync(request.CategoryId);
            if (category == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found.");
            }

            if (request.MainImage != null)
            {
                // Optionally delete the old main image here if it exists:
                // if (!string.IsNullOrEmpty(product.MainImage)) { _fileService.DeleteFile(product.MainImage, "products"); }
                product.MainImage = _fileService.UploadFile(request.MainImage, "products");
            }

            var finalAdditionalImages = new List<string>(request.ExistingAdditionalImages ?? new List<string>());

            if (request.NewAdditionalImages != null && request.NewAdditionalImages.Any())
            {
                foreach (var img in request.NewAdditionalImages)
                {
                    finalAdditionalImages.Add(_fileService.UploadFile(img, "products"));
                }
            }

            // Optional: delete any old additional images that are not in finalAdditionalImages

            product.Description = request.Description;
            product.Price = request.Price;
            product.AdditionalImages = finalAdditionalImages;
            product.IsAvailable = request.IsAvailable;
            product.CategoryId = request.CategoryId;
            product.Sizes = request.Sizes ?? new List<string>();
            product.Features = request.Features?.Select(f => new ProductFeature { Name = f.Name, Value = f.Value }).ToList() ?? new List<ProductFeature>();

            productRepo.Update(product);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDto>(product);
        }
    }
}
