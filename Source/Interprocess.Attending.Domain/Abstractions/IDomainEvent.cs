using MediatR;

namespace Interprocess.Attending.Domain.Abstractions;
// Representa um evento ocorrido no domínio.
// Onde sera notificado outras partes do sistema sobre alguma mudança.
public interface IDomainEvent : INotification
{
}