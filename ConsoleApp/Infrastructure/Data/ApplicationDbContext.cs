using ConsoleApp.Domain.Models.Warehouse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pallet> Pallets { get; set; } // Замените Item на вашу модель

        public DbSet<Box> Boxes { get; set; } // Замените Item на вашу модель

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
