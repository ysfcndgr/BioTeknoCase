using BioTekno.Application.Dtos;
using MediatR;

namespace BioTekno.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<ApiResponse<List<ProductDto>>>
    {
        public string Category { get; set; }

        public GetProductsQuery(string category)
        {
            Category = category;
        }
    }
}
