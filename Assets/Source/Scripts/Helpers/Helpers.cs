using System;
using System.Collections.Generic;
using UnityEngine;

namespace NSpace
{
    public static class Helpers
    {

    }

    public static class Extensions
    {
        public static Vector3 Scaled(this Vector3 v, Vector3 scale)
        {
            v.Scale(scale);
            return v;
        }

        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
        }
    }


}
