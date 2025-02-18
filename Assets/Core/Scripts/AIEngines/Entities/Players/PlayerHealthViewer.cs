using System.Linq;
using System.Collections;
using Core.Scripts.AIEngines.Healths;
using UnityEngine;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class PlayerHealthViewer : MonoBehaviour
    {
        [Space(10)] [Header("Sprite Renderers")] 
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [Space(10)] [Header("Materials")] 
        [SerializeField] private Material _targetMaterial;
        [SerializeField] private Material _defaultMaterial;
        [Space(10)] [Header("Colors")] 
        [SerializeField] private Color _damageColor = Color.red;
        [SerializeField] private Color _healColor = Color.green;
        [SerializeField] private Color _defaultColor = Color.white;
        [Space(10)] [Header("Blink Settings")] 
        [SerializeField] private int _countBlinks = 3;
        [SerializeField] private float _intervalBlinks = 0.1f;
        [Space(10)] [Header("VFX")]
        [SerializeField] private ParticleSystem _damageVFX;
        [SerializeField] private ParticleSystem _healVFX;

        private Coroutine _coroutine;

        private IHealth _health;

        public void Initialize(IHealth health)
        {
            _health = health;
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            _health = null;
        }

        private void Subscribe()
        {
            _health.AppliedDamageEvent += OnDamagePlayBlinkedEffect;
            _health.AppliedHealEvent += OnHealPlayBlinkedEffect;
        }
        
        private void Unsubscribe()
        {
            _health.AppliedDamageEvent -= OnDamagePlayBlinkedEffect;
            _health.AppliedHealEvent -= OnHealPlayBlinkedEffect;
        }

        private void OnDamagePlayBlinkedEffect(int health)
        {
            StopBlinkingCoroutine();
            _coroutine ??= StartCoroutine(BlinkingEffectRoutine(_countBlinks, _intervalBlinks, _damageColor));
            _damageVFX.Play();
        }
        
        private void OnHealPlayBlinkedEffect(int health)
        {
            StopBlinkingCoroutine();
            _coroutine ??= StartCoroutine(BlinkingEffectRoutine(_countBlinks, _intervalBlinks, _healColor));
            _healVFX.Play();
        }

        private IEnumerator BlinkingEffectRoutine(int countBlinks, float intervalBlinks, Color targetColor)
        {
            var currentCountBlinks = countBlinks;

            while (currentCountBlinks > 0)
            {
                _spriteRenderers.ToList().ForEach(spriteRenderer =>
                {
                    spriteRenderer.material = _targetMaterial;
                    spriteRenderer.color = targetColor;
                });
                yield return new WaitForSeconds(intervalBlinks);
                _spriteRenderers.ToList().ForEach(spriteRenderer =>
                {
                    spriteRenderer.material = _defaultMaterial;
                    spriteRenderer.color = _defaultColor;
                });
                yield return new WaitForSeconds(intervalBlinks);
                currentCountBlinks--;
                yield return null;
            }

            _coroutine = null;
        }

        private void StopBlinkingCoroutine()
        {
            if (_coroutine == null) 
                return;
            
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}