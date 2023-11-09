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

        [SerializeField] private Image _image;
        
        private float _moveUpSpeed = 15f;
        private float _timeStep = 0.01f;

        private Coroutine _upCoroutine;

        private const float Delay = 1f;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            EnsureCoroutineStopped();

            _image.rectTransform.anchoredPosition = Vector2.zero;

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
            
            while (_image.rectTransform.anchoredPosition.y < _image.rectTransform.rect.height)
            {
                MoveImageUp();
                yield return new WaitForSeconds(_timeStep);
            }

            OnHideLoadingCurtainEvent?.Invoke();

            _upCoroutine = null;

            gameObject.SetActive(false);
        }

        private void MoveImageUp()
        {
            RectTransform imageTransform = _image.rectTransform;
            Vector2 anchoredPosition = imageTransform.anchoredPosition;

            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + _moveUpSpeed);

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