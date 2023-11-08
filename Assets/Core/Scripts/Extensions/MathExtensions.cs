using UnityEngine;

namespace Extensions
{
    public static class MathExtensions
    {
        public static float RoundToDecimal(float value, int decimalPlaces)
        {
            float multiplier = Mathf.Pow(10f, decimalPlaces);
            return Mathf.Round(value * multiplier) / multiplier;
        }

        public static float DistanceBetweenPoints(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b);
        }

        public static Vector3 DirectionBetweenPoints(Vector3 a, Vector3 b)
        {
            return (b - a).normalized;
        }

        public static Vector3 MidpointBetweenPoints(Vector3 a, Vector3 b)
        {
            return (a + b) / 2f;
        }

        public static float MapValue(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
        }
    }
}