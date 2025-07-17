using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Patients;

public static class PatientErros
{
    public static readonly Error NotFound = new(
        "Patient.Found",
        "O paciente com o identificador especificado não foi encontrado");

    public static readonly Error InvalidCredentials = new(
        "Patient.InvalidCredentials",
        "As credenciais fornecidas eram inválidas");

    //Paciente já está inativo
    public static readonly Error AlreadyInactive = new(
        "Patient.AlreadyInactive",
        "O paciente já está inativo");
}