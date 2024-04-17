using Ecomerce.Public;
using ecommerce.Admin.Catalog.Manufactures;
using ecommerce.Manufactrures;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecommerce.Public.Catalogs.Manufactures
{
   /* [Authorize]*/
    public class ManufactureAppService : ReadOnlyAppService<
        Manufacture,
        ManufactureDto,
        Guid,
        PagedResultRequestDto>, IManufactureAppService
    {
  
        public ManufactureAppService(IRepository<Manufacture, Guid> repository) : base(repository)
        {
           
        }


        public async Task<List<ManufactureInListDto>> GetListAllAsync()
        {
        
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);

            var data = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Manufacture>, List<ManufactureInListDto>>(data);
        }

        public async Task<PagedResult<ManufactureInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Name.Contains(input.keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
            .ToListAsync(
               query.Skip((input.CurrentPage - 1) * input.PageSize)
            .Take(input.PageSize));

            return new PagedResult<ManufactureInListDto>(
                ObjectMapper.Map<List<Manufacture>, List<ManufactureInListDto>>(data),
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }
    }
}
