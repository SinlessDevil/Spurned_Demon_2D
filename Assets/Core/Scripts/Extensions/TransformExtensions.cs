using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void Destroy(this Transform transform)
        {
            Object.Destroy(transform.gameObject);
        }
        public static void Activate(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }
        public static void Deactivate(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static void SpawnObjects(this Transform transform, GameObject[] objectPrefabs, int spawnCount)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var rand = Random.Range(0, objectPrefabs.Length);
                float spawnDistance = Random.Range(2f, 3f);

                InstantiateObject(objectPrefabs[rand], transform, spawnDistance);
            }
        }
        public static void SpawnObjects(this Transform transform, GameObject objectPrefab, int spawnCount)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                float spawnDistance = Random.Range(1.5f, 2.5f);

                InstantiateObject(objectPrefab, transform, spawnDistance);
            }
        }

        public static T[] SpawnObjects<T>(this Transform transform, GameObject objectPrefab, int spawnCount)
        {
            T[] spawnedObjects = new T[spawnCount];

            for (int i = 0; i < spawnCount; i++)
            {
                float spawnDistance = Random.Range(1.5f, 2.5f);
                Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
                Vector3 spawnPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + transform.position;

                GameObject spawnedObject = GameObject.Instantiate(objectPrefab, spawnPosition, Quaternion.identity, null);
                T component = spawnedObject.GetComponent<T>();

                if (component != null)
                {
                    spawnedObjects[i] = component;
                }
                else
                {
                    Debug.LogError("Component of type " + typeof(T) + " not found on spawned object.");
                }
            }

            return spawnedObjects;
        }
        public static T[] SpawnObjects<T>(this Transform transform, GameObject[] objectPrefab, int spawnCount)
        {
            T[] spawnedObjects = new T[spawnCount];

            for (int i = 0; i < spawnCount; i++)
            {
                float spawnDistance = Random.Range(1.5f, 2.5f);
                int randObject = Random.Range(0, objectPrefab.Length);
                Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
                Vector3 spawnPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + transform.position;

                GameObject spawnedObject = GameObject.Instantiate(objectPrefab[randObject], spawnPosition, Quaternion.identity, null);
                T component = spawnedObject.GetComponent<T>();

                if (component != null)
                {
                    spawnedObjects[i] = component;
                }
                else
                {
                    Debug.LogError("Component of type " + typeof(T) + " not found on spawned object.");
                }
            }

            return spawnedObjects;
        }

        public static T SpawnObject<T>(this Transform transform, GameObject objectPrefab) where T : MonoBehaviour
        {
            float spawnDistance = Random.Range(1.5f, 2f);

            GameObject spawnedObject = InstantiateObject(objectPrefab, transform, spawnDistance);

            T component = spawnedObject.GetComponent<T>();
            return component;
        }

        private static GameObject InstantiateObject(GameObject prefab, Transform transform, float spawnDistance)
        {
            Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
            Vector3 spawnPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + transform.position;
            return GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}