using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public abstract class BaseViewModel : ILifecycle
    {
        protected VisualElement root;
       
        public BaseViewModel(VisualElement root)
        {
            this.root = root;
            GlobalEventer.Instance.OnWindowEnable += OnEnable;
            GlobalEventer.Instance.OnWindowDisable += OnDisable;
        }

        public virtual void OnEnable()
        {
            DebugLogger.Instance.Log(this,$"{this.GetType().Name} is Enable");
        }

        public virtual void OnDisable()
        {
            GlobalEventer.Instance.OnWindowEnable -= OnEnable;
            GlobalEventer.Instance.OnWindowDisable -= OnDisable;
            
            DebugLogger.Instance.Log(this,$"{this.GetType().Name} is Disable");
            
        }
    }
}