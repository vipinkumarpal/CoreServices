using CoreServices.Infrastructure.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreServices.Infrastructure.Repositories.context
{
    public class CoreServicesDataContext: DbContext
    {
        public CoreServicesDataContext(DbContextOptions<CoreServicesDataContext> options ):base(options)
        {

        }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Register> Register { get; set; }
    }
}
