using itec420.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace itec420.Repository
{
    public interface IAppUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
    }

    public class AppUserRepository : GenericRepository<ApplicationUser>, IAppUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
