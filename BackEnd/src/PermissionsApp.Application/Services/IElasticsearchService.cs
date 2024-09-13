namespace PermissionsApp.Application.Services;

public interface IElasticsearchService<T> where T : class
{
    Task RequestOrModify(int id, T document);
}
