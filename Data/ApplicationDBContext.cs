using System;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }

    public DbSet<Stock> Stock { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
