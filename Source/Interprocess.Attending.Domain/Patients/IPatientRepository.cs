namespace Interprocess.Attending.Domain.Patients;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Patient?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
    
    // Listar pacientes com filtros opcionais por nome, CPF e/ou status
    Task<IEnumerable<Patient>> GetByFiltersAsync(string? name = null, string? cpf = null, PatientStatus? status = null, CancellationToken cancellationToken = default);
    void Update(Patient patient);
    void Add(Patient patient);
}