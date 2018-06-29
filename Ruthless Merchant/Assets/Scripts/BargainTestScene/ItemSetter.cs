using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ItemSetter : MonoBehaviour
    {
        [SerializeField]
        int value;

        public void SetTrade(Trade trade)
        {
            // TODO: Change this to currency class
            Material currencyItem = gameObject.AddComponent<Material>();

            currencyItem.itemType = ItemType.Other;

            ItemValue itemValue = new ItemValue()
            {
                Item = currencyItem,
                Count = value
            };

            ItemValue[] itemValueArray = new ItemValue[1];
            itemValueArray[0] = itemValue;

            Material itemToTrade = trade.gameObject.AddComponent<Material>();
            itemToTrade.ItemValue = itemValueArray;

            trade.Item = itemToTrade;
        }
    }
}
