using System;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Game", fileName = "GameConfig", order = 0)]
    public class GameStaticData : ScriptableObject
    {
        public string InitialScene = "Initial";
        public string FirstScene = "First";
        public bool CanLoadCurrentOpenedScene = false;
        public LogStackTrace LogStackTraceDev = new LogStackTrace();
        public LogStackTrace LogStackTraceRelease = new LogStackTrace();
    }

    [Serializable]
    public class LogStackTrace
    {
        public StackTraceLogType Info = StackTraceLogType.None;
        public StackTraceLogType Errors = StackTraceLogType.ScriptOnly;
    }
}