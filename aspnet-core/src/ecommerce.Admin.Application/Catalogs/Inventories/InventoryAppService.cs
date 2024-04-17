using ecommerce.Admin.Catalog.Inventories;
using ecommerce.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ecommerce.Admin.Catalogs.Inventories
{
    public class InventoryAppService : CrudAppService<
        Inventory,
        InventoryDto,
        Guid, PagedResultRequestDto,
        CreateUpdateInventoryDto,
        CreateUpdateInventoryDto>,IInventoryAppService
    {
        public InventoryAppService(IRepository<Inventory, Guid> repository) : base(repository)
        {

        }

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public async Task<List<InventoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Inventory>,List<InventoryInListDto>>(data);
        }

        public async Task<PagedResultDto<InventoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.SKU.Contains(input.keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<InventoryInListDto>(totalCount, ObjectMapper.Map<List<Inventory>, List<InventoryInListDto>>(data));
        }
    }
}
