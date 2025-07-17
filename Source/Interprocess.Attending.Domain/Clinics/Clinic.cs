using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Clinics;

public sealed class Clinic : Entity
{
    public Clinic(Guid id, string name) : base(id)
    {
        Name = name;
    }
    
    private Clinic() { }

    public string Name { get; private set; }
}