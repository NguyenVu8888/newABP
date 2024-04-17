using Ecomerce.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecommerce.Public.Catalogs.Manufactures
{
    public interface IManufactureAppService : IReadOnlyAppService
       <ManufactureDto,
        Guid,
        PagedResultRequestDto>
    {
        Task<PagedResult<ManufactureInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<ManufactureInListDto>> GetListAllAsync();
    }
}
