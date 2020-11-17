using System.Collections.Generic;
using System.Linq;
using ShopWars.UnityDependency;
using TMPro;
using UnityEngine;
using VKLib.Native;
using Zenject;

namespace VKLib.UI
{
    public class UI_ConsoleCommand : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;
        public TMP_InputField InputField;
        public TextMeshProUGUI LogText;
        public GameObject Panel;
        private int _commandIndex;
        private readonly List<string> _lastCommand = new List<string>();
        private readonly int _logTextMaxLine = 5;
        private readonly List<string> _logTexts = new List<string>();

        private void Awake()
        {
            InputField.onEndEdit.AddListener(OnInputText);
            _eventManager.ToggleDebugConsole += TogglePanel;
        }

        private void TogglePanel()
        {
            if (GameStaticState.IsConsoleMode)
            {
                Close();
            }
            else
            {
                Opne();
            }
        }

        private void Start()
        {
            Close();
        }

        private void Update()
        {
        #if UNITY_EDITOR
            //esc로 열고닫을 수 있다.
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                TogglePanel();
                return;
            }
            
        #else
            //hack : 백큐토 키가 인풋매니저에서 지정되지가 않음.. 유니티 자체 버그인듯. 그래서 우회함.
            if (Input.GetKeyUp(KeyCode.BackQuote)) 
            {
                TogglePanel();
                return;
            }
        #endif

            //방향키 아래위를 이용해서 사용한 콘솔명령어를 쉽게 재사용 할 수 있다.
            if (GameStaticState.IsConsoleMode)
            {
                if (Input.GetButtonUp("Up"))
                {
                    SetConsoleTextToBefore();
                }

                if (Input.GetButtonUp("Down"))
                {
                    SetConsoleTextToNext();
                }
            }
        }

        private void OnInputText(string consoleText)
        {
            if (!_lastCommand.Any() || _lastCommand.Last() != consoleText)
            {
                _lastCommand.Add(consoleText);
            }

            _commandIndex = _lastCommand.Count - 1;
            var executeResult = _eventManager.Raise_ConsoleTextInput(consoleText);
            AddLogTextAndPrint(executeResult);
            _eventManager.Raise_PlaySoundUI(SoundNames.Positive);
        }

        private void AddLogTextAndPrint(string newLog)
        {
            if (_logTexts.Count <= _logTextMaxLine)
            {
                _logTexts.Add(newLog);
            }
            else
            {
                _logTexts.RemoveAt(0);
                _logTexts.Add(newLog);
            }

            var result = "";
            foreach (var text in _logTexts) result += "\r\n" + text;

            LogText.text = result;
        }

        private void Close()
        {
            GameStaticState.IsConsoleMode = false;
            Panel.gameObject.SetActive(false);
        }

        private void SetConsoleTextToBefore()
        {
            _commandIndex = Mathf.Max(0, _commandIndex - 1);
            InputField.text = _lastCommand[_commandIndex];
        }

        private void SetConsoleTextToNext()
        {
            _commandIndex = Mathf.Min(_lastCommand.Count - 1, _commandIndex + 1);
            InputField.text = _lastCommand[_commandIndex];
        }

        private void Opne()
        {
            GameStaticState.IsConsoleMode = true;
            Panel.gameObject.SetActive(true);
            InputField.Select();
            InputField.ActivateInputField();
        }
    }
}