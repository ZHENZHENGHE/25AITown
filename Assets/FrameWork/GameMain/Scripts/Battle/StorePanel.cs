using System;
using System.Collections;
using System.Collections.Generic;
using BFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace BFramework
{
    public class StorePanel : PanelBase
    {
        public Button hp;
        public Button removeCard;
        public Button UpdateCard;
        public Button exit;
        private IBattleModel _battleModel;
        private GameObject cards;
        private GameObject prefabObj;
        private TextMeshProUGUI money;
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("StorePanel"),
                resNames =
                {
                    Resname.RoleTexture("warrior2"), Resname.RoleTexture("warrior"), Resname.RoleTexture("wizard"),
                    Resname.RoleTexture("assassins")
                }
            };
        }
        protected override  void OnInit()
        {
            
        }

        public void Money(Money e)
        {
            if (money)
            {
                money.text = _battleModel.GetMoney().ToString();
            }
         
        }
        protected override async void OnShow()
        {
           
            _battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            hp = TransformUtilty.find(transform, "Hp").GetComponent<Button>();
            removeCard = TransformUtilty.find(transform, "removeCard").GetComponent<Button>();
            UpdateCard = TransformUtilty.find(transform, "UpdateCard").GetComponent<Button>();
            exit = TransformUtilty.find(transform, "exit").GetComponent<Button>();
            cards =  TransformUtilty.find(transform, "cards").gameObject;
            money = TransformUtilty.find(transform, "money").GetComponent<TextMeshProUGUI>();
            money.text = _battleModel.GetMoney().ToString();
            AddEvent<Money>(Money);
            if (prefabObj == null)
            {
                prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.CardShow()).Task;
            }
            var objs =cards.GetComponentsInChildren<Button>();
            foreach (var obj in objs)
            {
                DestroyImmediate(obj.transform.parent.gameObject);
            }
            var store = Config.GetTable("card");
            System.Random r = new System.Random(DateTime.Now.Millisecond);
            int i = 1;
            while (i <= 4)
            {
                var n = r.Next(1, store.Count + 1);
                if (store.TryGetValue(n,out var card))
                {
                    if (card is cardRow c)
                    {
                        var obj = Instantiate(prefabObj,cards.transform, false);
                        TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                        var money = TransformUtilty.find(obj.transform, "money").gameObject;
                        money.SetActive(true);
                        money.GetComponentInChildren<TextMeshProUGUI>().text = c.cost.ToString();
                        obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                        obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
                        {
                            if (_battleModel.GetMoney()<c.cost)
                            {
                                return;
                            }
                            var allCard = _battleModel.GetAllCard();
                            if (allCard.Contains(allCard[n-1]))
                            {
                                allCard.Add(allCard[n-1]);
                            }
                            _battleModel.SetMoney(_battleModel.GetMoney()-c.cost);
                            obj.GetComponentInChildren<Button>().interactable = false;
                        });
                        i++; 
                    }
                   
                }
            }
            exit.onClick.AddListener(() =>
            {
                EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(), _battleModel.GetRoomId()));
                PanelManager.Instance.HidePanel<StorePanel>();
            });
            hp.onClick.AddListener((() =>
            {
                if (_battleModel.GetMoney()<20)
                {
                    return;
                }
                _battleModel.SetMoney(_battleModel.GetMoney() -20);
                _battleModel.SetHp(_battleModel.GetMaxHp());
                EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(),_battleModel.GetRoomId()));
                PanelManager.Instance.HidePanel<FirePanel>();
            }));
            removeCard.onClick.AddListener((() =>
            {
                if (_battleModel.GetMoney()<20)
                {
                    return;
                }
                CardGroupPanel.isStore = true;
                CardGroupPanel.type = 2;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
            UpdateCard.onClick.AddListener((() =>
            {
                if (_battleModel.GetMoney()<20)
                {
                    return;
                }
                CardGroupPanel.isStore = true;
                CardGroupPanel.type = 1;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
        }

        protected override void OnHide()
        {
         
            RemoveEvent<Money>(Money);
            hp.onClick.RemoveAllListeners();
            removeCard.onClick.RemoveAllListeners();
            UpdateCard.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
        }
    } 
}

