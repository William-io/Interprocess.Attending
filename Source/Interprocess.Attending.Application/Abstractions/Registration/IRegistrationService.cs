namespace Interprocess.Attending.Application.Abstractions.Registration;

public interface IRegistrationService
{
    Task SendRegisterConfirmationAsync(string cpf, string message);
}