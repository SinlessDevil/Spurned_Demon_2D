using UnityEngine;

namespace Infrastructure.Services.Input
{
    public interface IInputDevice
    {
        float Horizontal { get; }
        float Vertical { get; }
        Vector2 Direction { get; }
    }
}