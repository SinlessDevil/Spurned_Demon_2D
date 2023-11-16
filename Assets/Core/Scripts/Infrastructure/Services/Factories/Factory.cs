using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Services.Factories
{
    public abstract class Factory
    {
        private readonly IInstantiator _instantiator;

        protected Factory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        protected T Instantiate<T>() =>
            _instantiator.Instantiate<T>();

        protected GameObject Instantiate(string resourcePath)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(string resourcePath, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(string resourcePath, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath, position, rotation, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(GameObject prefab)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(GameObject prefab, Transform parent) => 
            _instantiator.InstantiatePrefab(prefab, parent);

        protected GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab, position, rotation, parent);
            return MoveToCurrentScene(gameObject);
        }

        private GameObject MoveToCurrentScene(GameObject gameObject)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }
    }
}