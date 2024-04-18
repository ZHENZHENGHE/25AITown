using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BFramework.UI
{
    public class ChooseHeroPanel : PanelBase
    {
        public Button wizard;
        public Button warrior;
        public Button assassins;
        public Button Enter;
        public Button Exit;
        private Dictionary<Button,bool> _buttons= new Dictionary<Button,bool>();
        public Image img;
        public Image warriorimg;
        private RoleType selectIndex;
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("ChooseHeroPanel"),
                resNames = {Resname.Texture("dikuang_04"),Resname.RoleTexture("warrior2"),Resname.RoleTexture("warrior")}
            };
        }
        protected override  void OnInit()
        {
            LogKit.Level = LogKit.LogLevel.Max;
            Exit = TransformUtilty.find(transform,"Exit").GetComponent<Button>();
            wizard =TransformUtilty.find(transform,"wizard").GetComponent<Button>();
            warrior =TransformUtilty.find(transform,"warrior").GetComponent<Button>();
            assassins =TransformUtilty.find(transform,"assassins").GetComponent<Button>();
            Enter = TransformUtilty.find(transform,"EnterButton").GetComponent<Button>();
            img = View.GetComponent<Image>();
            warriorimg =  TransformUtilty.find(transform,"warrior").GetComponent<Image>();
            
            
        }
        protected override  void OnShow()
        {
            _buttons.Add(wizard,false);
            _buttons.Add(warrior,false); 
            _buttons.Add(assassins,false);
            Enter.gameObject.SetActive(false);
            //img.sprite = LoadTexture(Resname.Texture("dikuang_04"));
            warriorimg.sprite = LoadTexture(Resname.RoleTexture("warrior2"));
            wizard.onClick.AddListener((() =>
            {
                buttonAnime(0, wizard.transform);
            }));
            warrior.onClick.AddListener((() =>
            {
                buttonAnime(1, warrior.transform);
            }));
            assassins.onClick.AddListener((() =>
            {
                buttonAnime(2, assassins.transform);
            }));
            Exit.onClick.AddListener((() =>
            {
                PanelManager.Instance.Back();
            }));
            Enter.onClick.AddListener((() =>
            {
                LogKit.I("进入游戏");
                PanelManager.Instance.ShowPanel<LevelPanel>(false,callback:(() =>
                {
                    PanelManager.Instance.HideAllPanel();
                }));
              
            }));
        
        }

        private void buttonAnime(int num,Transform transform)
        {
            var temp = new Dictionary<Button, bool>();
            foreach (var b in _buttons)
            {
                if (b.Key.transform == transform)
                {
                    if (!b.Value)
                    {
                        var tweener =  b.Key.transform.DOLocalMoveY(80, 0.4f);
                        tweener.SetEase(Ease.InBack);
                        temp.Add(b.Key,true);
                    }
                    else
                    {
                        temp.Add(b.Key,true);
                    }
                }
                else
                {
                    if (b.Value)
                    {
                        var tweener =  b.Key.transform.DOLocalMoveY(0, 0.4f);
                        tweener.SetEase(Ease.InBack);
                        temp.Add(b.Key,false);
                    }
                    else
                    {
                        temp.Add(b.Key,false);
                    }
                }
            }

            _buttons = temp;
            selectIndex = (RoleType)num;
            var battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            var battleSystem =  App.Interface.GetSystem<IBattleSystem>("BattleSystem");
            battleSystem.InitAllCard();
            battleModel.SetRoleType(selectIndex);
            Config.LoadConfig("role");
            if (Config.GetRow("role", (int)selectIndex + 1) is roleRow row)
            {
                battleModel.SetMaxHp(row.hp);
                battleModel.SetHp(row.hp);
            }
            
            var c = Enter.image.color;
            if (c.a==0)
            {
                Enter.gameObject.SetActive(true);
                Enter.image.DOFade(1, 0.1f);
            }
            
        }
        protected override void OnHide()
        {
            _buttons.Clear();
            var c = Enter.image.color;
            Enter.image.color = new Color(c.r, c.g, c.b, 0);
            wizard.onClick.RemoveAllListeners();
            warrior.onClick.RemoveAllListeners();
            assassins.onClick.RemoveAllListeners();
            Enter.onClick.RemoveAllListeners();
            Exit.onClick.RemoveAllListeners();
            img.sprite = null;
            warriorimg.sprite = null;
        }

        void OnDestroy()
        {
            UnLoadTexture(Resname.Texture("dikuang_04"));
            UnLoadTexture(Resname.RoleTexture("warrior2"));
        }

    
    }
}