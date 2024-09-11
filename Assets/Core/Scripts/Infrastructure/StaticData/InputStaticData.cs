using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/InputStaticData", fileName = "InputConfig", order = 4)]
    public class InputStaticData : ScriptableObject
    {
        public InputType InputType = InputType.Keyboard;
    }
    
    public enum InputType
    {
        Keyboard,
        Mouse,
        Gamepad
    }
}