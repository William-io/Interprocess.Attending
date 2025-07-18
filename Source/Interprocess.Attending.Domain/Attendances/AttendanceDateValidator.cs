namespace Interprocess.Attending.Domain.Attendances;

public static class AttendanceDateValidator
{
    /// <summary>
    /// Valida se a data e hora do atendimento não está no futuro
    /// </summary>
    /// <param name="attendanceDateTime">Data e hora do atendimento</param>
    /// <exception cref="ArgumentException">Lançada quando a data está no futuro</exception>
    public static void ValidateAttendanceDateTime(DateTime attendanceDateTime)
    {
        var currentDateTime = DateTime.UtcNow;

        if (attendanceDateTime > currentDateTime)
        {
            throw new ArgumentException(
                $"Não é permitido registrar atendimentos com data e hora no futuro. " +
                $"Data informada: {attendanceDateTime:dd/MM/yyyy HH:mm:ss}, " +
                $"Data atual: {currentDateTime:dd/MM/yyyy HH:mm:ss}");
        }
    }
}