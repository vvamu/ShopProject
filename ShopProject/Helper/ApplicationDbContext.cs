using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShopProject.Data;

namespace ShopProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public static string DbPath {
            get
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);

                var directory = Directory.CreateDirectory(Path.Join(path, "LiteDb"));
                var db = Path.Join(directory.FullName, "LiteDb.db");

                return db;
            }
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"DataSource={DbPath}");






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().HasKey(u => new { u.UserId, u.ItemId });
            modelBuilder.Entity<Comment>().HasKey(u => new { u.UserId, u.ItemId });

            //

            modelBuilder.Entity<User>().HasMany(c => c.Likes).WithOne(c => c.ApplicationUser);
            modelBuilder.Entity<Item>().HasMany(c => c.Likes).WithOne(c => c.ApplicationItem);

            modelBuilder.Entity<User>().HasMany(c => c.Comments).WithOne(c => c.ApplicationUser);
            modelBuilder.Entity<Item>().HasMany(c => c.Comments).WithOne(c => c.ApplicationItem);





            ////
            //modelBuilder.Entity<Property>().HasMany(c => c.PropertyValues).WithOne(c => c.Property);
            //modelBuilder.Entity<PropertyValue>().HasOne(c => c.ApplicationItem).WithOne(c => c.PropertyValue);

            modelBuilder
           .Entity<Property>()
           .Property(e => e.PropertyType)
           .HasConversion<byte>();

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.ClientCascade;
            }

            base.OnModelCreating(modelBuilder);



        }


        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyValue> PropertyValues { get; set; }


    }
 
}