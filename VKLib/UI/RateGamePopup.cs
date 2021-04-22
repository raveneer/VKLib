using System;
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
        public Button Button_Rate1;
        public Button Button_Rate2;
        public Button Button_Rate3;
        public Button Button_Rate4;
        public Button Button_Rate5;
        public Button Button_Submit;
        public Sprite DisabledStar;
        public string GooglePlayBundleID;
        public GameObject Panel;
        public List<Image> Stars = new List<Image>();
        private Action _onSubmitAction;
        private int _rate;
        private readonly int _rateThreshold = 4;

        private void Awake()
        {
            _eventManager.OpenRatePopup += OnOpenRatePopup;
            Button_Rate1.onClick.AddListener(() => SetRate(1));
            Button_Rate2.onClick.AddListener(() => SetRate(2));
            Button_Rate3.onClick.AddListener(() => SetRate(3));
            Button_Rate4.onClick.AddListener(() => SetRate(4));
            Button_Rate5.onClick.AddListener(() => SetRate(5));
            Panel.gameObject.SetActive(false);
            Button_close.onClick.AddListener(() => Panel.gameObject.SetActive(false));
            Button_Submit.onClick.AddListener(() => TrySendRate(_rate));
        }

        private void OnOpenRatePopup(Action onSubmitRate)
        {
            _onSubmitAction = onSubmitRate;
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
            TDebug.Log($"rate {rate}");
            _rate = rate;
            for (var j = 0; j < Stars.Count; j++)
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

            PlayerPrefs.SetInt("RateGame", _rate);

            if (_rate >= _rateThreshold)
            {
                Application.OpenURL("https://play.google.com/store/apps/details?id=" + GooglePlayBundleID);
            }

            //´Ý´Â´Ù.
            Panel.gameObject.SetActive(false);
            _onSubmitAction.Invoke();
        }
    }
}