namespace HandleMessage.Services;

public interface IService<T>
{
    public IEnumerable<T> findAll();
}