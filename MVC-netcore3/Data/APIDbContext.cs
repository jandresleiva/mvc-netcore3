using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_netcore3.Models;

namespace MVC_netcore3.Data
{
  public class APIDbContext : DbContext
  {
    
    public DbSet<Empleado> Empleado { get; set; }
    public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
    {
    }
  }
}
