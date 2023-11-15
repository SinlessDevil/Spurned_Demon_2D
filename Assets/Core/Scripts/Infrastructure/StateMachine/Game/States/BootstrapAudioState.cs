using AudioService;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAudioState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IAudioClipsService _audioClipsService;

        private GameObject _audioManagerObject;
        private Sound[] _sounds;

        private const string NameAudionManager = "AudioManager";

        [Inject]
        public BootstrapAudioState(IStateMachine<IGameState> gameStateMachine, IAudioClipsService audioClipsService, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _audioClipsService = audioClipsService;
        }

        public void Enter(string payload)
        {
            InitGameObejctAudioManager();

            AddComponentsSound(_staticDataService.AudioConfig.Sounds.ToArray());

            InitSoundsInService();

            _gameStateMachine.Enter<LoadLevelState, string>(payload);
        }
        private void InitGameObejctAudioManager()
        {
            _audioManagerObject = new GameObject(NameAudionManager);
            Object.DontDestroyOnLoad(_audioManagerObject);
        }
        private void InitSoundsInService()
        {
            _audioClipsService.Sounds = _sounds;
        }
        private void AddComponentsSound(Sound[] sounds)
        {
            _sounds = sounds;

            foreach (Sound s in _sounds)
            {
                s.source = _audioManagerObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }


        }

        public void Exit()
        {

        }
    }
}