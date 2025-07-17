using System;

namespace Interprocess.Attending.Domain.Patients;

public static class PatientValidator
{
    /// <summary>
    /// Valida se o CPF não está duplicado no sistema
    /// </summary>
    /// <param name="cpf">CPF a ser validado</param>
    /// <param name="patientRepository">Repositório de pacientes</param>
    /// <exception cref="ArgumentException">Lançada quando o CPF já existe</exception>
    public static async Task ValidateUniqueCpfAsync(Document cpf, IPatientRepository patientRepository)
    {
        var existingPatient = await patientRepository.GetByCpfAsync(cpf);
        
        if (existingPatient != null)
        {
            throw new ArgumentException($"Já existe um paciente cadastrado com o CPF: {cpf}");
        }
    }
}
