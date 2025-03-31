using System.Collections.Generic;
using GirlBoardEditor.Model;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public delegate void DelegateGirlChanged(GirlInfo girlInfo);
    public interface ISelectedGirl
    {
        void OnSelectedGirl(GirlInfo girlInfo);
        void AddListener(DelegateGirlChanged action);
        void RemoveListener(DelegateGirlChanged action);
    }
    
    public class EditorViewModel : BaseViewModel,ISelectedGirl
    {
        public event DelegateGirlChanged OnSelectGirl; 

        private GirlDetailViewModel m_GirlDetail;
        private GirlInfoListViewModel m_GirlList;
        
        private List<GirlInfo> m_GirlInfos = new ();
        public List<GirlInfo> MGirlInfos => m_GirlInfos;
        
        public EditorViewModel(VisualElement root) : base(root)
        {
            m_GirlDetail = new GirlDetailViewModel(root,this);
            m_GirlList = new GirlInfoListViewModel(root,this);
        }

        public void OnSelectedGirl(GirlInfo girlInfo)
        {
            OnSelectGirl?.Invoke(girlInfo);
        }

        public void AddListener(DelegateGirlChanged action)
        {
            OnSelectGirl += action;
        }

        public void RemoveListener(DelegateGirlChanged action)
        {
            OnSelectGirl -= action;
        }
    }
}