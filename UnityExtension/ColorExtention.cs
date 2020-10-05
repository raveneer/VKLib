using System.Collections.Generic;
using UnityEngine;

namespace PDC.Extensions.UnityDefendency
{
    public static class ColorExtention
    {
        /// <summary>
        /// todo : HSL을 이용해서 제대로 뒤집을 것.
        /// https://stackoverflow.com/questions/2942/how-to-use-hsl-in-asp-net/2504318#2504318 등을 고려할 것.
        /// </summary>
        /// <param name="MyColor"></param>
        /// <returns></returns>
        public static Color Invert(this Color MyColor)
        {
           return  MyColor;
        }

    }
}