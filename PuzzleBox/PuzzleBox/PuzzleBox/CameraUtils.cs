using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{
    class CameraUtils
    {
        public static float GetDistance(Vector3 v, Vector3 c, Vector3 p)
        {
            Vector3 w = p - c;
            return Vector3.Dot(v, w) / v.Length();
        }

        public static int GetScreenX(Vector3 v, Vector3 c, Vector3 u, Vector3 p)
        {
            Vector3 w = p - c;
            Vector3 x = Vector3.Cross(u, v);
            return (int)(Vector3.Dot(x, w) / x.Length());
        }

        public static int GetScreenY(Vector3 v, Vector3 c, Vector3 u, Vector3 p)
        {
            Vector3 w = p - c;
            return (int)(Vector3.Dot(u, w) / u.Length());
        }
    }
}
