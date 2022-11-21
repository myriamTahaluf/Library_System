using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Library_System.Model
{
    public class LibraryContext:DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Library> Libraries { get; set; }
        //public virtual DbSet<Library_Book> Library_Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MaqraaConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          //  modelBuilder.Entity<Library_Book>()
          //.HasKey(bc => new { bc.Book_Id, bc.Library_Id});
          //  modelBuilder.Entity<Library_Book>()
          //      .HasOne(bc => bc.Book)
          //      .WithMany(b => b.library_Books)
          //      .HasForeignKey(bc => bc.Book_Id);
            
          //  modelBuilder.Entity<Library_Book>()
          //      .HasOne(bc => bc.Library)
          //      .WithMany(c => c.library_Books)
          //      .HasForeignKey(bc => bc.Library_Id);
        }
        }
    
}