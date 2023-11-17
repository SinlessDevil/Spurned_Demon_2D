using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
using Zenject;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _buttonPlay;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private Button _buttonExit;

        private bool _isInitialize;

        private Settings _settings;

        private const string NameGamePlayScene = "GamePlay";

        private IStateMachine<IGameState> _stateMachine;
        [Inject]
        public void Constract(IStateMachine<IGameState> stateMachine) => _stateMachine = stateMachine;

        public void Initialize(Settings settings)
        {
            _settings = settings;

            Asserts();

            _isInitialize = true;

            SubscribeEvents();
        }
        private void Asserts()
        {
            _buttonPlay.LogErrorIfComponentNull();
            _buttonSettings.LogErrorIfComponentNull();
            _buttonExit.LogErrorIfComponentNull();
        }

        private void OnEnable()
        {
            if (_isInitialize)
                SubscribeEvents();
        }
        private void OnDisable()
        {
            if (_isInitialize)
                UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _buttonPlay.onClick.AddListener(OnPlayGame);
            _buttonSettings.onClick.AddListener(OnShowSettings);
            _buttonExit.onClick.AddListener(OnExit);
        }
        private void UnsubscribeEvents()
        {
            _buttonPlay.RemoveListener(OnPlayGame);
            _buttonSettings.RemoveListener(OnShowSettings);
            _buttonExit.RemoveListener(OnExit);
        }

        private void OnPlayGame()
        {
            _stateMachine.Enter<DelegateStatesForSceneState, string>(NameGamePlayScene);
        }
        private void OnShowSettings()
        {
            if(_settings.IsActivated == false)
            {
                _settings.Show();
            }
            else
            {
                _settings.Hide();
            }
        }
        private void OnExit()
        {
            Application.Quit();
        }
    }
}