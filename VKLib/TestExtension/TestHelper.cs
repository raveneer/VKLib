using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VKGames
{
    public class TestHelper : MonoBehaviour {

        public static readonly string PrfFolder = "Prf/";

        public static void PrepareSimple2DTestScene()
        {
            Debug.Log("================Start New Test============");
            Debug.Log("Delete All Gameobjects...");
            DeleteAllGameObjectExceptTestRunner();
            Debug.Log("...Spawn base objects...");
            InstantiateFromResource("MainCameraPrf");
            InstantiateFromResource("EventManagerPrf");
            Debug.Log("...Spawn EventSystem...");
            GameObject EventSystem = new GameObject("EventSystem");
            EventSystem.AddComponent<EventSystem>();
            Debug.Log("...Prepare done!");
        }

        public static void DeleteAllGameObjectExceptTestRunner()
        {
            GameObject[] AllGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject _g in AllGameObjects)
            {
                if (_g.name == "Code-based tests runner" || _g.name == "[DOTween]")
                {
                    //pass
                }
                else
                {
                    Debug.Log(_g.name + " / " + "Destroyed!");
                    GameObject.Destroy(_g);
                }
            }
        }

        // 프리팹 경로가 스태틱으로 추가되는 것에 주의
        public static GameObject InstantiateFromResource(string _prfName)
        {
            try
            {
                return Instantiate(Resources.Load<GameObject>(PrfFolder + _prfName));
            }
            catch
            {
                throw new System.Exception(_prfName +  " 는 존재하지 않습니다");
            }
        
        }

    }
}