using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Clinics;

public static class ClinicErrors
{
    public static readonly Error NotFound = new(
        "Clinic.NotFound",
        "O apartamento com o identificador especificado não foi encontrado");
}