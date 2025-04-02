using GirlBoardEditor.Model;
using GirlBoardEditor.Tools;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public abstract class BaseViewModel : ILifecycle
    {
        protected VisualElement m_root;
        protected BaseModel m_Model;
       
        public BaseViewModel(VisualElement mRoot,BaseModel model)
        {
            this.m_root = mRoot;
            this.m_Model = model;
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