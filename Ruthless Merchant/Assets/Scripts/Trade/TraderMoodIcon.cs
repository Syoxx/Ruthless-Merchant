using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthlessMerchant
{
    public class TraderMoodIcon : MonoBehaviour
    {
        Vector3 startPosition;
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
            startPosition = new Vector3(0, 0.0824f);
            endPosition = new Vector3(0, 0.1128f);
        }

        private void Start()
        {
            transform.position = startPosition;
        }

        /// <summary>
        /// Moves the icon upwards and makes it fade out.
        /// </summary>
        private void Update()
        {
            lerpT += Time.deltaTime / 3;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, lerpT);
            transform.LookAt(Player.Singleton.transform);

            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - lerpT);
            irritationText.color = new Color(irritationText.color.r, irritationText.color.g, irritationText.color.b, 1 - lerpT);
            skepticismText.color = new Color(skepticismText.color.r, skepticismText.color.g, skepticismText.color.b, 1 - lerpT);

            if (lerpT > 1)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Sets the icon's mood and enables its movement. 
        /// </summary>
        /// <param name="trader"></param>
        public void SetIconMood(Trader trader)
        {
            float bigger = 0;

            if (trader.IrritationTotal > trader.SkepticismTotal)
                bigger = trader.IrritationTotal;
            else
                bigger = trader.SkepticismTotal;

            if (bigger < 25)
                image.sprite = moodVeryHappy;
            else if (bigger < 50)
                image.sprite = moodHappy;
            else if (bigger < 75)
                image.sprite = moodNeutral;
            else
                image.sprite = moodAngry;

            irritationText.text = ((int)trader.IrritationTotal).ToString();
            skepticismText.text = ((int)trader.SkepticismTotal).ToString();

            lerpT = 0;
            transform.localPosition = startPosition;
            enabled = true;
        }
    }
}
