using System;
using UnityEngine;

/// <summary>
///     independent from unity
/// </summary>
public struct TVector2 : IEquatable<object>
{
    public float X { get; set; }
    public float Y { get; set; }

    public TVector2(float xVal, float yVal)
    {
        X = xVal;
        Y = yVal;
    }

    public TVector2(Vector3 vector3)
    {
        X = vector3.x;
        Y = vector3.y;
    }

    public static Vector2 ToVector2(TVector2 tVector2)
    {
        return new Vector2(tVector2.X, tVector2.Y);
    }

    public static TVector2 FromVector2(Vector2 Vector2)
    {
        return new TVector2(Vector2.x, Vector2.y);
    }

    public static TVector2 operator +(TVector2 mv1, TVector2 mv2)
    {
        return new TVector2(mv1.X + mv2.X, mv1.Y + mv2.Y);
    }

    public static TVector2 operator -(TVector2 mv1, TVector2 mv2)
    {
        return new TVector2(mv1.X - mv2.X, mv1.Y - mv2.Y);
    }

    public static TVector2 operator -(TVector2 mv1, float var)
    {
        return new TVector2(mv1.X - var, mv1.Y - var);
    }

    public static TVector2 operator *(TVector2 mv1, TVector2 mv2)
    {
        return new TVector2(mv1.X * mv2.X, mv1.Y * mv2.Y);
    }

    public static TVector2 operator *(TVector2 mv, float var)
    {
        return new TVector2(mv.X * var, mv.Y * var);
    }

    public static TVector2 operator %(TVector2 mv1, TVector2 mv2)
    {
        throw new NotImplementedException();
        /* return new TVector2(mv1.Y * mv2.Z - mv1.Z * mv2.Y,
             mv1.Z * mv2.X - mv1.X * mv2.Z,
             mv1.X * mv2.Y - mv1.Y * mv2.X);*/
    }

    //정수형이니까 안돼...
    /*public static TVector2 Lerp(this TVector2 firstVector, TVector2 secondVector, float by)
    {
        var retX = Lerp(firstVector.X, secondVector.X, by);
        var retY = Lerp(firstVector.Y, secondVector.Y, by);
        return new TVector2(retX, retY);
    }*/

    public float this[int key]
    {
        get => GetValueByIndex(key);
        set => SetValueByIndex(key, value);
    }

    private void SetValueByIndex(int key, float value)
    {
        if (key == 0) X = value;
        else if (key == 1) Y = value;
    }

    private float GetValueByIndex(int key)
    {
        if (key == 0) return X;
        return Y;
    }

    public float DotProduct(TVector2 mv)
    {
        return X * mv.X + Y * mv.Y;
    }

    public TVector2 ScaleBy(float value)
    {
        return new TVector2(X * value, Y * value);
    }

    public TVector2 ComponentProduct(TVector2 mv)
    {
        return new TVector2(X * mv.X, Y * mv.Y);
    }

    public void ComponentProductUpdate(TVector2 mv)
    {
        X *= mv.X;
        Y *= mv.Y;
    }

    public TVector2 VectorProduct(TVector2 mv)
    {
        throw new NotImplementedException();
        /*return new TVector2(Y * mv.Z - Z * mv.Y,
            Z * mv.X - X * mv.Z,
            X * mv.Y - Y * mv.X);*/
    }

    public float ScalarProduct(TVector2 mv)
    {
        return X * mv.X + Y * mv.Y;
    }

    public void AddScaledVector(TVector2 mv, float scale)
    {
        X += mv.X * scale;
        Y += mv.Y * scale;
    }

    public float Magnitude()
    {
        return (float) Math.Sqrt(X * X + Y * Y);
    }

    public float SquareMagnitude()
    {
        return X * X + Y * Y;
    }

    public void Trim(float size)
    {
        if (SquareMagnitude() > size * size)
        {
            Normalize();
            X *= size;
            Y *= size;
        }
    }

    public void Normalize()
    {
        var m = Magnitude();
        if (m > 0)
        {
            X = X / m;
            Y = Y / m;
        }
        else
        {
            X = 0;
            Y = 0;
        }
    }

    public TVector2 Inverted()
    {
        return new TVector2(-X, -Y);
    }

    public TVector2 Unit()
    {
        var result = this;
        result.Normalize();
        return result;
    }

    public void Clear()
    {
        X = 0;
        Y = 0;
    }

    public static float Distance(TVector2 mv1, TVector2 mv2)
    {
        return (mv1 - mv2).Magnitude();
    }

    public static TVector2 Zero()
    {
        return new TVector2(0f, 0f);
    }

    /// <summary>
    ///     Override hash code method.
    /// </summary>
    public override int GetHashCode()
    {
        return X.GetHashCode() + Y.GetHashCode();
    }

    /// <summary>
    ///     Override equals method.
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is TVector2 == false)
            return false;

        if (obj is TVector2)
        {
            var Vector2 = (TVector2) obj;

            if (X == Vector2.X
                && Y == Vector2.Y
            )
                return true;
        }

        return false;
    }

    //equality
}

public struct TVector2Bound
{
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;

    public TVector2Bound(TVector2 start, TVector2 end)
    {
        MaxX = start.X > end.X ? start.X : end.X;
        MinX = start.X > end.X ? end.X : start.X;
        MaxY = start.Y > end.Y ? start.Y : end.Y;
        MinY = start.Y > end.Y ? end.Y : start.Y;
    }

    public bool IsInBound(TVector2 position)
    {
        return position.X >= MinX && position.X <= MaxX && position.Y >= MinY && position.Y <= MaxY;
    }

    public bool IsInBound(TVector3 position)
    {
        return IsInBound(new TVector2(position.X, position.Y));
    }

    public bool IsZeroSize()
    {
        return MaxX == MinX && MaxY == MinY;
    }
}