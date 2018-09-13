using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace RuthlessMerchant
{
    public class VRSmithItem : MonoBehaviour
    {
        enum SmithItemType
        {
            MeltBox,
            Anvil,
            HotIron,
            TradeTable
        }

        [SerializeField]
        SmithItemType smithItemType;

        private void OnCollisionEnter(Collision collision)
        {
            switch (smithItemType)
            {
                case SmithItemType.MeltBox:
                    VRSmithing.Singleton.PlacingSingleIronsStep(collision);
                    break;

                case SmithItemType.Anvil:
                    VRSmithing.Singleton.PlacingMeltedIronStep(collision);
                    break;

                case SmithItemType.HotIron:
                    VRSmithing.Singleton.CreatingSwordStep(collision);
                    break;

                case SmithItemType.TradeTable:
                    VRSmithing.Singleton.PlacingSwordStep(collision);
                    break;
            }
        }
    }
}
