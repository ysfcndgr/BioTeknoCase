using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Products.Queries;
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
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProducts_WithoutCategory_ReturnsOkWithData()
        {
            // Arrange
            var expectedResponse = new ApiResponse<List<ProductDto>>
            {
                Data = new List<ProductDto>(),
                Status = ApiStatus.Success
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default))
                        .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetProducts_WithCategory_ReturnsFilteredProducts()
        {
            // Arrange
            var category = "TestCategory";
            var expectedResponse = new ApiResponse<List<ProductDto>>
            {
                Data = new List<ProductDto>(),
                Status = ApiStatus.Success
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default))
                        .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetProducts(category);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<GetProductsQuery>(
                q => q.Category == category), default), Times.Once);
        }
    }
}