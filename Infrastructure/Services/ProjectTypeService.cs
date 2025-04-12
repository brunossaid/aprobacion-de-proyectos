using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProjectTypeService : IProjectTypeService
    {
        private readonly AppDbContext _context;

        public ProjectTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectType>> GetAllAsync()
        {
            return await _context.ProjectType.ToListAsync();
        }
    }
}
