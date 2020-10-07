using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using UnityEngine;

/// <summary>
///     비교를 할 때 == 를 사용하지 말 것. 반드시 .Equals 를사용할 것. => 구조체로 바뀌었는데도 그러한가?
/// </summary>
[TypeConverter(typeof(CoordConverter))]
public struct Coord : IEquatable<Coord>
{
    public static readonly Coord zero = new Coord(0, 0);
    public static readonly Coord one = new Coord(1, 1);
    public int X { get; private set; }
    public int Y { get; private set; }
    public Coord Up => new Coord(X, Y + 1);
    public Coord Down => new Coord(X, Y - 1);
    public Coord Left => new Coord(X - 1, Y);
    public Coord Right => new Coord(X + 1, Y);

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Coord XOnlyCoord(int x)
    {
        return new Coord(x, 0);
    }

    /// <summary>
    ///     진짜 거리를 받아냄
    /// </summary>
    public static double Distance(Coord _a, Coord _b)
    {
        return Math.Sqrt(Math.Pow(_a.X - _b.X, 2) + Math.Pow(_a.Y - _b.Y, 2));
    }

    public bool Equals(Coord other)
    {
        return X == other.X && Y == other.Y;
    }

    public static Coord FromVector2Int(Vector2Int position)
    {
        return new Coord(position.x, position.y);
    }

    public static Coord FromVector3(Vector3 position)
    {
        return new Coord((int) position.x, (int) position.y);
    }

    //Coord의 한 축이 최대 1만 이하라고 가정하고 해싱.
    public override int GetHashCode()
    {
        return X * 10000 + Y;
    }

    public static Coord Lerp(Coord a, Coord b, float percent)
    {
        if (percent < 0)
        {
            percent = 0;
        }

        if (percent > 1)
        {
            percent = 1;
        }

        var x = (int) Math.Round(a.X * (1 - percent) + b.X * percent);
        var y = (int) Math.Round(a.Y * (1 - percent) + b.Y * percent);
        return new Coord(x, y);
    }

    public static Coord operator +(Coord a, Coord b)
    {
        // Add index values
        return new Coord(a.X + b.X, a.Y + b.Y);
    }

    public static Coord operator *(Coord a, int b)
    {
        // Subtract index values
        return new Coord(a.X * b, a.Y * b);
    }

    public static Coord operator -(Coord a, Coord b)
    {
        // Subtract index values
        return new Coord(a.X - b.X, a.Y - b.Y);
    }

    /// <summary>
    ///     퍼포먼스를 올리기 위해 coord를 재활용 할 때의 처리.
    ///     그 외에는 쓰지 말 것.
    /// </summary>
    public void SetValueByForceForPerformance(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    ///     빠른 거리 비교를 위해 제곱근 하지 않은 값을 반환한다. 3,4,5 의 삼각형이라면, 5*5를 반환함. 5를 반환하는게 아니라.
    ///     Distance 보다 20%쯤 빠름. 큰 차이는 없음...
    /// </summary>
    public static double FastDistance(Coord a, Coord b)
    {
        return Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
    }

    public override string ToString()
    {
        return $"Coord : {X},{Y}";
    }

    public static IEnumerable<Coord> FromTuples(IEnumerable<(int, int)> input)
    {
        return input.Select(x => new Coord(x.Item1, x.Item2));
    }

    public IEnumerable<Coord> NeighborCoords4Way()
    {
        yield return Up;
        yield return Down;
        yield return Left;
        yield return Right;
    }

    public bool IsBeside(Coord other)
    {
        return Up.Equals(other) || Down.Equals(other) || Left.Equals(other) || Right.Equals(other);
    }

    /// <summary>
    ///     주어진 coord와의 방향을 구한다.
    /// </summary>
    public EDirection GetDirectionTo(Coord to)
    {
        //오른쪽
        if (to.X > X)
        {
            if (to.Y > Y)
            {
                return EDirection.NE;
            }
            if (to.Y == Y)
            {
                return EDirection.E;
            }
            if (to.Y < Y)
            {
                return EDirection.SE;
            }
        }

        //같은 x
        if (to.X == X)
        {
            if (to.Y > Y)
            {
                return EDirection.N;
            }
            if (to.Y == Y)
            {
                return EDirection.Center;
            }
            if (to.Y < Y)
            {
                return EDirection.S;
            }
        }

        //왼쪽
        if (to.X < X)
        {
            if (to.Y > Y)
            {
                return EDirection.NW;
            }
            if (to.Y == Y)
            {
                return EDirection.W;
            }
            if (to.Y < Y)
            {
                return EDirection.SW;
            }
        }

        throw new Exception();
    }

    [Serializable]
    //3*3 배열을 기준으로 할때, 다음과 같은 배열 모양이 된다. (좌하가 0,0)
    //NW, N, NE
    //W, Center, E
    //SW, S, SE
    public enum EDirection
    {
        Center
      , N
      , NE
      , E
      , SE
      , S
      , SW
      , W
      , NW
    }

    public class CoordConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value as string;
            if (text != null)
            {
                return Parse(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destination Type is null");
            }

            var Coord = (Coord) value;
            if (CanConvertTo(context, destinationType))
            {
                return Coord.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    private static Coord Parse(string toStringText)
    {
        var splited = toStringText.Replace("Coord : ", "").Split(',');
        return new Coord(int.Parse(splited[0]), int.Parse(splited[1]));
    }
}