using Marina.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace Marina.DataAccess.Repositories;

public interface IEFCoreGenericRepository<T> where T : BaseEntity
{
    Task<T> Get(long id);
    Task<List<T>> GetAll();

    Task<T> Add(/*ClaimsPrincipal user,*/ T entity);
    Task<ICollection<T>> Add(ClaimsPrincipal user, ICollection<T> entities);

    Task<T> Delete(ClaimsPrincipal user, long id);
    Task<bool> Delete(ClaimsPrincipal user, ICollection<T> entities);

    Task<T> Update(ClaimsPrincipal user, T entity);
    Task<ICollection<T>> Update(ClaimsPrincipal user, ICollection<T> entities);

    IDbContextTransaction BeginTransaction();
    IExecutionStrategy CreateExecutionStrategy();

    IQueryable<T> AsQueryable();

    //Task<IQueryable<T>> IsActive();

    //Task<IQueryable<T>> NotDeleted();

    //Task<IQueryable<T>> NotDeletedIsActive();
}
