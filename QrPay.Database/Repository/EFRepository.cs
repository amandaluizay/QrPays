using Microsoft.EntityFrameworkCore;
using QrPay.Database;
using QrPay.Domain.Interfaces;
using QrPay.Domain.Repository;

namespace QrPay.Database.Repository
{
    public class EFRepository
    {
        protected readonly QrPayDbContext _dbContext;

        public EFRepository(QrPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }

    public class EFRepository<TEntity> : IRepository<TEntity>
       where TEntity : class, IEntity
    {
        protected readonly QrPayDbContext _dbContext;

        public EFRepository(QrPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Entities
            => _dbContext.Set<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>()
                            .AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public async Task<TEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var current = await _dbContext.Set<TEntity>()
                                          .SingleAsync(i => i.Id == id, cancellationToken);

            _dbContext.Set<TEntity>().Remove(current);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return current;
        }
        public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entityList = await _dbContext.Set<TEntity>()
                                             .Where(i => entities.Select(e => e.Id).Contains(i.Id))
                                             .ToListAsync(cancellationToken);
            if (entityList.Count != 0)
            {
                _dbContext.Set<TEntity>().RemoveRange(entityList);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return entityList;
        }

        public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default)
            => _dbContext.Set<TEntity>()
                         .AsNoTracking()
                         .ToListAsync(cancellationToken);

        public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _dbContext.Set<TEntity>()
                         .AsNoTracking()
                         .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var current = await _dbContext.Set<TEntity>()
                                          .SingleAsync(i => i.Id == entity.Id, cancellationToken);

            entity.CreatedBy = current.CreatedBy;
            entity.CreatedAt = current.CreatedAt;

            _dbContext.Entry(current).CurrentValues.SetValues(entity);

            _dbContext.Set<TEntity>().Update(current);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return current;
        }

        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                var current = await _dbContext.Set<TEntity>()
                                              .SingleAsync(i => i.Id == entity.Id, cancellationToken);

                entity.CreatedBy = current.CreatedBy;
                entity.CreatedAt = current.CreatedAt;

                _dbContext.Entry(current).State = EntityState.Detached;
                _dbContext.Entry(current).CurrentValues.SetValues(entity);
            }

            _dbContext.Set<TEntity>().UpdateRange(entities);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default)
               => _dbContext.Set<TEntity>()
                            .AnyAsync(i => i.Id == id, cancellationToken);

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
