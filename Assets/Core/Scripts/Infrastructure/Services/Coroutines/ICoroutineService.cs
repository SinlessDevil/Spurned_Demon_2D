using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.Coroutines
{
    public interface ICoroutineService
    {
        Coroutine StartRoutine(IEnumerator routine);
        void StopRoutine(Coroutine routine);
    }   
}