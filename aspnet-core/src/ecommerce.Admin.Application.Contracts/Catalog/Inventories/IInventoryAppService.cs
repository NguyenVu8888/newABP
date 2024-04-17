using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin.Catalog.Inventories
{
    public interface IInventoryAppService :ICrudAppService<
        InventoryDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateInventoryDto,
        CreateUpdateInventoryDto>
    {

        Task<PagedResultDto<InventoryInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<InventoryInListDto>> GetListAllAsync();
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
