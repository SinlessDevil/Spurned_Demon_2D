using Infrastructure.Services.Window;
using UI.Window;

namespace Infrastructure.Services.Finish.Lose
{
    public class LoseService : ILoseService
    {
        private IWindowService _windowService;
        
        public LoseService(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        public void Lose()
        {
            _windowService.Open(WindowTypeId.Lose);
        }
    }   
}