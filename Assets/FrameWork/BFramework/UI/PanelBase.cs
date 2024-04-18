using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BFramework.UI
{
    public enum PanelState
    {
        SHOW_START,
        SHOW_OVER,
        HIDE_START,
        HIDE_OVER,
    }
    public class PanelBase :PanelCore
    {
        public Transform View;
        private PanelState _mPanelState; 
        public float hideTime = 0;
        public PanelState PanelState
        {
            get => _mPanelState;
            set => _mPanelState = value;
        }

        public Dictionary<string,Sprite> Res = new Dictionary<string, Sprite>();
        public void Init()
        {
            OnInit();
            doShow();
        }
        public void Show()
        {
            
            doShow();
        }
        public void Hide()
        {
            View.gameObject.SetActive(false);
            doHide();
        }
        public void HideIme()
        {
            doHide();
        }
        protected virtual void OnInit()
        {
        }
        protected virtual void OnShow()
        {
        }
        protected virtual void OnHide()
        {
        }
        protected virtual void OnConfig()
        {
        }
     
        
        public  void LoadConfig()
        {       
            OnConfig();
           
        }
        private void doShow()
        {
            View.gameObject.SetActive(true);
            OnShow();
            PanelState = PanelState.SHOW_OVER;
        }
        private void doHide()
        {
            OnHide();
        }
      
        protected Sprite LoadTexture(string name)
        {
            Res.TryGetValue(name, out var result);
            return result;

        }
        protected void UnLoadTexture(string name)
        {
            if (Res.TryGetValue(name, out var result))
            {
                Addressables.Release(result);
                Res.Remove(name);
            }
        }
        
       
    }
}