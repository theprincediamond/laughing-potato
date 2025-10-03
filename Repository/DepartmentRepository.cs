using itec420.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace itec420.Repository
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department?> GetDepartmentWithDoctorsAsync(int departmentId);
    }

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Department?> GetDepartmentWithDoctorsAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.Id == departmentId);
        }
    }
}
