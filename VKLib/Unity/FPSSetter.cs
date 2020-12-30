using UnityEngine;

public class FPSSetter : MonoBehaviour {
    public int targetFPS;
    private void Awake()
    {
        Application.targetFrameRate = targetFPS; //vsync 꺼졌을 때만 작동.

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
