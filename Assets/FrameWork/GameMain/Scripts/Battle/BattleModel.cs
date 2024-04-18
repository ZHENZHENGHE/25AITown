using System.Collections.Generic;

namespace BFramework
{
    public interface IBattleModel : IModel
    {
        public void SetRoleType(RoleType t);

        public void SetLevel(int l);
        public void SetRoomType(RoomType t);
        public RoomType GetRoomType();

        public int GetLevel();

        public RoleType GetRoleType();
        public void SetHp(int value);

        public int GetHp();

        public void SetMoney(int value);

        public int GetMoney();

        public Dictionary<int,Monster> GetMonsters();
        public void SetRole(IRole r);
        public IRole GetRole();
        public void SetMonsterNum(int n);

        public int GetMonsterNum();
        public void SetMaxHp(int value);

        public int GetMaxHp();
        public void AddCardToHand(Card card);

        public void AddCardToDead(Card card);

        public void AddDeadToCard();

        public void AddAllCard(int id);

        public void RemoveAllCard(int id);

        public void AllCardToReady();
        public List<Card> GetHandCard();
        public List<cardRow> GetReadyCard();
        public void SetReadyCard(List<cardRow> v);
        public void InitAllCard();
        public bool GetEndTurn();
        public void SetEndTurn(bool b);
        public int GetTakeCardNum();

        public void SetTakeCardNum(int b);

        public void Clear();
        public void LevelClear();
        public int GetRoomId();

        public void SetRoomId(int v);
        public List<cardRow> GetAllCard();
        public List<cardRow> GetDeadCard();
    }
    [Model("BattleModel",Lifetime.Singleton)]
    public class BattleModel :AbstractModel,IBattleModel
    {
        public static bool CanTouch = true;
        private  RoleType _roleType;
        private  int _level;
        private  int _RoomId;
        private  RoomType _roomType;
        private  int _hp;
        private  int _Maxhp;
        private  int _money;
        private  IRole _role;
        private  int _mNum;
        private  int _takeCardNum = 3;
        private  bool _endTurn;
        private  List<Card> _handCard = new List<Card>();
        private  List<cardRow> _deadCard = new List<cardRow>();
        private  List<cardRow> _readyCard = new List<cardRow>();
        private  List<cardRow> _allCard = new List<cardRow>();
        private  Dictionary<int,Monster> _monsters = new Dictionary<int,Monster>();
        public List<cardRow> GetDeadCard()
        {
            return _deadCard;
        }
        public void Clear()
        {
            _handCard.Clear();
            _deadCard.Clear();
            _readyCard.Clear();
            _allCard.Clear();
            _monsters.Clear();
            _endTurn = false;
            _role = null;
            _money = 30;
            _Maxhp = 0;
            _hp = 0;
            _roomType = 0;
            _level = 0;
            _roleType = 0;
            CanTouch = true;
        }
        public void LevelClear()
        {
            _handCard.Clear();
            _deadCard.Clear();
            _readyCard.Clear();
            _monsters.Clear();
            _endTurn = false;
            _roleType = 0;
            CanTouch = true;
        }
        public int GetRoomId()
        {
            return _RoomId;
        }
        public void SetRoomId(int v)
        {
            _RoomId = v;
        }
        protected override void OnInit()
        {
            _money = 30;
        }
        public int GetTakeCardNum()
        {
            return _takeCardNum;
        }
        public void SetTakeCardNum(int b)
        {
            _takeCardNum = b;
        }
        public bool GetEndTurn()
        {
            return _endTurn;
        }
        public void SetEndTurn(bool b)
        {
            _endTurn = b;
        }
        public IRole GetRole()
        {
           return _role;
        }

        public void SetMonsterNum(int n)
        {
            _mNum = n;
        }
        public int GetMonsterNum()
        {
            return _mNum;
        }
        public void SetRole(IRole r)
        {
            _role = r;
        }
        public void SetRoleType(RoleType t)
        {
            _roleType = t;
        }
        public void SetLevel(int l)
        {
            _level = l;
        } 
        public void SetRoomType(RoomType t)
        {
            _roomType = t;
        }
        public RoomType GetRoomType()
        {
            return _roomType;
        }
        public int GetLevel()
        {
            return _level;
        } 
        public RoleType GetRoleType()
        {
            return _roleType;
        }
        public void SetHp(int value)
        {
            _hp = value;
            EventManager.Global.Send<Hp>();
        }
        public int GetHp()
        {
            return _hp;
        }
        public void SetMaxHp(int value)
        {
            _Maxhp = value;
        }
        public int GetMaxHp()
        {
            return _Maxhp;
        }
        public void SetMoney(int value)
        {
            _money = value;
            EventManager.Global.Send<Money>();
        }
        public int GetMoney()
        {
            return _money;
        }
      
        public Dictionary<int,Monster> GetMonsters()
        {
            return _monsters;
        }
        public void AddCardToHand(Card card)
        {
            
            if (_readyCard.Contains(card.CardRow) && !_handCard.Contains(card))
            {
                _readyCard.Remove(card.CardRow);
                _handCard.Add(card);
            }
            else
            {
                LogKit.E("找不到这牌的ID");
            }
            EventManager.Global.Send<UpdateCardText>();
        }

  

        public void AddCardToDead(Card card)
        {
            if (_handCard.Contains(card) && !_deadCard.Contains(card.CardRow))
            {
                _handCard.Remove(card);
                _deadCard.Add(card.CardRow);
            }
            else
            {
                LogKit.E("找不到这牌的ID");
            }
            EventManager.Global.Send<UpdateCardText>();
        }
        public void AddDeadToCard()
        {
            if (_readyCard.Count>0)
            {
                LogKit.E("_readyCard.Count>0");
                return;
            }
            foreach (var c in _deadCard)
            {
                _readyCard.Add(c);
            }
            _deadCard.Clear();
            EventManager.Global.Send<UpdateCardText>();
        }
        public void AddAllCard(int id)
        {
            Config.LoadConfig("card");
            var c = Config.GetRow("card", id) as cardRow;
           _allCard.Add(c);
        }
        public void RemoveAllCard(int id)
        {
            Config.LoadConfig("card");
            var c = Config.GetRow("card", id) as cardRow;
            _allCard.Remove(c);
        }
        public void AllCardToReady()
        {
            foreach (var c in _allCard)
            {
                _readyCard.Add(c);
            }
            EventManager.Global.Send<UpdateCardText>();
        }
        public List<Card> GetHandCard()
        {
            return _handCard;
        }

        public List<cardRow> GetReadyCard()
        {
            return _readyCard;
        }

        public void SetReadyCard(List<cardRow> v)
        {
            _readyCard = v;
        }

        public List<cardRow> GetAllCard()
        {
            return _allCard;
        }
        public void InitAllCard()
        {
            Config.LoadConfig("card");
            var table = Config.GetTable("card");
            foreach (var c in table)
            {
                var card = c.Value as cardRow;
                if (card.isInit == 1)
                {
                    _allCard.Add(card);
                }
             
            }
        }
    }
}