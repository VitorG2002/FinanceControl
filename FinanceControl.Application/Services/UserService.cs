using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _rep;

        public UserService(IRepository<User> rep)
        {
            _rep = rep;
        }

        public async Task<User> AddAsync(UserCreateDto dto)
        {
            User user = dto.MapTo<UserCreateDto, User>();

            return await _rep.AddAsync(user);
        }

        public async Task<UserReadDto> GetByIdAsync(int id)
        {
            var user = await _rep.GetByIdAsync(id);

            var dto = user.MapTo<User, UserReadDto>();

            return dto;
        }

        public async Task<List<UserReadDto>> GetAllAsync()
        {
            var users = (await _rep.GetAllAsync()).ToList();

            var listDtos = users.MapTo<List<User>, List<UserReadDto>>();

            return listDtos;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _rep.DeleteAsync(id);

            return true;
        }

        public async Task<User> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _rep.GetByIdAsync(dto.Id);
            user = dto.MapTo(user);

            await _rep.UpdateAsync(user);

            return user;
        }
    }
}
