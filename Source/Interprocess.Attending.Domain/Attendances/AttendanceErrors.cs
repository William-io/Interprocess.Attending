using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Attendances;

public static class AttendanceErrors
{
    public static readonly Error NotActive = new(
        "AttendanceErrors.NotActive",
        "O Registro de atendimento não está ativo.");
        
    public static readonly Error CreationFailed = new(
        "Attendance.CreationFailed",
        "Falha ao criar o registro de atendimento.");
    
    public static readonly Error NotFound = new(
        "AttendanceErrors.NotFound",
        "Registro de atendimento não encontrado.");
    
    public static readonly Error FutureDate = new(
        "Attendance.FutureDate",
        "Não é possível criar atendimento com data futura");
}