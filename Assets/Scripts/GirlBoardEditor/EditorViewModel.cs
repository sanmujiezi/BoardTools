using System.Collections.Generic;
using GirlBoardEditor.Model;
using UnityEngine.UIElements;

namespace GirlBoardEditor
{
    public delegate void DelegateGirlChanged(GirlInfoModel girlInfoModel);
    public interface ISelectedGirl
    {
        void OnSelectedGirl(GirlInfoModel girlInfoModel);
        void AddListener(DelegateGirlChanged action);
        void RemoveListener(DelegateGirlChanged action);
    }
    
    public class EditorViewModel : BaseViewModel,ISelectedGirl
    {
        public event DelegateGirlChanged OnSelectGirl; 

        private GirlDetailViewModel m_GirlDetail;
        private GirlInfoListViewModel m_GirlList;
        
        private List<GirlInfoModel> m_GirlInfos = new ();
        public List<GirlInfoModel> MGirlInfos => m_GirlInfos;
        
        public EditorViewModel(VisualElement root,BaseModel model) : base(root,model)
        {
            BaseModel girlData = new GirlDataModel();
            BaseModel girlInfo = new GirlInfoModel();
            m_GirlDetail = new GirlDetailViewModel(root,this,girlData);
            m_GirlList = new GirlInfoListViewModel(root,this,null);
        }

        public void OnSelectedGirl(GirlInfoModel girlInfoModel)
        {
            OnSelectGirl?.Invoke(girlInfoModel);
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