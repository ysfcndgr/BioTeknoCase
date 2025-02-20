using AutoMapper;
using BioTekno.Application.Dtos;
using BioTekno.Application.Interfaces;
using BioTekno.Domain.Interfaces;
using MediatR;

namespace BioTekno.Application.Features.Products.Queries
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApiResponse<List<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService<GetProductsQueryHandler> _logger;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService<GetProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.Products.GetProductsWithCacheAsync(request.Category);
                if (products.Count == 0)
                {
                    _logger.LogInformation($"{request.Category} İstek atılan kategoriye ait ürün yok.");

                    return new ApiResponse<List<ProductDto>>
                    {
                        Status = Domain.Enums.ApiStatus.Failed,
                        ResultMessage = "İlgili kategoriye ait ürün bulunamadı",
                        ErrorCode = "1"
                    };
                }
                var productDtos = _mapper.Map<List<ProductDto>>(products);

                _logger.LogInformation("Ürünler başarıyla getirildi");


                return new ApiResponse<List<ProductDto>>
                {
                    Status = Domain.Enums.ApiStatus.Success,
                    ResultMessage = "Ürünler başarıyla getirildi",
                    Data = productDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message ?? ex.Message);

                return new ApiResponse<List<ProductDto>>
                {
                    Status = Domain.Enums.ApiStatus.Failed,
                    ResultMessage = ex.Message
                };
            }
        }
    }
}
