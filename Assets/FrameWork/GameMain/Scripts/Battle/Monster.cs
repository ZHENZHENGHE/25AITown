using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BFramework
{
    public interface IMonster
    {
 
    }
    public class Monster:IMonster
    {
        public int attack;
        public string name;
        public int def;
        public int hp;
        public int cur_hp;
        public string skill;
        public Image img;
        public Slider slider;
        public TextMeshProUGUI tex;
        public bool isOver = true;
        public Monster(IRow data,Sprite s,Image img,Slider slider,TextMeshProUGUI textMeshPro)
        {
            this.slider = slider;
            this.img = img;
            this.img.sprite = s;
            tex = textMeshPro;
            if (data is monsterRow monster)
            {
                name = monster.name;
                attack = monster.attack;
                def = monster.def;
                hp = monster.hp;
                cur_hp = hp;
                tex.text = cur_hp + "/" + hp;
                UpdateHp(0);
            }
        }

        public void UpdateHp(int value)
        {
            cur_hp += value;
            tex.text = cur_hp + "/" + hp;
            slider.value = cur_hp* 100 / hp ;
        }

        public void Attack()
        {
            
            img.transform.DOLocalMove(new Vector3(-300, 0), 0.5f).SetEase(Ease.InFlash).onComplete=(() =>
            {
                AttackRoleCommand.Attack = attack;
                App.Interface.SendCommand("AttackRoleCommand");
                img.transform.DOLocalMove(new Vector3(0, 0), 0.5f).SetEase(Ease.InFlash).onComplete=(() =>
                {
                    isOver = true;
                });
            });
            Timer.Instance.Schedule((() =>
            {
               
            }), 3, 0, 1);
      
        }
    }
}

