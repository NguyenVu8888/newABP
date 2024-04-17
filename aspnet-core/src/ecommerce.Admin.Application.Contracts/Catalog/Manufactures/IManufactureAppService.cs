using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin.Catalog.Manufactures
{
    public interface IManufactureAppService : ICrudAppService
       <ManufactureDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateManufactureDto,
        CreateUpdateManufactureDto>
    {
        Task<PagedResultDto<ManufactureInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<ManufactureInListDto>> GetListAllAsync();
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
