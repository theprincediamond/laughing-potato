using itec420.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace itec420.Repository
{
    public interface IAppRoleRepository : IGenericRepository<AppRole>
    {
        Task<IdentityRole?> GetRoleByIdAsync(string roleId);
    }

    public class AppRoleRepository : GenericRepository<AppRole>, IAppRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public AppRoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IdentityRole?> GetRoleByIdAsync(string roleId)
        {
            return await _context.AppRoles
                .FirstOrDefaultAsync(r=> r.Id == roleId);
        }
    }
}
