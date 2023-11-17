using UnityEngine;
using UnityEngine.UI;
using Extensions;
using TMPro;
using DG.Tweening;

namespace UI.Buttons.Animation
{
    public class SettingsAnimationButton : BaseAnimationButton
    {
        [SerializeField] private Color _colorButtonUnselected;
        [SerializeField] private Color _colorButtonSelected;

        private bool _isActive;

        private Image _bgImage;
        private TMP_Text _text;

        private const float DuractionAnimation = 0.25f;

        protected override void Awake()
        {
            base.Awake();

            SetUpStartMode();
        }
        protected override void InitComponent()
        {
            _bgImage = transform.GetChild(0).GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();

            base.InitComponent();
        }
        protected override void Asserts()
        {
            _bgImage.LogErrorIfComponentNull();
            _text.LogErrorIfComponentNull();

            base.Asserts();
        }
        private void SetUpStartMode()
        {
            transform.localScale = Vector3.one;
        }

        protected override void OnClick()
        {
            _bgImage.DOColor(_colorButtonUnselected, 0.1f).OnComplete(() =>
            {
                _bgImage.DOColor(_colorButtonSelected, 0.1f);
            });
        }
        protected override void OnRelease()
        {
            if (_isActive == true)
                return;

            SetAniamtionRelease();
        }
        protected override void OnDeselect()
        {
            transform.DOScale(1, DuractionAnimation);
        }
        protected override void OnSelect()
        {
            transform.DOScale(1.2f, DuractionAnimation);
        }

        public void ResetAniamtionRelease()
        {
            _bgImage.DOFillAmount(0f, DuractionAnimation).ChangeStartValue(1f);
            _bgImage.DOColor(_colorButtonUnselected, DuractionAnimation);

            _button.image.DOColor(_colorButtonUnselected, DuractionAnimation);

            _isActive = false;
        }
        public void SetAniamtionRelease()
        {
            _bgImage.DOFillAmount(1f, DuractionAnimation).ChangeStartValue(0f);
            _bgImage.DOColor(_colorButtonSelected, DuractionAnimation);

            _button.image.DOColor(_colorButtonSelected, DuractionAnimation);

            _isActive = true;
        }
    }
}
