using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BFramework
{
    public class Resname
    {
        private static string UiPath = "Assets/Art/Perfab/UI/";
        private static string PngPath = "Assets/Art/Texture/";
        private static string MonsterPath = "Assets/Art/Perfab/";
        public static string UI(string name)
        {
            return UiPath + name + ".prefab";
        }
        public static string CardShow()
        {
            return MonsterPath +  "CardShow.prefab";
        }
        public static string Monster()
        {
            return MonsterPath +  "monster.prefab";
        }
        public static string Card()
        {
            return MonsterPath +  "card.prefab";
        }
        public static string Role()
        {
            return MonsterPath +  "role.prefab";
        }
      
        public static string Texture(string name)
        {
            return PngPath + name + ".png";
        }
        public static string RoleTexture(string name)
        {
            return PngPath+"role/" + name + ".png";
        }
    }
}

