using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        public event Action OnShowLoadingCurtainEvent;
        public event Action OnHideLoadingCurtainEvent;
    
        public Image Image;
        public float MoveUpSpeed = 20f;
        public float TimeStep = 0.03f;

        private Coroutine _upCoroutine;

        private const float Delay = 0.5f;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            EnsureCoroutineStopped();
            Image.rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            EnsureCoroutineStopped();
            _upCoroutine = StartCoroutine(GoUp());
        }

        private IEnumerator GoUp()
        {
            yield return new WaitForSeconds(Delay);
            
            while (Image.rectTransform.anchoredPosition.y < Image.rectTransform.rect.height)
            {
                MoveImageUp();
                yield return new WaitForSeconds(TimeStep);
            }

            OnHideLoadingCurtainEvent?.Invoke();

            _upCoroutine = null;
            gameObject.SetActive(false);
        }

        private void MoveImageUp()
        {
            RectTransform imageTransform = Image.rectTransform;
            Vector2 anchoredPosition = imageTransform.anchoredPosition;

            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + MoveUpSpeed);

            imageTransform.anchoredPosition = anchoredPosition;

            OnShowLoadingCurtainEvent?.Invoke();
        }

        private void EnsureCoroutineStopped()
        {
            if (_upCoroutine != null)
                StopCoroutine(_upCoroutine);
        }
    }
}