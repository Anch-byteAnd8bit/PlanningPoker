using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core;

public interface IEntityFactoryService {

    IEntityRepository<TEntity> Get<TEntity>()
        where TEntity : class, IEntity;

    IEntityRepository<TEntity, TId> Get<TEntity, TId>()
        where TEntity : class, IEntity, IEntity<TId> where TId : IComparable<TId>;

}

public interface IEntityRepository<TEntity>
    where TEntity : class, IEntity {

    IQueryable<TEntity> Query(bool readOnly = true);

    Task<TEntity> InsertAsync(TEntity entity, bool eager = false, Cancellation c = default);

}

public interface IEntityRepository<TEntity, in TId> : IEntityRepository<TEntity>
    where TEntity : class, IEntity<TId>, IEntity where TId : IComparable<TId> {

    Task<TEntity?> SingleOrDefaultAsync(TId id, Cancellation c = default);

}

public static class EntityRepositoryExtensions {

    public static Task<Maybe<TEntity>> SingleOrNoneAsync<TEntity, TId>(
        this IEntityRepository<TEntity, TId> repository, TId id, Cancellation c = default
    ) where TEntity : class, IEntity<TId>, IEntity where TId : IComparable<TId> {
        return repository.SingleOrDefaultAsync(id, c).AsMaybe();
    }

    public static Task<Result<TEntity>> SingleOrFailureAsync<TEntity, TId>(
        this IEntityRepository<TEntity, TId> repository, TId id, Cancellation c = default
    ) where TEntity : class, IEntity<TId>, IEntity where TId : IComparable<TId> {
        return repository.SingleOrNoneAsync(id, c).ToResult(IEntity.Error.NotFound);
    }

}

public static class QueryableExtensions {

    public static Task<Maybe<TEntity>> SingleOrNoneAsync<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate, Cancellation c = default
    ) where TEntity : class, IEntity {
        return query.SingleOrDefaultAsync(predicate, c).AsMaybe();
    }

    public static Task<Maybe<TEntity>> SingleOrNoneAsync<TEntity, TId>(
        this IQueryable<TEntity> query, TId id, Cancellation c = default
    ) where TEntity : class, IEntity<TId>, IEntity where TId : IComparable<TId> {
        return query.SingleOrNoneAsync(x => Equals(x.Id, id), c);
    }

    public static Task<Result<TEntity>> SingleOrFailureAsync<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> predicate,
        string error = IEntity.Error.NotFound,
        Cancellation c = default
    ) where TEntity : class, IEntity {
        return query.SingleOrNoneAsync(predicate, c).ToResult(error);
    }

    public static Task<Result<TEntity>> SingleOrFailureAsync<TEntity, TId>(
        this IQueryable<TEntity> query,
        TId id,string error = IEntity.Error.NotFound,
        Cancellation c = default
    ) where TEntity : class, IEntity<TId>, IEntity where TId : IComparable<TId> {
        return query.SingleOrNoneAsync(x => Equals(x.Id, id) , c).ToResult(error);
    }

}


public interface IEntity : IComparable {
    public static class Error {
        public const string NotFound = "A single Entiy has not been found";
    }
}

public interface IEntity<TId> : IComparable where TId : IComparable<TId> {
    TId? Id { get; }
}

public abstract class Entity<TId> : IEntity, IEntity<TId> where TId : IComparable<TId> {

    public virtual TId? Id { get; protected set; }

    public int CompareTo(object? obj) {
        throw new NotImplementedException();
    }

}

public abstract class EntityAggregate<TId> : Entity<TId> where TId : IComparable<TId> {

    public virtual void Dispatch<TEntityEvent>(TEntityEvent entityEvent) where TEntityEvent : IEntityEvent {
        throw new NotImplementedException();
    }

}

public interface IEntityEvent;
