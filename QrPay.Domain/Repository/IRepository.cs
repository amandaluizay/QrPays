using Microsoft.EntityFrameworkCore;
using QrPay.Domain.Interfaces;

namespace QrPay.Domain.Repository
{
    public interface IRepository<TEntity>
              where TEntity : class, IEntity
    {
        DbSet<TEntity> Entities { get; }

        Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
