using Core.Scripts.AIEngines.Entities.Items;
using Core.Scripts.AIEngines.Entities.Items.Weapons;

namespace Infrastructure.Services.Factories.Items
{
    public interface IItemFactory
    {
        Item CreateItem(string id);
        Weapon CreateWeapon(string id);
    }   
}