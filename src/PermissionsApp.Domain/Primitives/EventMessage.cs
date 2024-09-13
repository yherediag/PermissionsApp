using PermissionsApp.Domain.Enums;

namespace PermissionsApp.Domain.Primitives;

public class EventMessage(Operation operation)
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;
    public string Operation { get; } = operation.ToString();
}
