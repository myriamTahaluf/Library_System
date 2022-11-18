using Library_System.Model;


namespace Library_System.Repositories.Generic.Interface
{
    public interface IUnitOfWork
    {
        IGenericRepository<Book> BookRepository { get; }
        IGenericRepository<Library> LibraryRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Library_Book> Library_BookRepository { get; }
       
        void Save();
        Task SaveChangesAsync();
    }
}
