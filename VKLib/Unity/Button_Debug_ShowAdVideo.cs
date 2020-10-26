using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VKLib;
using VKLib.Native;
using Zenject;

public class Button_Debug_ShowAdVideo : MonoBehaviour
{
    [Inject] private EventManager _eventManager;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => _eventManager.Raise_ShowAd(null, null, null));
    }
}
