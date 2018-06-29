using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuthlessMerchant
{
    public class ItemSetter : MonoBehaviour
    {
        public static  ItemSetter Singleton;

        [SerializeField]
        int value;

        void Awake()
        {
            Singleton = this;
        }

        public void SetTrade()
        {
            Trade trade = Trade.Singleton;

            Item currencyItem = trade.gameObject.AddComponent<Item>();
            currencyItem.Type = ItemType.Other;

            ItemValue itemValue = new ItemValue()
            {
                Item = currencyItem,
                Count = value
            };

            ItemValue[] itemValueArray = new ItemValue[1];
            itemValueArray[0] = itemValue;

            Item itemToTrade = trade.gameObject.AddComponent<Item>();
            itemToTrade.ItemValue = itemValueArray;

            trade.Item = itemToTrade;
        }
    }
}
