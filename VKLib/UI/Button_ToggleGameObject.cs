using UnityEngine;
using UnityEngine.UI;
using VKLib.Native;

namespace VKLib.UI
{
    public class Button_ToggleGameObject : MonoBehaviour
    {
        public GameObject TargetGo;

        // Start is called before the first frame update
        private void Start()
        {
            TDebug.AssertNotNull(TargetGo, nameof(TargetGo));
            GetComponent<Button>().onClick.AddListener(() => TargetGo.SetActive(!TargetGo.activeSelf));
        }

    }
}