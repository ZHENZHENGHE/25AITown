using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BFramework
{
    public interface IRole
    {
        public void SetDef(int v);
        public int GetDef();
        public void UpdateHp(int value);
        public int GetCurHp();
        public int GetEnergy();
        public void SetEnergy(int v);
    }  
    public class Role:IRole
    {
       
        public string name;
        public int def;
        public int hp;
        public int cur_hp;
        public int _energy;
        public static int _Maxenergy = 5;
        public int skill;
        public Image img;
        public Slider slider;
        public TextMeshProUGUI tex;
        public TextMeshProUGUI deftext;
        public IBattleModel model;
        public Image defimg;
        public GameObject Def;
        public Role(IRow data,Sprite s,Image img,Slider slider,TextMeshProUGUI textMeshPro,TextMeshProUGUI deftext,Image defimg)
        {
            model = App.Interface.GetModel<IBattleModel>("BattleModel");
            model.SetRole(this);
            this.slider = slider;
            this.img = img;
            this.img.sprite = s;
            tex = textMeshPro;
            this.deftext = deftext;
            this.defimg = defimg;
            Def = defimg.gameObject;
            if (data is roleRow role)
            {
                name = role.name;
                def = role.def;
                hp = model.GetMaxHp();
                cur_hp = model.GetHp();
                tex.text = cur_hp + "/" + hp;
                UpdateHp(0);
                SetEnergy(_Maxenergy);
            }
        }

        public void UpdateHp(int value)
        {
            if ((cur_hp + value)>hp)
            {
                cur_hp = hp;
                
            }
            else if((cur_hp + value)<= 0)
            {
                cur_hp = 0;
            }
            else
            {
                cur_hp += value;
            }
            
            tex.text = cur_hp + "/" + hp;
            slider.value = cur_hp* 100 / hp ;
            model.SetHp(cur_hp);
        }

        public void SetDef(int v)
        {
            if (def != v)
            {
                def = v;
                if (def<=0)
                {
                    Def.SetActive(false);
                }
                else
                {
                    Def.SetActive(true);
                }
                Debug.Log("防御力："+def);
                deftext.text = def.ToString();
                EventManager.Global.Send<Def>();
            }
        }
        public int GetDef()
        {
            return def;
        }
        public int GetCurHp()
        {
            return cur_hp;
        }

        public int GetEnergy()
        {
            return _energy;
        }
        public void SetEnergy(int v)
        {
            if ( v>=_Maxenergy)
            {
                _energy = _Maxenergy;
            }else if(v<=0)
            {
                _energy = 0;
            }
            else
            {
                _energy = v;
            }
            EventManager.Global.Send(new Energy());
        }
    } 
    public class Assassins:Role,IRole
    {
        public Assassins(IRow data,Sprite s,Image img,Slider slider,TextMeshProUGUI textMeshPro,TextMeshProUGUI deftext,Image defimg):base(data,s,img,slider,textMeshPro,deftext,defimg)
        {
            
        }
    }  
    public class Wizard:Role,IRole
    {
        public Wizard(IRow data,Sprite s,Image img,Slider slider,TextMeshProUGUI textMeshPro,TextMeshProUGUI deftext,Image defimg):base(data,s,img,slider,textMeshPro,deftext,defimg)
        {
            
        }
    }  
    public class Warrior:Role,IRole
    {
        public Warrior(IRow data,Sprite s,Image img,Slider slider,TextMeshProUGUI textMeshPro,TextMeshProUGUI deftext,Image defimg):base(data,s,img,slider,textMeshPro,deftext,defimg)
        {
            
        }
    }  
    
}