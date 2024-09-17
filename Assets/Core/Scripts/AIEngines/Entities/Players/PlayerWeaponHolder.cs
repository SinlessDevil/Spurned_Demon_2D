using Core.Scripts.AIEngines.Entities.Items.Weapons;
using Infrastructure.Services.StaticData;
using Zenject;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class PlayerWeaponHolder : WeaponHolder
    {
        private IStaticDataService _staticDataService;
        
        [Inject]
        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            EquipWeapon(_staticDataService.Balance.DefaultItem.ItemID);
        }
    }
}
