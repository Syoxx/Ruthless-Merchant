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

        static Valve.VR.InteractionSystem.Player steamVrPlayer;

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

            if (Player.Singleton == null && steamVrPlayer == null)
                steamVrPlayer = FindObjectOfType<Valve.VR.InteractionSystem.Player>();
        }

        private void Update()
        {
            lerpT += Time.deltaTime / 3;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, lerpT);

            if (Player.Singleton != null)
                transform.LookAt(Player.Singleton.transform);

            else
            {
                GameObject vrCamera = GameObject.Find("VRCamera");

                if(vrCamera != null)
                    transform.LookAt(steamVrPlayer.transform);
                else
                    transform.LookAt(GameObject.Find("FallbackObjects").transform);
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - lerpT);
            irritationText.color = new Color(irritationText.color.r, irritationText.color.g, irritationText.color.b, 1 - lerpT);
            skepticismText.color = new Color(skepticismText.color.r, skepticismText.color.g, skepticismText.color.b, 1 - lerpT);

            if (lerpT > 1)
            {
                Destroy(gameObject);
            }
        }

        public void UpdateMood(Trader trader)
        {
            float bigger = 0;

            if (trader.IrritationTotal > trader.SkepticismTotal)
                bigger = trader.IrritationTotal;
            else
                bigger = trader.SkepticismTotal;

            if (bigger < 20)
                image.sprite = moodVeryHappy;
            else if (bigger < 45)
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
