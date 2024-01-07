using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Marina.DataAccess.Entities;

namespace Marina.DataAccess.Repositories;

public class EFCoreGenericRepository<TEntity> : IEFCoreGenericRepository<TEntity>
    where TEntity : BaseEntity
    //where TContext : DbContext
{
    private readonly MarinaDbContext context;
    protected readonly DbSet<TEntity> _dbSet;
    public EFCoreGenericRepository(MarinaDbContext context)
    {
        this.context = context;
        _dbSet = context.Set<TEntity>();
    }
    public async Task<TEntity> Add(/*ClaimsPrincipal user,*/ TEntity entity)
    {
        //long currentUserId = 1;
        //if (user != null)
        //{
        //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //}

        _dbSet.Add(entity);

        //var newentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        //newentries.ForEach(e =>
        //{
        //    e.Property("CreateDate").CurrentValue = DateTime.Now;
        //    e.Property("CreatorUserId").CurrentValue = currentUserId;
        //});

        //var editedentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        //editedentries.ForEach(e =>
        //{
        //    e.Property("UpdateTime").CurrentValue = DateTime.Now;
        //    e.Property("UpdaterUserId").CurrentValue = currentUserId;
        //});


        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<ICollection<TEntity>> Add(ClaimsPrincipal user, ICollection<TEntity> entities)
    {
        //long currentUserId = 1;
        //if (user != null)
        //{
        //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //}

        _dbSet.AddRange(entities);
        //var newentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        //newentries.ForEach(e =>
        //{
        //    e.Property("CreateDate").CurrentValue = DateTime.Now;
        //    e.Property("CreatorUserId").CurrentValue = currentUserId;
        //});

        //var editedentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        //editedentries.ForEach(e =>
        //{
        //    e.Property("UpdateTime").CurrentValue = DateTime.Now;
        //    e.Property("UpdaterUserId").CurrentValue = currentUserId;
        //});

        await context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> Delete(ClaimsPrincipal user, long id)
    {
        //long currentUserId = 1;
        //if (user != null)
        //{
        //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //}
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return entity;
        }
        //entity.IsDeleted = true;
        //entity.UpdateTime = DateTime.Now;
        //entity.UpdaterUserId = currentUserId;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(ClaimsPrincipal user, ICollection<TEntity> entities)
    {
        try
        {
            //long currentUserId = 1;
            //if (user != null)
            //{
            //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            //}
            //foreach (var item in entities)
            //{
            //    item.IsDeleted = true;
            //    item.UpdateTime = DateTime.Now;
            //    item.UpdaterUserId = currentUserId;
            //}
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<TEntity> Get(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity> Update(ClaimsPrincipal user, TEntity entity)
    {
        //long currentUserId = 1;
        //if (user != null)
        //{
        //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //}
        //entity.UpdateTime = DateTime.Now;
        //entity.UpdaterUserId = currentUserId;
        //context.Entry(entity).State = EntityState.Modified;

        //var newentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        //newentries.ForEach(e =>
        //{
        //    e.Property("CreateDate").CurrentValue = DateTime.Now;
        //    e.Property("CreatorUserId").CurrentValue = currentUserId;
        //});

        //var editedentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        //editedentries.ForEach(e =>
        //{
        //    e.Property("UpdateTime").CurrentValue = DateTime.Now;
        //    e.Property("UpdaterUserId").CurrentValue = currentUserId;
        //});

        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> Update(ClaimsPrincipal user, ICollection<TEntity> entities)
    {
        //long currentUserId = 1;
        //if (user != null)
        //{
        //    currentUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //}
        //foreach (TEntity entity in entities)
        //{
        //    context.Entry(entity).State = EntityState.Modified;
        //}

        //var newentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        //newentries.ForEach(e =>
        //{
        //    e.Property("CreateDate").CurrentValue = DateTime.Now;
        //    e.Property("CreatorUserId").CurrentValue = currentUserId;
        //});

        //var editedentries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        //editedentries.ForEach(e =>
        //{
        //    e.Property("UpdateTime").CurrentValue = DateTime.Now;
        //    e.Property("UpdaterUserId").CurrentValue = currentUserId;
        //});

        await context.SaveChangesAsync();
        return entities;
    }

    public IDbContextTransaction BeginTransaction()
    {
        return context.Database.BeginTransaction();
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return context.Database.CreateExecutionStrategy();
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _dbSet.AsQueryable<TEntity>();
    }

    //public async Task<IQueryable<TEntity>> IsActive()
    //{
    //    return _dbSet.Where(e => e.IsActive);
    //}

    //public async Task<IQueryable<TEntity>> NotDeleted()
    //{
    //    return _dbSet.Where(e => !e.IsDeleted);
    //}

    //public async Task<IQueryable<TEntity>> NotDeletedIsActive()
    //{
    //    return _dbSet.Where(e => !e.IsDeleted && e.IsActive);
    //}
}
