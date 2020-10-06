using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VKLib
{
    public class Helper : MonoBehaviour
    {
        //두개의 게임 오브젝트의 중앙좌표를 반환한다.
        public static Vector3 FindAVGPosition(GameObject _g1, GameObject _g2)
        {
            Vector3 _avgPosition = new Vector3(
                                    (_g1.gameObject.transform.position.x + _g2.gameObject.transform.position.x) / 2,
                                    (_g1.gameObject.transform.position.y + _g2.gameObject.transform.position.y) / 2,
                                    (_g1.gameObject.transform.position.z + _g2.gameObject.transform.position.z) / 2
                                    );
            return _avgPosition;
        }
    
    }
}
