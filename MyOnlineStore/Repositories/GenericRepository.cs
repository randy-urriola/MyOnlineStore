using MyOnlineStore.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyOnlineStore.Repositories
{
    // los repositories se encargan de hacer los metodos basicos para consultar a la BD
    // y en este caso funcionara para todas las entidades.
    public class GenericRepository<TEntity>(AppDbContext _dbContext) where TEntity : class // TEntity es una clase que representa a una entidad de la BD
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        // devuelve una entidad anidada con sus relaciones, si no la encuentra devuelve null
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>[]? conditions = null,
            Expression<Func<TEntity, object>>[]? includes = null) 
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>(); // prepara un select * from TEntity pero aun no lo ejecuta

            if (conditions is not null)
                foreach (var condition in conditions) query = query.Where(condition);

            if (includes is not null)
                foreach (var include in includes) query = query.Include(include); // agrega los includes a la consulta como si fuera un inner join

            return await query.ToListAsync();
        }

        // agrega una entidad a la BD
        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        // devuelve una entidad en especifica por su id, si no la encuentra devuelve null
        public async Task<TEntity?> GetByIdAsync(int entityId)
        {
            return await _dbContext.Set<TEntity>().FindAsync(entityId);
        }

        // Editar una entidad de la BD
        public async Task EditAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Eliminar una entidad de la BD
        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}