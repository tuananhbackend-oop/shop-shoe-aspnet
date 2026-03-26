using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LVTNWEBGIAYDEP.Models
{
    public class DBGiayDepContextFactory : IDesignTimeDbContextFactory<DBGiayDepContext>
    {
        public DBGiayDepContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DBGiayDepContext>();

            optionsBuilder.UseSqlite("Data Source=app.db");

            return new DBGiayDepContext(optionsBuilder.Options);
        }
    }
}