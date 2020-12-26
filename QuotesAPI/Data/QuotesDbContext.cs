using System;
using Microsoft.EntityFrameworkCore;
using QuotesAPI.Models;

namespace QuotesAPI.Data
{
    // Responsible for dealing with the database - CRUD
    public class QuotesDbContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }

        // To configure QuotesDbContext via AddDbContext, we need to add constructor that receives a paramter of type
        // DbContextOptions<QuotesDbContext>. If not, ASP .NET CORE will not be able to inject the configuration we set via AddDbContext<>

        public QuotesDbContext(DbContextOptions<QuotesDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

    }
}
