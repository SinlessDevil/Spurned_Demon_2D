using System.Linq;
using Infrastructure.Services.StaticData;
using Infrastructure.StaticData.ItemObjects;
using Core.Scripts.AIEngines.Entities.Items;
using Core.Scripts.AIEngines.Entities.Items.Weapons;
using Core.Scripts.AIEngines.Entities.Items.Weapons.Types;
using Zenject;

namespace Infrastructure.Services.Factories.Items
{
    public class ItemFactory : Factory, IItemFactory
    {
        private IInstaller _installer;
        private IStaticDataService _staticDataService;
        
        public ItemFactory(
            IInstantiator instantiator, 
            IStaticDataService staticDataService) : base(instantiator)
        {
            _staticDataService = staticDataService;
        }
        
        public Item CreateItem(string id)
        {
            var allItems = _staticDataService.AllItemsObject;
            var itemData = allItems.FirstOrDefault(item => item.ItemID == id);
            var item = Instantiate(itemData.ItemPrefab.gameObject).gameObject.GetComponent<Item>();
            return item;
        }

        public Weapon CreateWeapon(string id)
        {
            var allItems = _staticDataService.AllItemsObject;
            var itemData = allItems.FirstOrDefault(item => item.ItemID == id);
            var weaponData = itemData as WeaponObjectData;

            var weapon = Instantiate(weaponData.ItemPrefab.gameObject).gameObject.GetComponent<Weapon>();

            switch (weaponData)
            {
                case SwordObjectData swordObjectData:
                    var sword = weapon as Sword;
                    sword.Initialize(swordObjectData.Damage, swordObjectData.AttackSpeed);
                    return sword;
            }

            return null;
        }
    }
}