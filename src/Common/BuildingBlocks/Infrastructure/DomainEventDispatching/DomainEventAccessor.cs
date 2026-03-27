using BuildingBlock.Domain;
using BuildingBlock.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DomainEventDispatching;

public class DomainEventAccessor : IDomainEventAccessor
{
    private readonly DbContext _dbContext;

    public DomainEventAccessor(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var domainEvents = _dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(entity => entity.Entity.DomainEvents != null 
                             && entity.Entity.DomainEvents.Any()).ToList();
        
        return domainEvents
            .SelectMany(entity => entity.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvents());    }
}