using UnityEngine;

namespace Assets.Scripts.PDC.Extensions
{
    public static class VectorExtensitons
    {
        public static Vector3Int FromVector3(this Vector3Int vector3int, Vector3 vector3)
        {
            return new Vector3Int((int) vector3.x, (int) vector3.y, (int) vector3.z);
        }

        public static Vector3 Avg(this Vector3 v1, Vector3 v2)
        {
            return (v1+v2)/2f;
        }
    }
}