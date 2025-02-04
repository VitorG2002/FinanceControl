using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> AddAsync(UserCreateDto User);
        Task<UserReadDto> GetByIdAsync(int id);
        Task<List<UserReadDto>> GetAllAsync();
        Task<User> UpdateAsync(UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<User> ValidateUserAsync(string email, string password);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry);
    }
}
