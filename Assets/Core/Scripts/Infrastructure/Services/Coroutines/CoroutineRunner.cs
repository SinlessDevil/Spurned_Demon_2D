using UnityEngine;

namespace Infrastructure.Services.Coroutines
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameObject = new GameObject("Coroutine Runner");
                    _instance = gameObject.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(gameObject);
                }

                return _instance;
            }
        }
    }   
}
