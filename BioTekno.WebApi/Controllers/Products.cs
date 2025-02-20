using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioTekno.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Kategoriye göre ürünleri getirir (Redis Cache kullanır).
        /// </summary>
        /// <param name="category">Ürünlerin kategorisi (opsiyonel)</param>
        /// <returns>Ürün listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetProducts([FromQuery] string category = null)
        {
            var query = new GetProductsQuery(category);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
