using BFramework.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BFramework.UI
{
    public class StartPanel : PanelBase
    {
        public Button startButton;
        public Button ViewButton;
        public Button ExitButton;
        public Image img;
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("StartPanel"),
                resNames = 
                {
                    Resname.Texture("GameStartBG")
                }
            };
        }

       
        protected override  void OnInit()
        {
            startButton =TransformUtilty.find(transform,"startButton").GetComponent<Button>();
            ViewButton =TransformUtilty.find(transform,"ViewButton").GetComponent<Button>();
            ExitButton =TransformUtilty.find(transform,"ExitButton").GetComponent<Button>();
            img = View.GetComponent<Image>();
        
           
        }
        protected override  void OnShow()
        {
            img.sprite = LoadTexture(Resname.Texture("GameStartBG"));
            ViewButton.onClick.AddListener((() =>
            {
              
                LogKit.Level = LogKit.LogLevel.Max;
                LogKit.W("666");
                Timer.Instance.Schedule((() => {  PanelManager.Instance.HidePanel(typeof(ChooseHeroPanel));}), 5, 0, 1);
                Timer.Instance.Schedule((() => { Debug.Log("5");}), 5, 0, 1);
                EventManager.Global.Send<UISHOW>(new UISHOW()
                {
                    age = 10,
                });
            }));
            startButton.onClick.AddListener((() =>
            {
                PanelManager.Instance.ShowPanel(typeof(ChooseHeroPanel),nextHide:false);
            }));
            ExitButton.onClick.AddListener((() =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }));
         
            AddEvent<UISHOW>(UISHOWcall);
        
        }
        protected override void OnHide()
        {
            startButton.onClick.RemoveAllListeners();
            ViewButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
          
            img.sprite = null;
        }
        private void UISHOWcall(UISHOW e)
        {
            LogKit.I("666{0}",e.age);
            // Debug.Log("6999"+e.age);
        }

        void OnDestroy()
        {
            UnLoadTexture(Resname.Texture("GameStartBG"));
        }
    }
}