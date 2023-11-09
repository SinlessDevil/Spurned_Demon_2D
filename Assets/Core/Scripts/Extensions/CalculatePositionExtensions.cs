using UnityEngine;

namespace Extensions
{
    public static class CalculatePositionExtensions
    {
        public static Vector3 GetPositionInCircleHorizontal(this Transform transform)
        {
            float spawnDistance = Random.Range(1.5f, 2f);
            Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
            Vector3 spawnPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + transform.position;
            return spawnPosition;
        }

        public static Vector3 GetPositionInCircleVertical(this Transform transform)
        {
            float spawnDistance = Random.Range(1.5f, 2f);
            Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
            Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y) + transform.position;
            return spawnPosition;
        }

        public static Vector3 GetPositionInCircleVertical(this Vector3 position)
        {
            float spawnDistance = Random.Range(0.75f, 1.25f);
            Vector3 randomPosition = Random.insideUnitCircle * spawnDistance;
            Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y) + position;
            return spawnPosition;
        }
    }
}