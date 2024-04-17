using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecomerce.Public.Orders
{
    public interface IOrderAppService : ICrudAppService
        <OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>
    {

    }
}
