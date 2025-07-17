using Interprocess.Attending.Application.Abstractions.Registration;
using Interprocess.Attending.Domain.Patients;

namespace Interprocess.Attending.Infrastructure.Registration;

internal sealed class RegistrationService : IRegistrationService
{
    public Task SendRegisterConfirmationAsync(string cpf, string message)
    {
        // Poderia ser um serviço de email, só uma notificação do CPF cadastrado.
        Console.WriteLine($"Registro confimado para CPF: {cpf}: {message}");
        
        // Simulate asynchronous operation
        return Task.CompletedTask;
    }
}