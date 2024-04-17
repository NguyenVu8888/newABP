﻿using ecommerce.Admin.Permissions;
using ecommerce.EntityFrameworkCore;
using ecommerce.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace ecommerce.Admin.System.Roles
{

    [Authorize(IdentityPermissions.Roles.Default, Policy = "AdminOnly")]

    public class RoleAppService : CrudAppService
        <IdentityRole,
        RoleDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateRoleDto,
        CreateUpdateRoleDto>, IRoleAppService
    {
        protected PermissionManagementOptions Options { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }
        public RoleAppService(IRepository<IdentityRole, Guid> repository,
            IOptions<PermissionManagementOptions> _options,
            IPermissionManager _permissionManager,
            IPermissionDefinitionManager _permissionDefinitionManager,
            ISimpleStateCheckerManager<PermissionDefinition> _simpleStateCheckerManager
            ) : base(repository)
        {
            Options = _options.Value;
            PermissionManager = _permissionManager;
            PermissionDefinitionManager = _permissionDefinitionManager;
            SimpleStateCheckerManager = _simpleStateCheckerManager;


            GetPolicyName = IdentityPermissions.Roles.Default;
            GetListPolicyName = IdentityPermissions.Roles.Default;
            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
        }


        [Authorize(IdentityPermissions.Roles.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();

        }


        [Authorize(IdentityPermissions.Roles.Default)]

        public async Task<List<RoleInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data);
        }


        [Authorize(IdentityPermissions.Roles.Default)]

        public async Task<PagedResultDto<RoleInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Name.Contains(input.keyword));

            var count = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<RoleInListDto>(count, ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data));
        }


        [Authorize(IdentityPermissions.Roles.Create)]

        public override async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            var query = await Repository.GetQueryableAsync();
            var isNameExisted = query.Any(x => x.Name == input.Name);
            if (isNameExisted)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.RoleNameAlreadyExists)
                    .WithData("Name", input.Name);
            }
            var role = new IdentityRole(Guid.NewGuid(), input.Name);
            role.ExtraProperties[RoleConstant.DescriptionFieldName] = input.Desciption;
            var data = await Repository.InsertAsync(role);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return ObjectMapper.Map<IdentityRole, RoleDto>(data);
        }



        [Authorize(IdentityPermissions.Roles.Update)]

        public override async Task<RoleDto> UpdateAsync(Guid id, CreateUpdateRoleDto input)
        {
            var role = await Repository.GetAsync(id);
            if (role == null)
            {
                throw new EntityNotFoundException(typeof(IdentityRole), id);
            }
            var query = await Repository.GetQueryableAsync();
            var isNameExisted = query.Any(x => x.Name == input.Name && x.Id != id);
            if (isNameExisted)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.RoleNameAlreadyExists)
                    .WithData("Name", input.Name);
            }
            role.ExtraProperties[RoleConstant.DescriptionFieldName] = input.Desciption;
            var data = await Repository.UpdateAsync(role);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return ObjectMapper.Map<IdentityRole, RoleDto>(data);
        }

        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------


        public async Task<GetPermissionListResultDto> GetPermissionsAsync(string providerName, string providerKey)
        {
            //await CheckProviderPolicy(providerName);

            var result = new GetPermissionListResultDto
            {
                EntityDisplayName = providerKey,
                Groups = new List<PermissionGroupDto>()
            };
            var a = await PermissionDefinitionManager.GetGroupsAsync();
            var query = a.Where(x => x.Name.StartsWith("AbpIdentity") || x.Name.StartsWith("ecomAdmin"));
            foreach (var group in query)
            {
                var groupDto = CreatePermissionGroupDto(group);

                var neededCheckPermissions = new List<PermissionDefinition>();

                foreach (var permission in group.GetPermissionsWithChildren()
                                                .Where(x => x.IsEnabled)
                                                .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName)))
                {
                    if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                    {
                        neededCheckPermissions.Add(permission);
                    }
                }

                if (!neededCheckPermissions.Any())
                {
                    continue;
                }

                var grantInfoDtos = neededCheckPermissions
                    .Select(CreatePermissionGrantInfoDto)
                    .ToList();

                var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

                foreach (var grantInfo in multipleGrantInfo.Result)
                {
                    var grantInfoDto = grantInfoDtos.First(x => x.Name == grantInfo.Name);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderKey = provider.Key,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }




        private PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
        {
            return new PermissionGrantInfoDto
            {
                Name = permission.Name,
                DisplayName = permission.DisplayName?.Localize(StringLocalizerFactory),
                ParentName = permission.Parent?.Name,
                AllowedProviders = permission.Providers,
                GrantedProviders = new List<ProviderInfoDto>()
            };
        }

        private PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
        {
            return new PermissionGroupDto
            {
                Name = group.Name,
                DisplayName = group.DisplayName?.Localize(StringLocalizerFactory),
                Permissions = new List<PermissionGrantInfoDto>(),
            };
        }

        public virtual async Task UpdatePermissionsAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            // await CheckProviderPolicy(providerName);

            foreach (var permissionDto in input.Permissions)
            {
                await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
            }
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }
    }
}

