using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Api.Track.co.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(IConfiguration configuration, ILogger<AppDbContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _logger.LogInformation("Connected to the database successfully");
        }

        public DbSet<Widget> Widgets { get; set; }

        public DbSet<Nps> Nps { get; set; }
    }
}