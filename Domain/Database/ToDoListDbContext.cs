using Contracts.Database;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database
{
    public class ToDoListDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<ToDoList> ToDoLists { get; init; }
        public DbSet<ToDoTask> ToDoTasks { get; init; }

        public ToDoListDbContext() : base()
        {

        }

        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<ToDoList>()
                .HasOne(tdl => tdl.User)
                .WithMany(u => u.ToDoLists)
                .HasForeignKey(tdl => tdl.UserId);

            _ = modelBuilder.Entity<ToDoTask>()
                .HasOne(tdt => tdt.ToDoList)
                .WithMany(tdl => tdl.ToDoTasks)
                .HasForeignKey(tdt => tdt.ToDoListId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseNpgsql("Uid=postgres;Pwd=fyfnjksq123;Host=localhost:5432;Database=to_do_lists_db;");
        }
    }
}