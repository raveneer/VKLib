using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSShower : MonoBehaviour
{

    private int FramesPerSec;
    private float frequency = 1.0f;
    private string fps;
    public Text t_FPSShower;

    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (;;)
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;

            yield return new WaitForSeconds(frequency);

            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            t_FPSShower.text = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }
}