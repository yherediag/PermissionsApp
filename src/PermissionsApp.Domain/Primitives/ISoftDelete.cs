namespace PermissionsApp.Domain.Primitives;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
}
