using Core.Scripts.AIEngines.Entities.Items;
using UnityEngine;

namespace Infrastructure.StaticData.ItemObjects
{
    public abstract class ItemObjectData : ScriptableObject
    {
        public Item ItemPrefab;
        public string ItemID;
    }
}