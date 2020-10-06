using UnityEngine;
using Zenject;

namespace VKLib
{
    public class GoFactory<T> : Factory<T> where T : MonoBehaviour
    {
        public void Destroy(T go)
        {
            Object.Destroy(go.gameObject);
            //TDebug.Log($"go destroy. :{go.gameObject.name}");
        }
    }

    public class BaseGoFactory<T> : Factory<T> where T : BaseGo
    {
        public void Destroy(T go)
        {
            go.ReadyToDestroy();
            //삭제예약 (이번 프레임 끝에서 발동함!)
            Object.Destroy(go.gameObject);
            //TDebug.Log($"go destroy. :{go.gameObject.name}");
        }
    }
}