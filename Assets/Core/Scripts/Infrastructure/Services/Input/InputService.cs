using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private IInputDevice _inputDevice;
        
        public float Horizontal => _inputDevice.Horizontal;
        public float Vertical => _inputDevice.Vertical;
        public Vector2 Direction => _inputDevice.Direction;
        
        public void SetInputDevice(IInputDevice inputDevice)
        {
            _inputDevice = inputDevice ?? throw new ArgumentNullException(nameof(inputDevice));
        }

        public void Cleanup()
        {
            SetInputDevice(new NullableInputDevice());
        }
    } 
}