using Infrastructure.Services.AudioService;
using Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapAudioState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IAudioClipsService _audioClipsService;

        private const string NameAudioManager = "AudioManager";

        [Inject]
        public BootstrapAudioState(IStateMachine<IGameState> gameStateMachine, IAudioClipsService audioClipsService, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _audioClipsService = audioClipsService;
        }
    
        public void Enter(string payload)
        {
            var sounds = _staticDataService.AudioConfig.Sounds.ToArray();

            var audioManagerObject = CreateGameObejctAudioManager();

            AddComponentsSound(sounds, audioManagerObject);

            InitSoundsInService(sounds);

            _gameStateMachine.Enter<LoadLevelState, string>(payload);
        }
        private GameObject CreateGameObejctAudioManager()
        {
            var audioManagerObject = new GameObject(NameAudioManager);
            Object.DontDestroyOnLoad(audioManagerObject);
            return audioManagerObject;
        }
        private void AddComponentsSound(Sound[] sounds, GameObject gameObject)
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }
        private void InitSoundsInService(Sound[] sounds)
        {
            _audioClipsService.Sounds = sounds;
        }

        public void Exit()
        {

        }
    }
}