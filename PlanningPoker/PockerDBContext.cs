using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanningPoker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningPoker;

public class PockerDBContext: DbContext
{
    protected readonly IConfiguration Configuration;
    public PockerDBContext(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }
    public DbSet<Sample> Samples { get; set; }
}
