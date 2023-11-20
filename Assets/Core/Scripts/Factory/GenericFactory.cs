using UnityEngine;

namespace Factory
{
    public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _pointToSpawn;
        [SerializeField] private Transform _centerPoint;

        public T GetNewInstance()
        {
            Vector3 pos = _pointToSpawn.position;
            Quaternion rotate = _centerPoint.rotation;
            return Instantiate(_prefab,pos, rotate);
        }
    }
}