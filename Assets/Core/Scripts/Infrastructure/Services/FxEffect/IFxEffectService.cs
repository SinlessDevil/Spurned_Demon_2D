using Infrastructure.StaticData;
using UnityEngine;

namespace Infrastructure.Services.FxEffect
{
    public interface IFxEffectService
    {
        ParticleSystem SpawnFxByVector(FxTypeId fxTypeId, Vector3 spawnPos);
        ParticleSystem SpawnFxByVectorRandom(FxTypeId fxTypeId, Vector3 spawnPos);
        ParticleSystem SpawnFxByVectorOffset(FxTypeId fxTypeId, Vector3 spawnPos, Vector3 offset);
        ParticleSystem SpawnFxByTransform(FxTypeId fxTypeId, Transform transform);
    }
}