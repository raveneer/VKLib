using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VKLib.Native;

namespace VKLib.VKLib.UI
{
    public class Button_Holdable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action FireEvent;
        public Button Button;
        public UnityEvent FireUnityEvent;
        public Image BG;
        private float contiueRemainSec;
        public TextMeshProUGUI ButtonText;
        private EventManager _eventManager;
        
        /// <summary>
        ///     홀드 상태가 되면, 이 시간마다 이벤트를 쏜다.
        /// </summary>
        private readonly float contiueTermSec = 0.05f;

        [SerializeField]
        private Image fillImage;
        private bool pointerDown;
        private float pointerDownTimer;

        [SerializeField]
        private float requiredHoldTime = 1f;

        private void Start()
        {
            Reset();
            FireEvent += () => FireUnityEvent?.Invoke();

            //이벤트 매니저를 주입받지 못하기에, 우회함. 서플라이어가 반드시 씬에 있어야 한다.
            //그래서 start에서 참조를 따는 것. 시점도 중요.
            var eventManagerInjectSupplier = FindObjectOfType<EventManagerInjectSupplier>();
            TDebug.AssertNotNull(eventManagerInjectSupplier, nameof(eventManagerInjectSupplier));
            _eventManager = eventManagerInjectSupplier.EventManagerRef;
            TDebug.AssertNotNull(_eventManager, nameof(_eventManager));
        }

        /// <summary>
        ///     일정시간 이상 누르고 있으면 일정 텀으로 연사함.
        /// </summary>
        private void Update()
        {
            if (pointerDown)
            {
                //누르는 중임을 알림. 누르는 중에는 스크롤을 막아야 하므로. 
                _eventManager.Notify_HoldableButtonDown();

                fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
                pointerDownTimer += Time.deltaTime;
                if (pointerDownTimer >= requiredHoldTime)
                {
                    contiueRemainSec += Time.deltaTime;
                    if (contiueRemainSec >= contiueTermSec)
                    {
                        contiueRemainSec -= contiueTermSec;
                        FireEvent?.Invoke();
                        //TDebug.Log("LongLongClicks fire!");
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            //Debug.Log("OnPointerDown");
        }

        /// <summary>
        ///     뗄떼도 한번은 발동 해 주어야 함
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            FireEvent?.Invoke();

            Reset();
            //Debug.Log("OnPointerUp");
        }

        private void Reset()
        {
            pointerDown = false;
            pointerDownTimer = 0;
            fillImage.fillAmount = 0;
        }

        public void SetPushable(bool isPushAble)
        {
            if (isPushAble)
            {
                BG.color = Color.white;
            }
            else
            {
                BG.color = Color.gray;
            }
        }

    }
}