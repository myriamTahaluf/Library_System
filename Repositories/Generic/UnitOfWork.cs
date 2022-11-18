using Library_System.Model;
using Library_System.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Repositories.Generic
{
    public class UnitOfWork : IUnitOfWork, System.IDisposable
    {
        #region Fields
        private bool disposed = false;
        private readonly LibraryContext _context;

        private IGenericRepository<Book> _BookRepository;
        private IGenericRepository<User> _UserRepository;
        private IGenericRepository<Library> _LibraryRepository;
        private IGenericRepository<Library_Book> _Library_BookRepository;
       
        #endregion
        #region Properties
        //
        public IGenericRepository<Book> BookRepository { get => _BookRepository ?? (_BookRepository = new GenericRepository<Book>(_context)); }



        public IGenericRepository<Library> LibraryRepository { get => _LibraryRepository ?? (_LibraryRepository = new GenericRepository<Library>(_context)); }

        public IGenericRepository<User> UserRepository { get => _UserRepository ?? (_UserRepository = new GenericRepository<User>(_context)); }

        public IGenericRepository<Library_Book> Library_BookRepository { get => _Library_BookRepository ?? (_Library_BookRepository = new GenericRepository<Library_Book>(_context)); }




        #endregion
        #region CTOR
        public UnitOfWork(LibraryContext context)
        {
            _context = context;
        }
        #endregion
        public void Save()
        {
            try
            {
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                 await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

    }
}
