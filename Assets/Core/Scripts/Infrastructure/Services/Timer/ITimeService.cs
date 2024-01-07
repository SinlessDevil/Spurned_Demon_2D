namespace Infrastructure.Services.Timer
{
    public interface ITimeService
    {
        void SlowMode();
        void SimpleMode();
        void Pause();
    }   
}