using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioTekno.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni sipariş oluşturur ve RabbitMQ kuyruğuna ekler.
        /// </summary>
        /// <param name="request">Sipariş bilgileri</param>
        /// <returns>Oluşturulan sipariş ID</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.ProductDetails == null || !request.ProductDetails.Any())
                return BadRequest("Geçersiz sipariş isteği!");

            var command = new CreateOrderCommand(request);
            var response = await _mediator.Send(command);

            if (response.Status == BioTekno.Domain.Enums.ApiStatus.Failed)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
