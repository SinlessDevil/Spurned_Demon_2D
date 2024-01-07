using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.StaticData;
using Infrastructure.StaticData;
using UnityEngine;

namespace Infrastructure.Services.FxEffect
{
    public class FxEffectService : IFxEffectService
    {
        private IStaticDataService _staticDataService;
        private IGameFactory _gameFactory;

        public FxEffectService(IStaticDataService staticDataService, IGameFactory gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
        }
        
        public ParticleSystem SpawnFxByVector(FxTypeId fxTypeId, Vector3 spawnPos)
        {
           var fxEffcet = _gameFactory.CreateFxEffect(fxTypeId, spawnPos);
           return fxEffcet;
        }

        public ParticleSystem SpawnFxByVectorRandom(FxTypeId fxTypeId, Vector3 spawnPos)
        {
            var fxEffcet = _gameFactory.CreateFxEffect(fxTypeId, spawnPos);
            fxEffcet.transform.position +=
                new Vector3(UnityEngine.Random.Range(0.25f, 3f), UnityEngine.Random.Range(0.25f, 3f), 0);
            return fxEffcet;
        }

        public ParticleSystem SpawnFxByVectorOffset(FxTypeId fxTypeId, Vector3 spawnPos, Vector3 offset)
        {
            var fxEffcet = _gameFactory.CreateFxEffect(fxTypeId, spawnPos);
            fxEffcet.transform.position += offset;
            return fxEffcet;
        }

        public ParticleSystem SpawnFxByTransform(FxTypeId fxTypeId, Transform transform)
        {
            var fxEffcet = _gameFactory.CreateFxEffect(fxTypeId, transform.position);
            return fxEffcet;
        }
    }
}