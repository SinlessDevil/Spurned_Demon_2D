using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Extensions;

namespace UI.Buttons.Animation
{
    public abstract class BaseAnimationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        public bool IsAnimating { get; private set; }

        protected Button _button;

        protected virtual void Awake()
        {
            InitComponent();
            Asserts();
        }
        protected virtual void InitComponent() => _button = GetComponent<Button>();
        protected virtual void Asserts() => _button.LogErrorIfComponentNull();

        public void OnPointerEnter(PointerEventData eventData) => OnSelect();
        public void OnPointerExit(PointerEventData eventData) => OnDeselect();
        public void OnPointerUp(PointerEventData eventData) => OnRelease();
        public void OnPointerDown(PointerEventData eventData) => OnClick();

        protected abstract void OnClick();
        protected abstract void OnRelease();
        protected abstract void OnSelect();
        protected abstract void OnDeselect();
    }
}