using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace WebApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
       }

        public void Add(T item)
        {
            dbSet.Add(item);
            
        }

        public void Delete(T item)
        {
            dbSet.Remove(item);    
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items); 
        }

        public T Get(Expression<Func<T, bool>> filter, string? includedProperties =null)
        {


            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includedProperties))
            {
                foreach (var includedProperty in includedProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includedProperty);
                }
            }
            return query.Where(filter).FirstOrDefault();

        }

        public IEnumerable<T> GetAll(string? includedProperties =null)
        {

            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includedProperties))
            {
                foreach (var includedProperty in includedProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includedProperty);
                }
            }
            return query.ToList();
        }
    }
}
