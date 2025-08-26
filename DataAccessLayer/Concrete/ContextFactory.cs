using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var cs = "Server=DESKTOP-EQEHSQD\\SQLEXPRESS;Database=IntsecEmplyeTrackDb;Trusted_Connection=True;TrustServerCertificate=True;";

            return new Context(new DbContextOptionsBuilder<Context>()
                .UseSqlServer(cs)
                .Options);
        }
    }
}
