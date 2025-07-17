namespace Interprocess.Attending.Application.Abstractions.Registration;

public interface IRegistrationService
{
    Task SendRegisterConfirmationAsync(Domain.Patients.Document cpf, string message);
}