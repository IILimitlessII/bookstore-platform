using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookstorePlatform.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<StoreUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StoreUser> StoreUsers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Series> Seriess { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; }
        public DbSet<Book_Series> Book_Series { get; set; }
        public DbSet<Book_Genre> Book_Genres { get; set; }
        public DbSet<Book_Publisher> Book_Publisher { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Book> Cart_Books { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Order_Book> Order_Book { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Wishlist_Book> Wishlist_Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*builder.ApplyConfiguration(new ApplicationUserEnitityConfiguration());*/
        }
    }

/*    public class ApplicationUserEnitityConfiguration : IEntityTypeConfiguration<StoreUser>
    {
        public void Configure(EntityTypeBuilder<StoreUser> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
        }
    }*/
}
