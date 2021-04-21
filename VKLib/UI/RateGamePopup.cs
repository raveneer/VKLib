using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKLib.Native;
using Zenject;

namespace VKLib.VKLib.UI
{
    public class RateGamePopup : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public Sprite ActiveStar;
        public Button Button_close;
        public Button Button_Submit;
        public List<Button> Buttons_Stars;
        public Sprite DisabledStar;
        public string GooglePlayBundleID;
        public GameObject Panel;
        public List<Image> Stars = new List<Image>();
        private int _rate;
        private readonly int _rateThreshold = 4;

        private void Awake()
        {
            _eventManager.OpenRatePopup += OnOpenRatePopup;
            for (int i = 0; i < Buttons_Stars.Count; i++)
            {
                var starButton = Buttons_Stars[i];
                starButton.onClick.AddListener(() => SetRate(i + 1));
            }
            Panel.gameObject.SetActive(false);
            Button_close.onClick.AddListener(() => Panel.gameObject.SetActive(false));
            Button_Submit.onClick.AddListener(() => TrySendRate(_rate));
        }

        private void OnOpenRatePopup()
        {
            if (AlreadyRate())
            {
                return;
            }

            Panel.gameObject.SetActive(true);
            Stars.ForEach(x => x.sprite = DisabledStar);
        }

        private bool AlreadyRate()
        {
            return PlayerPrefs.GetInt("RateGame") != 0;
        }

        private void SetRate(int rate)
        {
            _rate = rate;
            for (int j = 0; j < Stars.Count; j++)
            {
                var star = Stars[j];
                star.sprite = rate >= j + 1 ? ActiveStar : DisabledStar;
            }
        }

        private void TrySendRate(int rate)
        {
            if (rate == 0)
            {
                return;
            }

            if (_rate >= _rateThreshold)
            {
                PlayerPrefs.SetInt("RateGame", _rate);
                Application.OpenURL("https://play.google.com/store/apps/details?id=" + GooglePlayBundleID);
            }
        }
    }
}