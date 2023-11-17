using UnityEngine;
using UnityEngine.UI;
using Extensions;
using TMPro;
using DG.Tweening;

namespace UI.Buttons.Animation
{
    public class MenuAnimationButton : BaseAnimationButton
    {
        [SerializeField] private Color _colorButtonUnselected;
        [SerializeField] private Color _colorButtonSelected;

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

            _bgImage.transform.localScale = Vector3.zero;

            var newColor = _colorButtonUnselected;
            newColor.a = 0;
            _bgImage.color = newColor;
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

        }
        protected override void OnDeselect()
        {
            transform.DOScale(1, DuractionAnimation);
            _bgImage.transform.DOScale(Vector3.zero, DuractionAnimation);

            var newColor = _colorButtonUnselected;
            newColor.a = 0;
            _bgImage.DOColor(newColor, DuractionAnimation);
        }
        protected override void OnSelect()
        {
            transform.DOScale(1.2f, DuractionAnimation);
            _bgImage.transform.DOScale(Vector3.one, DuractionAnimation);
            _bgImage.DOColor(_colorButtonSelected, DuractionAnimation);
        }
    }
}
