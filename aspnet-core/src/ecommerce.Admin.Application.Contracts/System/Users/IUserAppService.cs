﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin.System.Users
{
    public interface IUserAppService :ICrudAppService<
        UserDto,
        Guid,
        PagedResultRequestDto,
        CreateUserDto,
        UpdateUserDto>
    {
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<PagedResultDto<UserInListDto>> GetListWithFilterAsync(BaseListFilterDto input);

        Task<List<UserInListDto>> GetListAllAsync();

        Task AssignRolesAsync(Guid userId, string[] roleNames);

        Task SetPasswordAsync(Guid userId, SetPasswordDto input);
    }
}
