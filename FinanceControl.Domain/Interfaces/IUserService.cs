using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> AddAsync(UserCreateDto User);
        Task<UserReadDto> GetByIdAsync(int id);
        Task<List<UserReadDto>> GetAllAsync();
        Task<User> UpdateAsync(UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
