using UnityEngine.Events;

namespace GirlBoardEditor.Tools
{
    public interface ILifecycle
    {
        void OnEnable();
        void OnDisable();
    }
    
    public class GlobalEventer : ILifecycle
    {
        private static GlobalEventer _instance;

        public static GlobalEventer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalEventer();
                }

                return _instance;
            }
        }
        public event UnityAction OnWindowEnable;
        public event UnityAction OnWindowDisable;

        public void OnEnable()
        {
            OnWindowEnable?.Invoke();
            DebugLogger.Instance.Log(this,"Window Enable");
        }

        public void OnDisable()
        {
            OnWindowDisable?.Invoke();
            DebugLogger.Instance.Log(this,"Window Disable");
        }
    }
}