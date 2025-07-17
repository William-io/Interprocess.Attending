namespace Interprocess.Attending.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    protected Entity(Guid id)
    {
        Id = id;
    }
    
    protected Entity()
    {
    }
    
    public Guid Id { get; init; }
    
    /*
     * metodos de gestao dos eventos vinculados a entidade
     */
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
}