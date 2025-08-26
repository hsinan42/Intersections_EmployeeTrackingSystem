using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepository<T> where T : class
    {
        T? Get(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        T? Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
        List<T> List(params Expression<Func<T, object>>[] includes);
        List<T> List(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        void Insert(T p);
        void Delete(T p);
        void Update(T p);
        bool Any(Expression<Func<T, bool>>? filter = null, bool noTracking = true);
    }
}
