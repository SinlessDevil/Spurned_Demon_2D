using System;
using System.IO;
using UnityEngine;

namespace StorageService
{
    public class JsonToFileStorageService : IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null)
        {
            string path = BuildPath(key);
            string json = JsonUtility.ToJson(data);

            try
            {
                File.WriteAllText(path, json);
                callback?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to file: {e}");
                callback?.Invoke(false);
            }
        }

        public void Load<T>(string key, Action<T> callback)
        {
            string path = BuildPath(key);

            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    T data = JsonUtility.FromJson<T>(json);
                    callback?.Invoke(data);
                }
                else
                {
                    Debug.LogWarning($"File not found at path: {path}");
                    //   callback?.Invoke(default);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data from file: {e}");
                //  callback?.Invoke(default);
            }
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, $"{key}.json");
        }
    }
}