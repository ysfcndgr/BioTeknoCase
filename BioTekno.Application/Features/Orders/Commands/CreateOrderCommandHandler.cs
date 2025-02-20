using AutoMapper;
using BioTekno.Application.Dtos;
using BioTekno.Application.Interfaces;
using BioTekno.Domain.Entities;
using BioTekno.Domain.Interfaces;
using BioTekno.Infrastructure.Services.RabbitMQ;
using MediatR;

namespace BioTekno.Application.Features.Orders.Commands
{
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IRabbitMqService rabbitMqService, IMapper mapper, ILoggerService<CreateOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _rabbitMqService = rabbitMqService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createReq = request.OrderRequest;

                var newOrder = _mapper.Map<Order>(createReq);
                newOrder.TotalAmount = createReq.ProductDetails.Sum(x => x.UnitPrice * x.Amount);
                newOrder.OrderDetails = createReq.ProductDetails.Select(pd => new OrderDetail
                {
                    ProductId = pd.ProductId,
                    UnitPrice = pd.UnitPrice,
                    Amount = pd.Amount
                }).ToList();


                await _unitOfWork.Orders.AddAsync(newOrder);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"{newOrder.Id} numaralı sipariş kayıdı oluşturuldu");

                _rabbitMqService.PublishMailQueue(newOrder.CustomerEmail, $"Siparişiniz oluşturuldu", 
                    "<!DOCTYPE html>\n<html>\n<head>\n    " +
                    "<title>Sipariş Bilgisi</title>\n</head>\n<body>\n  " +
                    "" +
                    $"  <h1>Sayın {request.OrderRequest.CustomerName},</h1>\n    <p>Siparişiniz başarıyla oluşturuldu." +
                    "" +
                    " Sipariş detayları aşağıdadır:</p>\n    <ul>\n " +
                    "" +
                    $"       <li>Sipariş ID: {newOrder.Id}</li>\n      " +
                    $"  <li>Tarih: {newOrder.CreatedDate}</li>\n    </ul>\n " +
                     $"  <li>Toplam Tutar: {newOrder.TotalAmount}</li>\n    </ul>\n " +

                    "   <p>Teşekkür ederiz.</p>\n</body>\n</html>", true);

                return new ApiResponse<int>
                {
                    Status = Domain.Enums.ApiStatus.Success,
                    ResultMessage = "Sipariş başarıyla oluşturuldu",
                    Data = newOrder.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message);

                return new ApiResponse<int>
                {
                    Status = Domain.Enums.ApiStatus.Failed,
                    ResultMessage = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}
