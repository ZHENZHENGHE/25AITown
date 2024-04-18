using BFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace BFramework
{
    public class MapNode : MonoBehaviour
    {
        public int value;
        public MapNode left;
        public MapNode right;
        public int level;
        public bool isUsed = false;
        public Image img;
        public Button btn;
        public RoomType type = 0;
        public bool isPass = false;
        private void Awake()
        {
            img = transform.GetComponent<Image>();
            btn = transform.GetComponent<Button>();
            btn.onClick.AddListener((() =>
            {
                var battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
                battleModel.SetLevel(level);
                battleModel.SetRoomId(value);
                battleModel.SetRoomType(type);
                Debug.Log(level + "........." + value);
                if (type == RoomType.normal_monster)
                {
                    //EventManager.Global.Send<StartBattle>();
                    PanelManager.Instance.ShowPanel<StorePanel>(nextHide:false);
                }
                if (type == RoomType.store)
                {
                    PanelManager.Instance.ShowPanel<StorePanel>(nextHide:false);
                }
                if (type == RoomType.boss)
                {
                    EventManager.Global.Send<StartBattle>();
                }
                if (type == RoomType.fire)
                {
                    PanelManager.Instance.ShowPanel<FirePanel>(nextHide:false);
                }
             

            }));
            btn.interactable = false;
            isPass = false;
        }

        private void Update()
        {
          
        }

        public int Init()
        {
            
            if (level <=5 && level>2)
            {
                if (Random.Range(1, 100) < 20)
                {
                    type = RoomType.store;
                }else if (Random.Range(1, 100) < 20)
                {
                    type = RoomType.fire;
                }else if (Random.Range(1, 100) < 10)
                {
                    type = RoomType.boss;
                }
                else
                {
                    type = RoomType.normal_monster;
                }
                    
            }
            else if (level >5 )
            {
               
                if (Random.Range(1, 100) < 30)
                {
                    type = RoomType.fire;
                }else if (Random.Range(1, 100) < 50)
                {
                    type = RoomType.boss;
                }
                else
                {
                    type = RoomType.normal_monster;
                }
            }
            else
            {
                if (Random.Range(1, 100) < 10)
                {
                    type = RoomType.store;
                }
                else if (Random.Range(1, 100) < 15)
                {
                    type = RoomType.fire;
                }
                else
                {
                    type = RoomType.normal_monster;
                }
            }
            return (int)type;
        }
    }
    
}