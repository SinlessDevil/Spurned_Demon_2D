using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.Coroutines
{
    public class CoroutineService : ICoroutineService
    {
        public Coroutine StartRoutine(IEnumerator routine)
        {
            return CoroutineRunner.Instance.StartCoroutine(routine);
        }

        public void StopRoutine(Coroutine routine)
        {
            CoroutineRunner.Instance.StopCoroutine(routine);
        }
    }   
}
