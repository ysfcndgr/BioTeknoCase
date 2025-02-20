using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Orders.Commands;
using BioTekno.Domain.Enums;
using BioTekno.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BioTekno.UnitTest.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OrdersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateOrder_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateOrder(null);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateOrderCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task CreateOrder_EmptyProductDetails_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new CreateOrderRequest
            {
                CustomerName = "Test",
                ProductDetails = new List<ProductDetail>()
            };

            // Act
            var result = await _controller.CreateOrder(invalidRequest);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateOrderCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task CreateOrder_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var validRequest = new CreateOrderRequest
            {
                CustomerName = "Test",
                ProductDetails = new List<ProductDetail> { new ProductDetail() }
            };

            var expectedResponse = new ApiResponse<int> { Data = 1, Status = ApiStatus.Success };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default))
                        .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateOrder(validRequest);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
            _mediatorMock.Verify(m => m.Send(It.Is<CreateOrderCommand>(
                c => c.OrderRequest == validRequest), default), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_MediatorFailure_ReturnsBadRequest()
        {
            // Arrange
            var validRequest = new CreateOrderRequest
            {
                CustomerName = "Test",
                ProductDetails = new List<ProductDetail> { new ProductDetail() }
            };

            var failedResponse = new ApiResponse<int> { Status = ApiStatus.Failed };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default))
                        .ReturnsAsync(failedResponse);

            // Act
            var result = await _controller.CreateOrder(validRequest);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo(failedResponse);
        }
    }
}