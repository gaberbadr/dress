using API.Controllers;
using Application.Common.Pagination;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.DTOs;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { message = "Id in path does not match Id in body." });
            }

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id });

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(new { success = true });
        }
    }
}
