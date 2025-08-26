using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public bool Any(Expression<Func<T, bool>>? filter = null, bool noTracking = true)
        {
            IQueryable<T> query = noTracking ? _dbSet.AsNoTracking() : _dbSet;
            return filter == null ? query.Any() : query.Any(filter);
        }

        public void Delete(T p)
        {
            _dbSet.Remove(p);
            _context.SaveChanges();
        }

        public T? Get(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var item in includes)
                query = query.Include(item);

            return filter != null ? query.FirstOrDefault(filter) : query.FirstOrDefault();
        }

        public T? Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            return query.FirstOrDefault();
        }

        public void Insert(T p)
        {
            _dbSet.Add(p);
            _context.SaveChanges();
        }

        public List<T> List(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return query.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return query.Where(filter).ToList();
        }

        public void Update(T p)
        {
            _dbSet.Update(p);
            _context.SaveChanges();
        }
    }
}
