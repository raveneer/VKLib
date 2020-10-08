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
        public UIText LogText;
        public GameObject Panel;
        private int _commandIndex;
        private readonly int _logTextMaxLine = 5;
        private readonly List<string> _logTexts = new List<string>();
        private readonly List<string> _lastCommand = new List<string>();

        private void Awake()
        {
            InputField.onEndEdit.AddListener(OnInputText);
            _eventManager.ToggleDebugConsole += TogglePanel;
        }

        private void Start()
        {
            Close();
        }

        private void Close()
        {
            GameStaticState.IsConsoleMode = true;
            TogglePanel();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote)) //hack : 백큐토 키가 인풋매니저에서 지정되지가 않음.. 유니티 자체 버그인듯. 그래서 우회함.
            {
                GameStaticState.IsConsoleMode = !GameStaticState.IsConsoleMode;
                TogglePanel();
                return;
            }

            //esc로 닫을 수 있다.
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GameStaticState.IsConsoleMode = false;
                TogglePanel();
                return;
            }

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

            LogText.Text = result;
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

        private void TogglePanel()
        {
            GameStaticState.IsConsoleMode = !GameStaticState.IsConsoleMode;
            Panel.gameObject.SetActive(GameStaticState.IsConsoleMode);
            
            if (GameStaticState.IsConsoleMode)
            {
                InputField.Select();
                InputField.ActivateInputField();
            }
        }

    }
}