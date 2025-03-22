using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _rep;
        private readonly ITransactionRepository _repTransactions;
        private readonly IRecurringTransactionRepository _repRecurringTransactions;
        private readonly ICategoryRepository _repCategories;

        public UserService(IRepository<User> rep, ITransactionRepository repTransactions, ICategoryRepository repCategories, IRecurringTransactionRepository repRecurringTransactions)
        {
            _rep = rep;
            _repTransactions = repTransactions;
            _repCategories = repCategories; 
            _repRecurringTransactions = repRecurringTransactions;
        }

        public async Task<User> AddAsync(UserCreateDto dto)
        {
            var existingUser = await _rep.GetAllAsync(u => u.Email == dto.Email);
            if (existingUser.Any())
                throw new InvalidOperationException("Este email já está em uso.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = dto.MapTo<UserCreateDto, User>();
            user.Password = hashedPassword;

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

        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _rep.GetByIdAsync(userId);
            if (user == null)
                return false;

            // Deletar transações do usuário primeiro
            await _repTransactions.DeleteByUserIdAsync(userId);

            // Deletar transações do usuário primeiro
            await _repRecurringTransactions.DeleteByUserIdAsync(userId);

            // Deletar categorias do usuário
            await _repCategories.DeleteByUserIdAsync(userId);

            // Agora pode deletar o usuário
            await _rep.DeleteAsync(userId);

            return true;
        }


        public async Task<User> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _rep.GetByIdAsync(dto.Id);
            user = dto.MapTo(user);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Password = hashedPassword;

            await _rep.UpdateAsync(user);

            return user;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _rep.GetFirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return user;
        }

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry)
        {
            var user = await _rep.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;

            await _rep.UpdateAsync(user);
        }
    }
}
