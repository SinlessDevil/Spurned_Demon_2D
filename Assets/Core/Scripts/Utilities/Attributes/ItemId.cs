using System;

namespace Utilities.Attributes
{
    [Serializable]
    public class ItemId
    {
        [ItemIdSelector(HasGameObjectField = true)]
        public string ItemID;
    }
}