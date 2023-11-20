using System;
using UnityEngine;
using UnityEngine.UI;
using UI.Buttons.Animation;
using Extensions;
using DG.Tweening;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private SettingsContainer[] _containers;

        public bool IsActivated {  get; private set; }

        private SettingsContainer _currentContainer;

        private bool _initialized = false;

        private const float DuractionAnimation = 0.35f;

        #region Initialize Methods
        public void Initialize()
        {
            Asserts();

            SetUpStartParametrContainer();

            _currentContainer = _containers[0];

            _settingsPanel.SetActive(IsActivated);

            _initialized = true;

            SubscribeEvents();
        }
        private void Asserts()
        {
            _settingsPanel.LogErrorIfComponentNull();
            _containers.LogErrorIfComponentNull();
        }
        private void SetUpStartParametrContainer()
        {
            foreach (var container in _containers)
            {
                container.Panel.transform.localScale = Vector3.zero;
                container.Panel.transform.Deactivate();

                container.SettingsAnimationButton = container.Button.GetComponent<SettingsAnimationButton>();
            }
        }
        #endregion

        public void Show()
        {
            _settingsPanel.transform.Activate();

            SetStartContainer();

            IsActivated = true;
        }
        public void Hide()
        {
            _settingsPanel.transform.Deactivate();

            IsActivated = false;
        }

        private void SetStartContainer()
        {
            _currentContainer.SettingsAnimationButton.SetAniamtionRelease();

            _currentContainer.Panel.transform.Activate();

            _currentContainer.Panel.transform.DOScale(Vector3.one, DuractionAnimation)
                .ChangeStartValue(Vector3.zero)
                .SetEase(Ease.OutBack);
        }

        #region Handle Button on Container Settings Methods
        private void OnEnable()
        {
            if (_initialized)
                SubscribeEvents();
        }
        private void OnDisable()
        {
            if(_initialized)
                UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            foreach (SettingsContainer conteiner in _containers)
            {
                conteiner.Button.onClick.AddListener(() => ShowContainer(conteiner));
            }
        }
        private void UnsubscribeEvents()
        {
            foreach (SettingsContainer conteiner in _containers)
            {
                conteiner.Button.onClick.RemoveListener(() => ShowContainer(conteiner));
            }
        }
        private void ShowContainer(SettingsContainer container)
        {  
            if(container == _currentContainer)
                return;

            HideContainer(_currentContainer);

            container.Panel.transform.Activate();

            container.Panel.transform.DOScale(Vector3.one, DuractionAnimation)
                .ChangeStartValue(Vector3.zero)
                .SetEase(Ease.OutBack);

            _currentContainer = container;
        }
        private void HideContainer(SettingsContainer container)
        {
            container.Panel.transform.DOScale(Vector3.zero, DuractionAnimation / 2).ChangeStartValue(Vector3.one).OnComplete(() =>
            {
                container.Panel.transform.Deactivate();
            });

            container.SettingsAnimationButton.ResetAniamtionRelease();
        }
        #endregion
    }

    [Serializable]
    public class SettingsContainer
    {
        [field: SerializeField] public GameObject Panel { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }

        public SettingsAnimationButton SettingsAnimationButton { get; set; }
    }
}
