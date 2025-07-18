using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients.Events;

namespace Interprocess.Attending.Domain.Patients;

public sealed class Patient : Entity
{
    private Patient(
        Guid id,
        string name,
        string cpf,
        string dateBirth,
        Sex sex,
        PatientStatus status, 
        Address address) : base(id)
    {
        Name = name;
        Cpf = cpf;
        DateBirth = dateBirth;
        Sex = sex;
        Status = status;
        Address = address;
    }
    
    private Patient()
    {
    }

    public string Name { get; private set; } = null!;
    public string Cpf { get; private set; } = null!;
    public string DateBirth { get; private set; } = null!;
    public Sex Sex { get; private set; }
    public PatientStatus Status { get; private set; }
    public Address Address { get; private set; } = null!;
    
    public static async Task<Patient> CreateAsync(
        string name,
        string cpf,
        string dateBirth,
        Sex sex,
        PatientStatus status,
        Address address,
        IPatientRepository patientRepository)
    {
        // Validação para não permitir CPF duplicado
        await PatientValidator.ValidateUniqueCpfAsync(cpf, patientRepository);
        
        var patient = new Patient(
            Guid.NewGuid(), 
            name,
            cpf,
            dateBirth,
            sex,
            status,
            address);
        
        patient.AddDomainEvent(new PatientCreatedDomainEvent(patient.Id));

        return patient;
    }

    public void Update(
        string name,
        string dateBirth,
        Sex sex,
        Address address)
    {
        Name = name;
        DateBirth = dateBirth;
        Sex = sex;
        Address = address;
    }

    public void Inactivate()
    {
        Status = PatientStatus.Inactive;
    }

    public void Activate()
    {
        Status = PatientStatus.Active;
    }
    
}