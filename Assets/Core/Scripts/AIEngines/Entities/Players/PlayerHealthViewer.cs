using System.Collections;
using System.Linq;
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
        [SerializeField] private Color _targetColor = Color.red;
        [SerializeField] private Color _defaultColor = Color.white;
        [Space(10)] [Header("Blink Settings")]
        [SerializeField] private int _countBlinks = 3;
        [SerializeField] private float _intervalBlinks = 0.1f;
        
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
            _health.AppliedDamageEvent += OnPlayBlinkedEffect;
        }
        private void Unsubscribe()
        {
            _health.AppliedDamageEvent -= OnPlayBlinkedEffect;
        }

        private void OnPlayBlinkedEffect(int health)
        {
            _coroutine ??= StartCoroutine(BlinkingEffectRoutine());
        }
        private IEnumerator BlinkingEffectRoutine()
        {
            var currentCountBlinks = _countBlinks;

            while (currentCountBlinks > 0)
            {
                _spriteRenderers.ToList().ForEach(spriteRenderer => 
                {
                    spriteRenderer.material = _targetMaterial;
                    spriteRenderer.color = _targetColor;
                });
                yield return new WaitForSeconds(_intervalBlinks);
                _spriteRenderers.ToList().ForEach(spriteRenderer => 
                {
                    spriteRenderer.material = _defaultMaterial;
                    spriteRenderer.color = _defaultColor;
                });
                yield return new WaitForSeconds(_intervalBlinks);
                currentCountBlinks--;
                yield return null;
            }
            
            _coroutine = null;
        }
    }
}