using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TraderMoodIcon : MonoBehaviour
    {
        public static TraderMoodIcon Singleton;

        [SerializeField]
        Vector3 startPosition;

        [SerializeField]
        Vector3 endPosition;

        [SerializeField]
        Sprite moodVeryHappy;

        [SerializeField]
        Sprite moodHappy;

        [SerializeField]
        Sprite moodNeutral;

        [SerializeField]
        Sprite moodAngry;

        [SerializeField]
        Text irritationText;

        [SerializeField]
        Text skepticismText;

        Image image;

        float lerpT = 0;

        private void Awake()
        {
            image = GetComponent<Image>();
            transform.position = startPosition;
            Singleton = this;
        }

        private void Update()
        {
            lerpT += Time.deltaTime / 3;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, lerpT);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - lerpT);
            irritationText.color = new Color(irritationText.color.r, irritationText.color.g, irritationText.color.b, 1 - lerpT);
            skepticismText.color = new Color(skepticismText.color.r, skepticismText.color.g, skepticismText.color.b, 1 - lerpT);

            if (lerpT > 1)
                enabled = false;
        }

        public void UpdateMood()
        {
            float bigger = 0;

            if (Trader.CurrentTrader != null)
            {
                if (Trader.CurrentTrader.IrritationTotal > Trader.CurrentTrader.SkepticismTotal)
                    bigger = Trader.CurrentTrader.IrritationTotal;
                else
                    bigger = Trader.CurrentTrader.SkepticismTotal;
            }

            if (bigger < 20)
                image.sprite = moodVeryHappy;
            else if (bigger < 45)
                image.sprite = moodHappy;
            else if (bigger < 75)
                image.sprite = moodNeutral;
            else
                image.sprite = moodAngry;

            irritationText.text = ((int)Trader.CurrentTrader.IrritationTotal).ToString();
            skepticismText.text = ((int)Trader.CurrentTrader.SkepticismTotal).ToString();

            lerpT = 0;
            transform.localPosition = startPosition;
            enabled = true;
        }
    }
}
