namespace StorageService
{
    public interface IStorageService
    {
        void Save(string key, object data, System.Action<bool> callback);
        void Load<T>(string key, System.Action<T> callback);
    }
}