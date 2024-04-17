using ecommerce.Admin.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ecommerce.Admin.System.Users
{


    [Authorize(IdentityPermissions.Users.Default, Policy = "AdminOnly")]

    public class UserAppService : CrudAppService<
        IdentityUser,
        UserDto,
        Guid,
        PagedResultRequestDto,
        CreateUserDto,
        UpdateUserDto>, IUserAppService
    {
        private readonly IdentityUserManager _identityUserManager;
        public UserAppService(IRepository<IdentityUser, Guid> repository, IdentityUserManager identityUserManager) : base(repository)
        {
            _identityUserManager = identityUserManager;


            GetPolicyName = ecommercePermissions.Product.Default;
            GetListPolicyName = ecommercePermissions.Product.Default;
            CreatePolicyName = ecommercePermissions.Product.Create;
            UpdatePolicyName = ecommercePermissions.Product.Update;
            DeletePolicyName = ecommercePermissions.Product.Delete;
        }



        [Authorize(IdentityPermissions.Users.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }




        [Authorize(IdentityPermissions.Users.Default)]

        public async Task<List<UserInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();

            var data = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);
        }


        [Authorize(IdentityPermissions.Users.Default)]

        public async Task<PagedResultDto<UserInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();

            if (!input.keyword.IsNullOrWhiteSpace())
            {
                input.keyword = input.keyword.ToLower();
                query = query.Where(o => o.Name.ToLower().Contains(input.keyword)
                || o.Email.ToLower().Contains(input.keyword)
                || o.PhoneNumber.ToLower().Contains(input.keyword));
            }
            query = query.OrderByDescending(x => x.CreationTime);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var data = await AsyncExecuter.ToListAsync(query);
            var users = ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);
            return new PagedResultDto<UserInListDto>(totalCount, users);
        }



        [Authorize(IdentityPermissions.Users.Create)]

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            var query = await Repository.GetQueryableAsync();
            var isUserNameExisted = query.Any(x => x.UserName == input.UserName);
            if (isUserNameExisted)
            {
                throw new UserFriendlyException("Tài khoản đã tồn tại");
            }

            var isUserEmailExisted = query.Any(x => x.Email == input.Email);
            if (isUserEmailExisted)
            {
                throw new UserFriendlyException("Email đã tồn tại");
            }
            var userId = Guid.NewGuid();
            var user = new IdentityUser(userId, input.UserName, input.Email);
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.SetPhoneNumber(input.PhoneNumber,true);
            var result = await _identityUserManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                return ObjectMapper.Map<IdentityUser, UserDto>(user);
            }
            else
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }




        [Authorize(IdentityPermissions.Users.Update)]

        public override async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }
            user.Name = input.Name;
            user.SetPhoneNumber(input.PhoneNumber, true);
            user.Surname = input.Surname;
            var result = await _identityUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ObjectMapper.Map<IdentityUser, UserDto>(user);
            }
            else
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }



        [Authorize(IdentityPermissions.Users.Default)]

        public override async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }
            var userDto = ObjectMapper.Map<IdentityUser, UserDto>(user);
            var roles = await _identityUserManager.GetRolesAsync(user);
            userDto.Roles = roles;
            return userDto;
        }


        [Authorize(IdentityPermissions.Users.Update)]

        public async Task AssignRolesAsync(Guid userId, string[] roleNames)
        {
            var user = await _identityUserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }
            var currentRoles = await _identityUserManager.GetRolesAsync(user);
            var removedResult = await _identityUserManager.RemoveFromRolesAsync(user, currentRoles);
            var addedResult = await _identityUserManager.AddToRolesAsync(user, roleNames);
            if (!addedResult.Succeeded || !removedResult.Succeeded)
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> addedErrorList = addedResult.Errors.ToList();
                List<Microsoft.AspNetCore.Identity.IdentityError> removedErrorList = removedResult.Errors.ToList();
                var errorList = new List<Microsoft.AspNetCore.Identity.IdentityError>();
                errorList.AddRange(addedErrorList);
                errorList.AddRange(removedErrorList);
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }



        [Authorize(IdentityPermissions.Users.Update)]

        public async Task SetPasswordAsync(Guid userId, SetPasswordDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }
            var token = await _identityUserManager.GeneratePasswordResetTokenAsync(user);
            var result = await _identityUserManager.ResetPasswordAsync(user, token, input.NewPassword);
            if (!result.Succeeded)
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }
    }
}
