using Build_School_Project_No_4.DataModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Build_School_Project_No_4.Services
{
    public class Repository
    {
        public readonly DbContext _context;

        public Repository(DbContext context)
        {
            if (context == null) { throw new ArgumentNullException(); }
            _context = context;
        }
        public Repository()
        {
            _context = new EPalContext();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Create<T>(T value) where T : class
        {
            _context.Entry(value).State = EntityState.Added;
        }
        public void Update<T>(T value) where T : class
        {
            _context.Entry(value).State = EntityState.Modified;
        }
        public void Delete<T>(T value) where T : class
        {
            _context.Entry(value).State = EntityState.Deleted;
        }
        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}