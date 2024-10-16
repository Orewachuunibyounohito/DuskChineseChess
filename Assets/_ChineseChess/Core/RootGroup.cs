using UnityEngine;

namespace ChineseChess
{
    public static class RootGroup
    {
        public static Transform Enviroment;
        public static Transform Core;
        public static Transform Object;

        static RootGroup(){
            Enviroment = GameObject.Find("--Enviroment--").transform;
            Core = GameObject.Find("-----Core-----").transform;
            Object = GameObject.Find("----Object----").transform;
        }
    }
}