using UnityEngine;
using UnityEngine.UI;

namespace VKLib.Unity
{
    public class Button_OpenURL : MonoBehaviour
    {
        public string URL;
        public Button Button;
        void Awake()
        {
            Button.onClick.AddListener(()=>Application.OpenURL(URL));
        }
    }    
}

