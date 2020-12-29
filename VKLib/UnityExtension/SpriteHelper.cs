using UnityEngine;

namespace Assets.Scripts.PDC.Extensions
{
    public static class SpriteHelper
    {
        /// <summary>
        /// https://answers.unity.com/questions/641006/is-there-a-way-to-get-a-sprites-current-widthheigh.html
        /// 스프라이트의 엣지의 월드 좌표를 받아냄.
        /// </summary>
        public static Vector3[] SpriteLocalToWorld(Transform transform, Sprite sp) 
        {
            Vector3 pos = transform.position;
            Vector3 [] array = new Vector3[2];
            //top left
            array[0] = pos + sp.bounds.min;
            // Bottom right
            array[1] = pos + sp.bounds.max;
            return array;
        }
    }
}