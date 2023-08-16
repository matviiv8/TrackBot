using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrackBot.Infrastructure.Context;
using TrackBot.Domain.Repositories;

namespace TrackBot.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TrackContext _trackContext;
        protected readonly DbSet<T> _entities;

        public BaseRepository(TrackContext trackContext)
        {
            this._trackContext = trackContext;
            _entities = trackContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(
            Func<T, bool> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _entities.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await Task.FromResult(query.AsNoTracking().ToList());
        }

        public async Task<bool> isExists(Expression<Func<T, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }
    }
}
