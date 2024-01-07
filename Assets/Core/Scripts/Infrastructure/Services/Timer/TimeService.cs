using UnityEngine;

namespace Infrastructure.Services.Timer
{
    public class TimeService : MonoBehaviour, ITimeService
    {

        public void SlowMode() => Time.timeScale = 0.2f;

        public void SimpleMode() => Time.timeScale = 1f;

        public void Pause() => Time.timeScale = 0f;
    }   
}