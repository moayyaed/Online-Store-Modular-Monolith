namespace Common.Storage
{
    public interface IRequestStorage
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
    }
}