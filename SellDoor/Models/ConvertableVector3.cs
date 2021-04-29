using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Models
{
    public class ConvertableVector3
    {
        public ConvertableVector3() { }

        public ConvertableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public static ConvertableVector3 FromVector3(Vector3 vector)
        {
            return new ConvertableVector3(vector.x, vector.y, vector.z);
        }

        public bool Equals(Vector3 vector)
        {
            if (vector.x == X && vector.y == Y && vector.z == Z)
                return true;
            return false;
        }
    }
}
