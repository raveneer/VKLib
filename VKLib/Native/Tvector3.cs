﻿using System;
using UnityEngine;

namespace VKLib.Native
{
    /// <summary>
    ///     independent from unity
    /// </summary>
    public struct TVector3 : IEquatable<object>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public TVector3(float xVal, float yVal, float zVal)
        {
            X = xVal;
            Y = yVal;
            Z = zVal;
        }

        public TVector3(Vector3 vector3)
        {
            X = vector3.x;
            Y = vector3.y;
            Z = vector3.z;
        }

        public static Vector3 ToVector3(TVector3 tVector3)
        {
            return new Vector3(tVector3.X, tVector3.Y, tVector3.Z);
        }

        public static TVector3 operator +(TVector3 mv1, TVector3 mv2)
        {
            return new TVector3(mv1.X + mv2.X, mv1.Y + mv2.Y, mv1.Z + mv2.Z);
        }

        public static TVector3 operator -(TVector3 mv1, TVector3 mv2)
        {
            return new TVector3(mv1.X - mv2.X, mv1.Y - mv2.Y, mv1.Z - mv2.Z);
        }

        public static TVector3 operator -(TVector3 mv1, float var)
        {
            return new TVector3(mv1.X - var, mv1.Y - var, mv1.Z - var);
        }

        public static TVector3 operator *(TVector3 mv1, TVector3 mv2)
        {
            return new TVector3(mv1.X * mv2.X, mv1.Y * mv2.Y, mv1.Z * mv2.Z);
        }

        public static TVector3 operator *(TVector3 mv, float var)
        {
            return new TVector3(mv.X * var, mv.Y * var, mv.Z * var);
        }

        public static TVector3 operator %(TVector3 mv1, TVector3 mv2)
        {
            return new TVector3(mv1.Y * mv2.Z - mv1.Z * mv2.Y,
                                mv1.Z * mv2.X - mv1.X * mv2.Z,
                                mv1.X * mv2.Y - mv1.Y * mv2.X);
        }

        public float this[int key]
        {
            get => GetValueByIndex(key);
            set => SetValueByIndex(key, value);
        }

        private void SetValueByIndex(int key, float value)
        {
            if (key == 0) X = value;
            else if (key == 1) Y = value;
            else if (key == 2) Z = value;
        }

        private float GetValueByIndex(int key)
        {
            if (key == 0) return X;
            if (key == 1) return Y;
            return Z;
        }

        public float DotProduct(TVector3 mv)
        {
            return X * mv.X + Y * mv.Y + Z * mv.Z;
        }

        public TVector3 ScaleBy(float value)
        {
            return new TVector3(X * value, Y * value, Z * value);
        }

        public TVector3 ComponentProduct(TVector3 mv)
        {
            return new TVector3(X * mv.X, Y * mv.Y, Z * mv.Z);
        }

        public void ComponentProductUpdate(TVector3 mv)
        {
            X *= mv.X;
            Y *= mv.Y;
            Z *= mv.Z;
        }

        public TVector3 VectorProduct(TVector3 mv)
        {
            return new TVector3(Y * mv.Z - Z * mv.Y,
                                Z * mv.X - X * mv.Z,
                                X * mv.Y - Y * mv.X);
        }

        public float ScalarProduct(TVector3 mv)
        {
            return X * mv.X + Y * mv.Y + Z * mv.Z;
        }

        public void AddScaledVector(TVector3 mv, float scale)
        {
            X += mv.X * scale;
            Y += mv.Y * scale;
            Z += mv.Z * scale;
        }

        public float Magnitude()
        {
            return (float) Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float SquareMagnitude()
        {
            return X * X + Y * Y + Z * Z;
        }

        public void Trim(float size)
        {
            if (SquareMagnitude() > size * size)
            {
                Normalize();
                X *= size;
                Y *= size;
                Z *= size;
            }
        }

        public void Normalize()
        {
            var m = Magnitude();
            if (m > 0)
            {
                X = X / m;
                Y = Y / m;
                Z = Z / m;
            }
            else
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
        }

        public TVector3 Inverted()
        {
            return new TVector3(-X, -Y, -Z);
        }

        public TVector3 Unit()
        {
            var result = this;
            result.Normalize();
            return result;
        }

        public void Clear()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public static float Distance(TVector3 mv1, TVector3 mv2)
        {
            return (mv1 - mv2).Magnitude();
        }

        public static TVector3 Zero()
        {
            return new TVector3(0f, 0f, 0f);
        }

        /// <summary>
        ///     Override hash code method.
        /// </summary>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        ///     Override equals method.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is TVector3 == false)
                return false;

            if (obj is TVector3)
            {
                var vector3 = (TVector3) obj;

                if (X == vector3.X
                    && Y == vector3.Y
                    && Z == vector3.Z
                )
                    return true;
            }

            return false;
        }

        //equality
    }
}