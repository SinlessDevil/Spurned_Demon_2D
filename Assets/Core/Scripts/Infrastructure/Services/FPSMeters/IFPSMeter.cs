namespace Infrastructure.Services.FPSMeters
{
    public interface IFPSMeter
    {
        void Begin();
        void Break();
    }
}