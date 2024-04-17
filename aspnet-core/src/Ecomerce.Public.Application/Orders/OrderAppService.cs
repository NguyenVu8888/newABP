using ecommerce.Orders;
using ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecomerce.Public.Orders
{
    public class OrderAppService : CrudAppService<
        Order,
        OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>, IOrderAppService
    {
        private readonly IRepository<Orderitem> _orderItemRepository;
        private readonly OrderCodeGenerator _orderCodeGenerator;
        private readonly IRepository<Product, Guid> _productRepository;
        public OrderAppService(IRepository<Order, Guid> repository,
            OrderCodeGenerator orderCodeGenerator,
            IRepository<Orderitem> orderItemRepository,
            IRepository<Product, Guid> productRepository)
            : base(repository)
        {
            _orderItemRepository = orderItemRepository;
            _orderCodeGenerator = orderCodeGenerator;
            _productRepository = productRepository;
        }

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var subTotal = input.Items.Sum(x => x.Quantity * x.Price);
            var orderId = Guid.NewGuid();
            var order = new Order(orderId)
            {
                Code = await _orderCodeGenerator.GenerateAsync(),
                CustomerAddress = input.CustomerAddress,
                CustomerName = input.CustomerName,
                CustomerPhone = input.CustomerPhoneNumber,
                ShippingFee = 0,
                CustomerUserId = input.CustomerUserId,
                Tax = 0,
                SubTotal = subTotal,
                GrandTotal = subTotal,
                Discount = 0,
                PaymentMethod = PaymentMethod.COD,
                Total = subTotal,
                Status = OrderStatus.New

            };
            var items = new List<Orderitem>();
            foreach (var item in input.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                items.Add(new Orderitem()
                {
                    OrderId = orderId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SKU = product.SKU
                });
            }
            await _orderItemRepository.InsertManyAsync(items);

            var result = await Repository.InsertAsync(order);


            return ObjectMapper.Map<Order, OrderDto>(result);
        }


    }
}
