using UnityEngine;

namespace Extensions
{
    public static class RandExtensions
    {
        public static bool Chance(this bool value, int percent)
        {
            var rand = Random.Range(0, 11);
            var isChance = rand < percent;
            Debug.Log("Value: " + rand + " Percent: " + percent + " = " + isChance);
            return isChance;
        }
    }
}