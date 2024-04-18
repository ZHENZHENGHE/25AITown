using System;
using System.Collections.Generic;
using System.Linq;
using BFramework.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BFramework
{
    public class BattlePanel : PanelBase
    {
        public Image bg;
        public GameObject cardgroup;
        public List<GameObject> cardGroup = new List<GameObject>();
        private IBattleModel battleModel;
        private IBattleSystem battleSystem;
        private GameObject monster;
        private GameObject _role;
        private TextMeshProUGUI _topHp;
        private TextMeshProUGUI _money;
        private TextMeshProUGUI _energy;
        private TextMeshProUGUI _readyCardtxt;
        private TextMeshProUGUI _deadCardtxt;
        private GameObject _readyCard;
        private GameObject _deadCard;
        private BattleFsm _BattleFsm;
        private Button endButton;
        public GameObject Role;
        public GameObject prefabCard;
        public System.Random random = new System.Random();
        private List<GameObject> monsters = new List<GameObject>();
        protected override void OnConfig()
        {
            config = new PanelConfig()
            {
                panel = Resname.UI("BattlePanel"),
                resNames = {Resname.Texture("bg3"),Resname.Texture("bg2"),Resname.Texture("battlebg1"),Resname.RoleTexture("assassins"),Resname.RoleTexture("wizard"),Resname.RoleTexture("warrior")}
                
            };
        }
    
        protected  override async  void OnInit()
        {
            LogKit.Level = LogKit.LogLevel.Max;
           
            bg = TransformUtilty.find(transform,"BattleBg").GetComponent<Image>();
            monster = TransformUtilty.find(transform,"monsters").gameObject;
            cardgroup = TransformUtilty.find(transform,"cardgroup").gameObject;
            battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            _topHp = TransformUtilty.find(transform, "hp").GetComponentInChildren<TextMeshProUGUI>();
            _money = TransformUtilty.find(transform, "money").GetComponentInChildren<TextMeshProUGUI>();
            _role = TransformUtilty.find(transform,"role").gameObject;
            _readyCard = TransformUtilty.find(transform,"readyCard").gameObject;
            _readyCardtxt = _readyCard.GetComponentInChildren<TextMeshProUGUI>();
            _deadCard = TransformUtilty.find(transform,"deadcard").gameObject;
            _deadCardtxt = _deadCard.GetComponentInChildren<TextMeshProUGUI>();
            battleSystem =  App.Interface.GetSystem<IBattleSystem>("BattleSystem");
            endButton = TransformUtilty.find(transform, "endturn").GetComponentInChildren<Button>();
            _energy = TransformUtilty.find(transform, "p").GetComponentInChildren<TextMeshProUGUI>();
            battleSystem.AllCardToReady();
            ShuffleInit();
            GenerateRole();
            GenerateMonster();
            AddEvent<ATTACK>(Attack);
            AddEvent<Hp>(UpdateTopHp);
            AddEvent<UpdateCardText>(UpdateCardText);
            AddEvent<Money>(UpdateTopMoney);
            AddEvent<AddCard>(AddCard);
            AddEvent<Energy>(UpdateEnergy);
            prefabCard = await Addressables.LoadAssetAsync<GameObject>(Resname.Card()).Task;
            //AddCard(new AddCard(5));
            _BattleFsm = transform.AddComponent<BattleFsm>();
        }
        public async void GenerateRole()
        {
            var type = battleModel.GetRoleType();
            GameObject prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.Role()).Task;
            var obj = Instantiate(prefabObj, _role.transform, false);
            Role = obj;
            Config.LoadConfig("role");
            var roleData = Config.GetTable("role");
            if (roleData.TryGetValue((int)type+1,out var result ))
            {
                IRole role;
                var roleTable = result as roleRow;
                if (roleTable == null)
                {
                    return;
                }
                var slider = Role.transform.GetComponentInChildren<Slider>();
                var img = TransformUtilty.find(Role.transform,"png").GetComponent<Image>();
                var text = TransformUtilty.find(Role.transform,"hpt").GetComponent<TextMeshProUGUI>();
                var s = LoadTexture(Resname.RoleTexture(roleTable.png));
                var d = TransformUtilty.find(Role.transform, "def").gameObject;
                var dm = d.GetComponent<Image>();
                var dt = d.GetComponentInChildren<TextMeshProUGUI>();
                switch (type+1)
                {
                    case RoleType.assassins:
                        role = new Assassins(roleTable,s,img,slider,text,dt,dm);
                        break;
                    case RoleType.wizard:
                        role = new Wizard(roleTable,s,img,slider,text,dt,dm);
                        break;
                    case RoleType.warrior:
                        role = new Warrior(roleTable,s,img,slider,text,dt,dm);
                        break;
                }
            }
        }
        public async void GenerateMonster()
        {
            var type = battleModel.GetRoomType();
            var monsterNum = 1;
            if (type == RoomType.normal_monster)
            {
                monsterNum = 2;//Random.Range(1, 3);
            }
            battleModel.SetMonsterNum(monsterNum);
            for (int i = 0; i < battleModel.GetMonsterNum(); i++)
            {
                GameObject prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.Monster()).Task;
                var obj = Instantiate(prefabObj, monster.transform, false);
                monsters.Add(obj);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(i*300, 0);
            }
            Config.LoadConfig("monster");
            var monstersData = Config.GetTable("monster");
            for (int i = 0; i < monsterNum; i++)
            {
                var monsterType = Random.Range(1, monsters.Count);
                if (monstersData.TryGetValue(monsterType,out var result ))
                {
                    var monsterTable = result as monsterRow;
                    if (monsterTable == null)
                    {
                        return;
                    }
                    var slider = monsters[i].transform.GetComponentInChildren<Slider>();
                    var img = TransformUtilty.find(monsters[i].transform,"png").GetComponent<Image>();
                    var text = monsters[i].GetComponentInChildren<TextMeshProUGUI>();
                    var s = LoadTexture(Resname.RoleTexture(monsterTable.png));
                    var monster1 = new Monster(monsterTable,s,img,slider,text);
                    battleModel.GetMonsters().Add(i,monster1);
                }
                
            }
          
        }
        void Update()
        {
            if (cardgroup.transform.childCount>4)
            {
                cardgroup.GetComponent<HorizontalLayoutGroup>().spacing = (cardgroup.transform.childCount - 4)*-30;
            }
            else
            {
                cardgroup.GetComponent<HorizontalLayoutGroup>().spacing = 0;
            }
        }

        public void UpdateCardText(UpdateCardText e)
        {
            if (_deadCardtxt ==null ||_readyCardtxt == null)
            {
                return;
            }
            _readyCardtxt.text = battleModel.GetReadyCard().Count.ToString();
            _deadCardtxt.text = battleModel.GetDeadCard().Count.ToString();
        }
        protected override void OnShow()
        {
            _deadCard.GetComponent<Button>().onClick.AddListener((() =>
            {
                CardGroupPanel.type = 4;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
            _readyCard.GetComponent<Button>().onClick.AddListener((() =>
            {
                CardGroupPanel.type = 5;
                PanelManager.Instance.ShowPanel<CardGroupPanel>(nextHide:false);
            }));
            endButton.onClick.AddListener((() =>
            {
                if (_BattleFsm.FSM.CurrentStateId == BattleFsm.States.My)
                {
                    battleModel.SetEndTurn(true);
                }

            }));
            var num = Random.Range(0, 3);
            switch (num)
            {
                case 0:
                    bg.sprite = LoadTexture(Resname.Texture("battlebg1"));
                    break;
                case 1:
                    bg.sprite = LoadTexture(Resname.Texture("bg3"));
                    break;
                case 2:
                    bg.sprite = LoadTexture(Resname.Texture("bg2"));
                    break;
            }
         
            UpdateTopHp(new Hp());
            UpdateTopMoney(new Money());
        }

        public void UpdateEnergy(Energy e)
        {
            _energy.text = battleModel.GetRole().GetEnergy() + "/" + BFramework.Role._Maxenergy;
        }
        protected override void OnHide()
        {
            RemoveEvent<ATTACK>(Attack);
            RemoveEvent<Hp>(UpdateTopHp);
            RemoveEvent<Money>(UpdateTopMoney);
            RemoveEvent<AddCard>(AddCard);
            RemoveEvent<Energy>(UpdateEnergy);
            RemoveEvent<UpdateCardText>(UpdateCardText);
            foreach (var monster in monsters)
            {
                Destroy(monster.gameObject);
                //Addressables.Release(monster);
            }
            battleModel.GetMonsters().Clear();
            _deadCard.GetComponent<Button>().onClick.RemoveAllListeners();
            endButton.onClick.RemoveAllListeners();
            Destroy(_BattleFsm);
        }
        private void Attack(ATTACK e)
        {
            if (battleModel.GetMonsters().TryGetValue(e.monster, out var m))
            {
                m.UpdateHp(-e.attack);
                if (m.cur_hp<=0)
                {
                    monsters[e.monster].gameObject.SetActive(false);
                    Destroy(monsters[e.monster].gameObject);
                    battleModel.GetMonsters().Remove(e.monster);
                }
            }
        }

        private void UpdateTopHp(Hp e)
        {
            if (battleModel!=null)
            {
                _topHp.text = battleModel.GetHp() + "/" + battleModel.GetMaxHp();
            }
        }
        private void UpdateTopMoney(Money e)
        {
            if (battleModel!=null)
            {
                _money.text = battleModel.GetMoney().ToString();
            }
        }
        private void AddCard(int n)
        {
            var list = new List<Card>();
            var count = 0;
            foreach (var c in battleModel.GetReadyCard())
            {
                if (count == n)
                {
                    break;
                }
               
                    
                var obj = Instantiate(prefabCard, cardgroup.transform, false);
                var card = obj.GetComponentInChildren<Card>();
                var img = obj.GetComponentInChildren<Image>();
                var k = battleModel.GetReadyCard().IndexOf(c);
                card.Init(c,c.id);
                list.Add(card);
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                Timer.Instance.Schedule(() =>
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                    img.rectTransform.DOLocalMoveX(-1000, 0.4f).SetEase(Ease.Linear).From();
                },count*0.2f,0,1);
                count++;

            }
            foreach (var v in list)
            {
                battleModel.AddCardToHand(v);
            }
        }
        public void AddCard(AddCard e)//几张牌--牌库随机 --生成牌赋值--存入手牌model -- 显示在面板 --dotween
        {
            var shuffe = false;
            int next = 0;
          
            int n = 0;
            if (battleModel.GetReadyCard().Count<e.AddNum)
            {
                shuffe = true;
                n = battleModel.GetReadyCard().Count;
                next = e.AddNum - battleModel.GetReadyCard().Count;
                //todo 洗牌 
            }
            else
            {
                n = e.AddNum;
            }

            AddCard(n);
            if (shuffe)
            {
                battleModel.AddDeadToCard();
                int[] numbers = new int[battleModel.GetReadyCard().Count];
                var cards = battleModel.GetReadyCard();
                int i = 0;
                foreach (var c in cards)
                {
                    numbers[i] = cards.IndexOf(c);
                    i++;
                }
                Shuffle<int>(numbers);
                List<cardRow> temp = new List<cardRow>();
                foreach (var number in numbers)
                {
                    temp.Add(cards[number]);
                }
                battleModel.SetReadyCard(temp);
                AddCard(next);
            }
              
            
        }
        public void ShuffleInit()
        {
            int[] numbers = new int[battleModel.GetReadyCard().Count];
            var cards = battleModel.GetReadyCard();
            int i = 0;
            foreach (var c in cards)
            {
                numbers[i] = cards.IndexOf(c);
                i++;
            }
            Shuffle<int>(numbers);
            List<cardRow> temp = new List<cardRow>();
            foreach (var number in numbers)
            {
                temp.Add(cards[number]);
            }
            battleModel.SetReadyCard(temp);
        }
        public void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int r = random.Next(i + 1);
                (array[i], array[r]) = (array[r], array[i]);
            }
        }
        
    }

}