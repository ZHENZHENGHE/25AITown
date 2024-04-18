using System.Collections;
using System.Collections.Generic;
using BFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BFramework
{
    public class FirePanel : PanelBase
    {
        public Button hp;
        public Button removeCard;
        public Button UpdateCard;
        private IBattleModel _battleModel;
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("FirePanel"),
                resNames = {}
            };
        }
        protected override  void OnInit()
        {
            _battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            hp = TransformUtilty.find(transform, "Hp").GetComponent<Button>();
            removeCard = TransformUtilty.find(transform, "removeCard").GetComponent<Button>();
            UpdateCard = TransformUtilty.find(transform, "UpdateCard").GetComponent<Button>();
        }

        protected override void OnShow()
        {
            hp.onClick.AddListener((() =>
            {
                _battleModel.SetHp(_battleModel.GetMaxHp());
                EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(),_battleModel.GetRoomId()));
                PanelManager.Instance.HidePanel<FirePanel>();
            }));
            removeCard.onClick.AddListener((() =>
            {
                CardGroupPanel.type = 2;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
            UpdateCard.onClick.AddListener((() =>
            {
                CardGroupPanel.type = 1;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
        }

        protected override void OnHide()
        {
            hp.onClick.RemoveAllListeners();
            removeCard.onClick.RemoveAllListeners();
            UpdateCard.onClick.RemoveAllListeners();
        }
    }  
}

