using System;
using UnityEngine;

namespace VKLib.Unity
{
    /// <summary>
    ///     폰의 뒤로가기 버튼으로 닫기.
    ///     현재 안드폰만 지원중
    /// </summary>
    public class GameQuitByPhoneBackButton : MonoBehaviour
    {
        private void Update()
        {
            // 안드로이드 백키가 눌렸을때
            if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }
        }

        private void Quit()
        {
            try
            {
                Application.Quit();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}