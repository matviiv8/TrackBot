using System.Linq.Expressions;

namespace TrackBot.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAll(Func<T, bool> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<bool> isExists(Expression<Func<T, bool>> predicate);
    }
}