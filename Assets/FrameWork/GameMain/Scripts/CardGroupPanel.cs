using BFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace BFramework
{
    public class CardGroupPanel : PanelBase
    {
        public static bool isStore = false;
        public static int type = 3;
        private IBattleModel _battleModel;
        private GameObject content;
        private Button exitBtn;
        private GameObject prefabObj;
        protected override  void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("CardGroupPanel"),
                resNames = {Resname.RoleTexture("warrior2"),Resname.RoleTexture("warrior"),Resname.RoleTexture("wizard"),Resname.RoleTexture("assassins")}
            };
           
        }
        protected override void OnInit()
        {
           
        }

        private void ClickCard(int k)
        {
            var allCard = _battleModel.GetAllCard();
            if (type ==1)
            {
                if (allCard[k]!=null)
                {
                     var id = allCard[k].nextlevel;
                     allCard.Remove(allCard[k]);
                     var c = Config.GetRow("card",id) as cardRow;
                     allCard.Add(c);
                }
            }

            if (type ==2 )
            {
                if (allCard[k]!=null)
                {
                    allCard.Remove(allCard[k]);
                }
            }

            if (isStore)
            {
                _battleModel.SetMoney(_battleModel.GetMoney() -20);
                PanelManager.Instance.HidePanel<CardGroupPanel>();
            }
            if (_battleModel.GetRoomType() == RoomType.fire)
            {
                EventManager.Global.Send(new UpdateLevel(_battleModel.GetLevel(), _battleModel.GetRoomId()));
                PanelManager.Instance.HidePanel<FirePanel>();
                PanelManager.Instance.HidePanel<CardGroupPanel>();
            }
        }
        protected override async void OnShow()
        {
            _battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            exitBtn = TransformUtilty.find(transform, "exit").GetComponent<Button>();
            content = TransformUtilty.find(transform, "Content").gameObject;
            var allCard = _battleModel.GetAllCard();
            if (prefabObj == null)
            {
                prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.CardShow()).Task;
            }
         
            content.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,(allCard.Count / 4 + 1)*550);

            if (type ==1)
            {
                foreach (var c in allCard)
                {
                    if (c.nextlevel != 0)
                    {
                        var obj = Instantiate(prefabObj,content.transform, false);
                        TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                        obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                        var k =allCard.IndexOf(c);
                        obj.GetComponentInChildren<Button>().onClick.AddListener(() => { ClickCard(k);});
                    }
                }
            }
            else if (type ==4)//dead
            {
                var deadCard = _battleModel.GetDeadCard();
                foreach (var c in deadCard)
                {
                    var obj = Instantiate(prefabObj,content.transform, false);
                    TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                    Destroy(obj.GetComponentInChildren<Button>());
                }
            }
            else if (type ==5)
            {
                var readycard = _battleModel.GetReadyCard();
                foreach (var c in readycard)
                {
                    var obj = Instantiate(prefabObj,content.transform, false);
                    TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                    Destroy(obj.GetComponentInChildren<Button>());
                }
            }
            else
            {
                foreach (var c in allCard)
                {
                    var obj = Instantiate(prefabObj,content.transform, false);
                    TransformUtilty.find(obj.transform, "cardbg").GetComponent<Image>().sprite = LoadTexture(Resname.RoleTexture(c.png));
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = c.des;
                    var k =allCard.IndexOf(c);
                    obj.GetComponentInChildren<Button>().onClick.AddListener(() => { ClickCard(k); });
                }
            }
            exitBtn.onClick.AddListener((() =>
            {
                PanelManager.Instance.HidePanelIme<CardGroupPanel>();
            }));
        }

        protected override void OnHide()
        {
            isStore = false;
            type = 3;
            // var s= content.GetComponentsInChildren<Image>();
            // foreach (var o in s)
            // {
            //     DestroyImmediate(o.gameObject);
            // }
            exitBtn.onClick.RemoveAllListeners();
        }
    }  
}