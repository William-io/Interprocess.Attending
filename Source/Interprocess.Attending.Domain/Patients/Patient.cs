using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients.Events;

namespace Interprocess.Attending.Domain.Patients;

public sealed class Patient : Entity
{
    private Patient(
        Guid id,
        FirstName? firstName,
        LastName? lastName,
        Document cpf,
        string dateBirth,
        Sex sex,
        PatientStatus status, 
        Address address) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Cpf = cpf;
        DateBirth = dateBirth;
        Sex = sex;
        Status = status;
        Address = address;
    }
    
    private Patient()
    {
        Cpf = null!;
        DateBirth = null!;
        Address = null!;
    }

    public FirstName? FirstName { get; private set; }
    public LastName? LastName { get; private set; }
    public Document Cpf { get; private set; }
    public string DateBirth { get; private set; }
    public Sex Sex { get; private set; }
    public PatientStatus Status { get; private set; }
    public Address Address { get; private set; }
    
    /*
     * Utilizando padrao factory para criar pacientes
     */
    
    public static async Task<Patient> CreateAsync(
        FirstName firstName,
        LastName lastName,
        Document cpf,
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
            firstName,
            lastName,
            cpf,
            dateBirth,
            sex,
            status,
            address);
        
        patient.AddDomainEvent(new PatientCreatedDomainEvent(patient.Id));

        return patient;
    }

    public void Update(
        FirstName firstName,
        LastName lastName,
        string dateBirth,
        Sex sex,
        Address address)
    {
        FirstName = firstName;
        LastName = lastName;
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