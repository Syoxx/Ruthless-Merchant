using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class ItemSetter : MonoBehaviour
    {
        [SerializeField]
        int value;

        [SerializeField]
        Text valueText;

        public void SetTrade(Trade trade, int value = 0)
        {
            // TODO: Change this to currency class
            Material currencyItem = gameObject.AddComponent<Material>();

            currencyItem.ItemType = ItemType.Other;

            if (value == 0)
                value = this.value;

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

            valueText.text = value.ToString();
        }
    }
}
