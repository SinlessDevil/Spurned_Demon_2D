using UnityEngine;

namespace Infrastructure.Services.Inputs
{
    public interface IInputDevice
    {
        float Horizontal { get; }
        float Vertical { get; }
        Vector2 Direction { get; }
    }
}