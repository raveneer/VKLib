using System;
using System.Collections.Generic;
using UnityEngine;

namespace VKLib.Native
{
    /// <summary>
    ///     independent from unity
    /// </summary>
    [Serializable]
    public struct TColor : IEquatable<object>
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public static TColor Black => FromUnityColor(Color.black);
        public static TColor White => FromUnityColor(Color.white);
        public static TColor Red => FromUnityColor(Color.red);
        public static TColor Green => FromUnityColor(Color.green);
        public static TColor Blue => FromUnityColor(Color.blue);
        public static TColor Yellow => FromUnityColor(Color.yellow);
        public static TColor Cyan => FromUnityColor(Color.cyan);
        public static TColor Magenta => FromUnityColor(Color.magenta);
        public static TColor Grey => FromUnityColor(Color.grey);
        public static TColor Pink  => TColor.FromRBGColor(255,192,203, 255);
        public static TColor Transparent => new TColor(255, 255, 255, 0);

        //적용이 제대로 안되나...?
        public static TColor RED8 => new TColor(224, 49, 49, 255);
        public static TColor RED5 => new TColor(255, 107, 107, 255);
        public static TColor INDIGO8 => new TColor(59, 91, 219,255);
        public static TColor INDIGO5 => new TColor(92, 124, 250,255);
        public static TColor GREEN8 => new TColor(47, 158, 68, 255);
        public static TColor GREEN5 => new TColor(81, 207, 102, 255);
        public static TColor YELLOW8 => new TColor(240, 140, 0, 255);
        public static TColor YELLOW6 => new TColor(250, 176, 5, 255);
        

        private static bool _randomColorsMade;
        private static readonly Dictionary<int, TColor> _randomColorDic = new Dictionary<int, TColor>();

        public TColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static  TColor FromRBGColor(int r, int g, int b, int a)
        {
            return new TColor(r/255f, g/255f, b/255f, a/255f);
        }

        public Color ToUnityColor()
        {
            return new Color(R, G, B, A);
        }

        public static TColor FromUnityColor(Color color)
        {
            return new TColor(color.r, color.g, color.b, color.a);
        }

        public static TColor RandomColor(int colorNumber)
        {
            if (colorNumber > 255)
                return Black;

            if (!_randomColorsMade)
            {
                MakeRandomColorsWithNumber();
            }

            return _randomColorDic[colorNumber];
        }

        /// <summary>
        ///     todo : 간단한 접근법, 그러나  HSV 러프를 고려해볼 것.
        ///     https://www.alanzucconi.com/2016/01/06/colour-interpolation/
        /// </summary>
        public static TColor Lerp(TColor a, TColor b, float t)
        {
            return new TColor
                (
                 a.R + (b.R - a.R) * t,
                 a.G + (b.G - a.G) * t,
                 a.B + (b.B - a.B) * t,
                 a.A + (b.A - a.A) * t
                );
        }

        private static void MakeRandomColorsWithNumber()
        {
            for (var i = 0; i < 255; i++)
            {
                _randomColorDic.Add(i, RandomColor());
            }
            _randomColorsMade = true;
        }

        private static TColor RandomColor()
        {
            return new TColor(TRandom.Range(0, 1f), TRandom.Range(0, 1f), TRandom.Range(0, 1f), 1);
        }

        /// <summary>
        ///     Override hash code method.
        /// </summary>
        public override int GetHashCode()
        {
            return R.GetHashCode() + G.GetHashCode() + B.GetHashCode() + A.GetHashCode();
        }

        /// <summary>
        ///     Override equals method.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is TColor == false)
                return false;

            if (obj is TColor)
            {
                var tColor = (TColor) obj;

                if (R == tColor.R
                    && G == tColor.G
                    && B == tColor.B
                    && A == tColor.A
                )
                    return true;
            }

            return false;
        }

        public TColor Multiply(float multiplier)
        {
            return new TColor(R * multiplier, G * multiplier, B * multiplier, A * multiplier);
        }

        public static bool operator ==(TColor t1, TColor t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(TColor t1, TColor t2)
        {
            return !t1.Equals(t2);
        }

        public static TColor operator +(TColor t1, TColor t2)
        {
            return new TColor(t1.R + t2.R, t1.G + t2.G, t1.B + t2.B, t1.A + t2.A);
        }

        public static TColor operator -(TColor t1, TColor t2)
        {
            return new TColor(t1.R - t2.R, t1.G - t2.G, t1.B - t2.B, t1.A - t2.A);
        }

        public static TColor GetColorByNumber(int number)
        {
            switch (number)
            {
                case -1:
                    return Black;
                case 0:
                    return Red;
                case 1:
                    return Blue;
                case 2:
                    return Green;
                case 3:
                    return Yellow;
                case 4:
                    return Cyan;
                case 5:
                    return Magenta;
                case 6:
                    return Grey;
                default:
                    return RandomColor(number);
            }
        }
    }
}