using TMPro;
using UnityEngine;

public class UI_VersionText : MonoBehaviour
{
    void Start()
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = $"ver:{Application.version}";
    }

    void Update()
    {
        
    }
}
