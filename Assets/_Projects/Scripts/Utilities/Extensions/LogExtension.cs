using System;
using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class LogExtension
    {
        public static void Log(this Exception except)
        {
            Debug.LogErrorFormat("[Exception]{0}", except.Message);
        }
        public static string LogVector2(Vector2 vector2)
        {
            return string.Format(vector2.x + "  " + vector2.y);
        }
    }
}