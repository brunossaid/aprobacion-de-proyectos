using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User
                .Include(u => u.RoleNavigation)
                .ToListAsync();
        }

        public async Task<User> GetByIndexAsync(int index)
        {
            var users = await GetAllAsync();
            if (index >= 0 && index < users.Count)
                return users[index];
            throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");
        }
    }
}
