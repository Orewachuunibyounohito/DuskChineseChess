using System.Collections.Generic;
using UnityEngine;

namespace ChuuniExtension.Loggers
{
    public class LoggerSystem
    {
        private static List<string> logs = new List<string>();

        public static void Add(string log){
            logs.Add(log);
        }
        
        public static void ShowLogForUnity(){
            string info = "";
            foreach(var log in logs){
                info += $"{log}\n";
            }

            if(string.IsNullOrEmpty(info)){
                Debug.Log("No log.");
            }else{
                info += "----";
                Debug.Log(info);
            }
        }
    }

}