using Microsoft.EntityFrameworkCore;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TodoEntity> Todos { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TopicEntity> Topics { get; set; }
        public DbSet<UserBagEntity> UserBags { get; set; }

        public TodoDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasOne<UserBagEntity>(p => p.UserBag)
                .WithOne(s => s.User)
                 .HasForeignKey<UserEntity>(b => b.UserBagId); ;

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }
    }
}