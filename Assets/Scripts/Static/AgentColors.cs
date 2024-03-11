using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Static
{
    public static class Colors
    {
        private static Dictionary<int, Color> _idToColor = new Dictionary<int, Color>()
        {
            {0, Color.red},
            {1, Color.blue},
            {2, Color.green},
            {3, Color.yellow},
            {4, Color.cyan},
            {5, Color.magenta},
            {6, Color.grey},
            {7, Color.white},
            {8, Color.black},
            {9, new Color(240,230,140)},
        };

        public static Color GetColor(int id) => _idToColor[id];
    }
}
