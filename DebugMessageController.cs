using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugTools.DebugMessageController; // use this line in your project, then you can do C(0) for colors

namespace DebugTools
{
    public static class DebugMessageController
    {
        public static List<Color> colors = new List<Color>(){
            new Color32(255, 179, 186, 255), // red - 0
            new Color32(255, 223, 186, 255), // orange - 1
            new Color32(255, 255, 186, 255), // yellow - 2
            new Color32(186, 255, 201, 255), // green - 3
            new Color32(186, 225, 255, 255), // blue - 4
            new Color32(197, 177, 225, 255)  // violet - 5
        };

        /// <summary>
        /// Returns the correct syntax for a color. Simply type this and your message right after the brackets.
        /// Example: Debug.Log($"{C(0)}This is a red message, but {C(4)}this part is blue.");
        /// 
        /// red - 0, orange - 1, yellow - 2, green - 3, blue - 4, violet - 5
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string C(int color)
        {
            string colorSyntax = "<color=#";
            colorSyntax += ColorUtility.ToHtmlStringRGB(colors[color]);
            colorSyntax += ">";

            return colorSyntax;
        }


    }
}