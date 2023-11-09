using System;

namespace Infrastructure
{
    public interface ILoadingCurtain
    {
        event Action OnShowLoadingCurtainEvent;
        event Action OnHideLoadingCurtainEvent;

        void Show();
        void Hide();
    }
}