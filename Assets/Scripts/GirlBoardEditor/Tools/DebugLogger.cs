using UnityEngine;
using Object = System.Object;

namespace GirlBoardEditor.Tools
{
    public class DebugLogger
    {
        private static DebugLogger instance;
  
        public static DebugLogger Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new DebugLogger();
                }

                return instance;
            }
        }
        
        public DebugLogger(){}

        public void Log(Object sender, string log)
        {
            Debug.Log(sender.GetType().FullName + " || Send Message:" + log);
        }

        public void LogError(Object sender, string log)
        {
            Debug.Log(sender.GetType().FullName + " || Error Message:" + log);
        }
        
    }
}