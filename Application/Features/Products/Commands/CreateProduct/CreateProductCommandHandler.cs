using Application.Features.Products.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ErrorOr<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<Category, int>();
            var productRepo = _unitOfWork.Repository<Product, int>();

            var category = await categoryRepo.GetAsync(request.CategoryId);
            if (category == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found.");
            }

            string mainImageUrl = string.Empty;
            if (request.MainImage != null)
            {
                mainImageUrl = _fileService.UploadFile(request.MainImage, "products");
            }

            var additionalImageUrls = new List<string>();
            if (request.AdditionalImages != null && request.AdditionalImages.Any())
            {
                foreach (var img in request.AdditionalImages)
                {
                    additionalImageUrls.Add(_fileService.UploadFile(img, "products"));
                }
            }

            var product = new Product
            {
                Description = request.Description,
                Price = request.Price,
                MainImage = mainImageUrl,
                AdditionalImages = additionalImageUrls,
                IsAvailable = request.IsAvailable,
                CategoryId = request.CategoryId,
                Sizes = request.Sizes,
                Features = request.Features?.Select(f => new ProductFeature { Name = f.Name, Value = f.Value }).ToList() ?? new List<ProductFeature>()
            };

            await productRepo.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDto>(product);
        }
    }
}
