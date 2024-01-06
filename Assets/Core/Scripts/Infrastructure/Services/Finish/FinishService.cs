using Infrastructure.Services.Finish.Lose;
using Infrastructure.Services.Finish.Win;

namespace Infrastructure.Services.Finish
{
    public class FinishService : IFinishService
    {
        private readonly IWinService _winService;
        private readonly ILoseService _loseService;
        
        public FinishService(IWinService winService, ILoseService loseService)
        {
            _winService = winService;
            _loseService = loseService;
        }

        public void Win() => _winService.Win();

        public void Lose() => _loseService.Lose();
    }
}