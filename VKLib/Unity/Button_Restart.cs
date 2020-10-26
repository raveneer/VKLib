﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VKLib;
using VKLib.Native;
using Zenject;

public class Button_Restart : MonoBehaviour
{
    [Inject] private EventManager _eventManager;
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(()=> _eventManager.Raise_SceneLoadAfterAdShow());
    }
}