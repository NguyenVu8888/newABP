using ecommerce.Admin.Catalog.Manufactures;
using ecommerce.Admin.Permissions;
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

namespace ecommerce.Admin.Catalogs.Manufactures
{
    [Authorize(ecommercePermissions.Manufacture.Default, Policy = "AdminOnly")]

    public class ManufactureAppService : CrudAppService<
        Manufacture,
        ManufactureDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateManufactureDto,
        CreateUpdateManufactureDto>, IManufactureAppService
    {
        private readonly IRepository<Manufacture, Guid> _repository;
        public ManufactureAppService(IRepository<Manufacture, Guid> repository) : base(repository)
        {
            _repository = repository;

            GetPolicyName = ecommercePermissions.Manufacture.Default;
            GetListPolicyName = ecommercePermissions.Manufacture.Default;
            CreatePolicyName = ecommercePermissions.Manufacture.Create;
            UpdatePolicyName = ecommercePermissions.Manufacture.Update;
            DeletePolicyName = ecommercePermissions.Manufacture.Delete;
        }

        [Authorize(ecommercePermissions.Manufacture.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(ecommercePermissions.Manufacture.Default)]

        public async Task<List<ManufactureInListDto>> GetListAllAsync()
        {
        
            var query = await _repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);

            var data = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Manufacture>, List<ManufactureInListDto>>(data);
        }


        [Authorize(ecommercePermissions.Manufacture.Default)]

        public async Task<PagedResultDto<ManufactureInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Name.Contains(input.keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ManufactureInListDto>(totalCount, ObjectMapper.Map<List<Manufacture>, List<ManufactureInListDto>>(data));
        }
    }
}
