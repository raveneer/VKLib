using UnityEngine;
using UnityEngine.UI;
using VKLib.Native;
using Zenject;

public class Button_ToggleConsole : MonoBehaviour
{
    [Inject] private EventManager _eventManager;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => _eventManager.Raise_ToggleDebugConsole());
    }

}
