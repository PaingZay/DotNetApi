using System;
using DOTNETAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTNETAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

        public virtual DbSet<UserSalary> UserSalary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)//See if we.ve already configured//If optionsBuilder is not configured
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),//setup configuration
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure()); //Whether or not retry if this connection fails the first time
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>()
                    .ToTable("Users", "TutorialAppSchema")//Users table on the TutorialAppSchema
                    .HasKey(u => u.UserId);//Set primary key on the table. EF knows this value is autopopulated when we insert a new row and this value is unique for each individual row in a table

            modelBuilder.Entity<UserSalary>()
                    .HasKey(u => u.UserId);

            modelBuilder.Entity<UserJobInfo>()
                    .HasKey(u => u.UserId);

        }

    }

}

