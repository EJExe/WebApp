using Microsoft.AspNetCore.Identity;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            });
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto)
        {
            var user = await _repository.GetByIdAsync(userDto.Id);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrEmpty(userDto.UserName))
                user.UserName = userDto.UserName;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
