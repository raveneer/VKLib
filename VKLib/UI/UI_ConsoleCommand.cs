using System.Collections.Generic;
using System.Linq;
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

        private void Start()
        {
            Close();
        }

        private void Update()
        {
            //단축키로 열고 닫기는 이제 에디터에서만 지원된다.
            if (Application.isEditor)
            {
                //esc로 열고닫을 수 있다.
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TogglePanel();
                    return;
                }

                if (Input.GetKeyUp(KeyCode.BackQuote) && Application.isEditor)
                {
                    TogglePanel();
                    return;
                }
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

        private void Opne()
        {
            GameStaticState.IsConsoleMode = true;
            Panel.gameObject.SetActive(true);
            InputField.Select();
            InputField.ActivateInputField();
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
            if (GameStaticState.IsConsoleMode)
            {
                Close();
            }
            else
            {
                Opne();
            }
        }
    }
}