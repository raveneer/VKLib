using UnityEngine;

namespace VKLib.Unity
{
    /// <summary>
    /// https://www.youtube.com/watch?v=P5JxTfCAOXo
    /// 폰의 자이로에 따라 회전한다. 왜 컨테이너에 넣어주는지는 아직 잘 모르겠음...
    /// </summary>
    public class GyroControll : MonoBehaviour
    {
        private bool _gyroEnabled;
        private Gyroscope _gyro;
        private GameObject _container;
        private Quaternion _rot;

        private void Start()
        {
            _container = new GameObject("Gyro Container");
            _container.transform.position = transform.position;
            transform.SetParent(_container.transform);
            _gyroEnabled = EnableGyro();

            Debug.Log($"system gyro support : {SystemInfo.supportsGyroscope}");
        }

        private bool EnableGyro()
        {
            if (SystemInfo.supportsGyroscope)
            {
                _gyro = Input.gyro;
                _gyro.enabled = true;

                _container.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
                _rot = new Quaternion(0, 0, 1, 0);
                return true;
            }
            return false;
        }

        private void Update()
        {
            if (_gyroEnabled)
            {
                transform.localRotation = _gyro.attitude * _rot;
            }
        }
    }
}