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
            //�������� (�̹� ������ ������ �ߵ���!)
            Object.Destroy(go.gameObject);
            //TDebug.Log($"go destroy. :{go.gameObject.name}");
        }
    }
}