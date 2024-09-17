using System.Linq;
using Infrastructure.StaticData.ItemObjects;
using JetBrains.Annotations;
using Utilities.Attributes;

namespace Utilities.Editor
{
    [UsedImplicitly]
    public class ItemIdSelectorAttributeDrawer : IdSelectorAttributeDrawer<ItemIdSelectorAttribute, ItemObjectData>
    {
        protected override string[] GetIds(ItemObjectData[] data)
        {
            return data.Select(x => x.ItemID).ToArray();
        }

        protected override string GetPropertyName() => "Item Id";
    }
}