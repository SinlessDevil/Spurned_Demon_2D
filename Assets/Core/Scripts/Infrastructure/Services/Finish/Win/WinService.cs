using Infrastructure.Services.Window;
using UI.Window;

namespace Infrastructure.Services.Finish.Win
{
    public class WinService : IWinService
    {
        private IWindowService _windowService;

        
        public WinService(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        public void Win()
        {
            _windowService.Open(WindowTypeId.Win);
        }
    }
}