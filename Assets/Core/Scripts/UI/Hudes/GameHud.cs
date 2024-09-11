using Controller.Joysticks;
using UnityEngine;

namespace UI.Hudes
{
    public class GameHud : Hud
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Joystick _joystick;
        
        public Joystick Joystick => _joystick;
        
        public void ToggleJoyStick(bool value)
        {
            _joystick.gameObject.SetActive(value);
        }
        
        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }
    }
}