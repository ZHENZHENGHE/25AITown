using System;
using BFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BFramework
{
    public class WinPanel : PanelBase
    {
        private IBattleModel _battleModel;
        private GameObject choose;
        private GameObject prefabObj;
        private Button abandon;
        private TextMeshProUGUI money;
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("WinPanel"),
                resNames = {Resname.RoleTexture("warrior2"),Resname.RoleTexture("warrior"),Resname.RoleTexture("wizard"),Resname.RoleTexture("assassins")}
            };
        }
        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override async void OnShow()
        {
            _battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            choose = TransformUtilty.find(transform, "choose").gameObject;
            abandon = TransformUtilty.find(transform, "abandon").GetComponent<Button>();
            money = TransformUtilty.find(transform, "money").GetComponent<TextMeshProUGUI>();
            money.text = "获得金币"+((_battleModel.GetLevel() + 1) * 10);
            abandon.onClick.AddListener((() =>
            {
                EventManager.Global.Send(new OverBattle(BattleFsm.isWin));
                EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(), _battleModel.GetRoomId()));
                PanelManager.Instance.HidePanel<WinPanel>();
            }));
            var allCard = Config.GetTable("card");
            if (prefabObj == null)
            {
                prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.CardShow()).Task;
            }

            System.Random r = new System.Random(DateTime.Now.Millisecond);
            int i = 1;
            while (i <= 3)
            {
                var n = r.Next(1, allCard.Count + 1);
                if (allCard.TryGetValue(n,out var card))
                {
                    if (card is cardRow c)
                    {
                        var obj = Instantiate(prefabObj,choose.transform, false);
                        TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                        obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                        obj.GetComponentInChildren<Button>().onClick.AddListener(() => { ClickCard(n); });
                        i++; 
                    }
                   
                }
            }
        }

        public void ClickCard(int k)
        {
            EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(), _battleModel.GetRoomId()));
            _battleModel.SetMoney(_battleModel.GetMoney()+(_battleModel.GetLevel()+1)*10);
            _battleModel.AddAllCard(k);
            EventManager.Global.Send(new OverBattle(BattleFsm.isWin));
            PanelManager.Instance.HidePanel<WinPanel>();
          
        }
        protected override void OnHide()
        {
          
        }

       
    }

}