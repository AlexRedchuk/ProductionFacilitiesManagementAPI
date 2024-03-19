namespace ProductionFacilitiesManagement.DAL.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> GetByCodeAsync(string code);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task Update(TEntity entity);
    Task Remove(TEntity entity);

}