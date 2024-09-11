using UnityEngine;

namespace Infrastructure.Services.Inputs
{
    public interface IInputService
    {
        float Horizontal { get; }
        float Vertical { get; }
        Vector2 Direction { get; }
        void SetInputDevice(IInputDevice inputDevice);
        void Cleanup();
    }
}