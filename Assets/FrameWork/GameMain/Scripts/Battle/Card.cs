using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BFramework
{
    public interface ICard
    {
        
     
    } 
    public class Card : MonoBehaviour,ICard,IPointerExitHandler,IPointerEnterHandler,IDragHandler,IDropHandler
    {
        private Button btn;
        public int debug;
        private int _hierarchy;
        private float _tempNum;
        public Canvas canvas;
        private Vector2 orginPos;
        private bool canDrag;
        private int monsterId;
        private int _energy;
        public  int id;
        private TweenerCore<Vector3,Vector3,VectorOptions> TweenI;
        private TweenerCore<Vector3,Vector3,VectorOptions> TweenE;
        private int value;
        private Action _attack;
        public cardRow CardRow;
        public IBattleModel battleModel;
        public TextMeshProUGUI des;
        void Awake()
        {
            btn = transform.GetComponent<Button>();
            canvas = transform.gameObject.AddComponent<Canvas>();
            transform.gameObject.AddComponent<GraphicRaycaster>();
            orginPos = new Vector2(0, 0);//transform.gameObject.GetComponent<RectTransform>().anchoredPosition;
            canDrag = true;
            _tempNum = transform.localPosition.y;
            battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
         
            if (canDrag)
            {
                TweenE.Kill();
                TweenE = transform.DOLocalMoveY(_tempNum, 0.2f);
                TweenE.SetEase(Ease.OutCirc);
                transform.DOScale(1f, 0.1f).SetEase(Ease.OutCirc);
                canvas.overrideSorting = false;
                canvas.sortingLayerName = "Default";
                canDrag = false;
                transform.GetComponent<RectTransform>().anchoredPosition = orginPos;
            }
            
           
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "CardShow";
            TweenI.Kill();
            TweenI = transform.DOLocalMoveY(_tempNum + 100, 0.2f);
            TweenI.SetEase(Ease.InCirc);
            transform.DOScale(1.2f, 0.1f).SetEase(Ease.InCirc);
            canDrag = true; 
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                if (Input.mousePosition.y<=980 && Input.mousePosition.y>=100 && Input.mousePosition.x<= 1820 &&Input.mousePosition.x>=100)
                {
                    transform.gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
                }
                else
                {
                    transform.gameObject.GetComponent<RectTransform>().anchoredPosition = orginPos;
                }
            }
         
        }

        private bool IsWithinDropZone(Vector2 position)
        {
            if (CardRow.needgoal == 0)
            {
                return true;
            }
            var rs = TransformUtilty.find(transform.parent.parent.parent, "monsters").GetComponentsInChildren<Image>().Length;
            for (int i = 0; i < rs; i++)
            { 
                Vector2 dropZoneMin =  new Vector2(980+105+i*300 - 150,540 - 200);
                Vector2 dropZoneMax = new Vector2(980+105+i*300 + 150,540 + 200);
                if ( position.x >= dropZoneMin.x && position.x <= dropZoneMax.x && position.y >= dropZoneMin.y && position.y <= dropZoneMax.y)
                {
                    monsterId = i;
                    return true;
                }
            }
           
           
            return false;
        }
        public void OnDrop(PointerEventData eventData)
        {
            if (BattleModel.CanTouch&&IsWithinDropZone(eventData.position))
            {
                if ( battleModel.GetRole().GetEnergy() - _energy>=0)
                {
                    if (CardRow.needgoal == 0||battleModel.GetMonsters().TryGetValue(monsterId, out var m) )
                    {
                        battleModel.GetRole().SetEnergy(battleModel.GetRole().GetEnergy() - _energy);
                        GameObject o;
                        Choose(CardRow,id);
                        battleModel.AddCardToDead(this);
                        transform.DOMove(new Vector3(1920,0), 0.4f).SetEase(Ease.InCirc);
                        transform.DOScale(0.1f, 0.4f).SetEase(Ease.InCirc).onComplete=() =>
                        {
                            (o = transform.parent.gameObject).SetActive(false);
                            Destroy(o); 
                        };
                    }
                 
                }
                else
                {
                    transform.GetComponent<RectTransform>().anchoredPosition = orginPos;
                }
             
                // if (battleModel.GetHandCard().Count<=0)//临时
                // {
                //     AddCardCommand.AddNum = 5;
                //     App.Interface.SendCommand("AddCardCommand");
                // }
            }
            else
            {
                transform.GetComponent<RectTransform>().anchoredPosition = orginPos;
            }
    
        }

        public void Init(cardRow cardData,int key)
        {
            id = key;
            CardRow = cardData;
            value = cardData.value;
            des.text = cardData.des;
            _energy = cardData.energy;
        }
        public void Choose(cardRow cardData,int id)
        {
            if (cardData.type == 1)
            {
                NormalAttackCommand.Attack = cardData.value+300;
                NormalAttackCommand.MonsterId = monsterId;
                App.Interface.SendCommand("NormalAttackCommand");
            }
            else if (cardData.type == 2)
            {
                NormalDefCommand.Def = cardData.value+100;
                App.Interface.SendCommand("NormalDefCommand");
            }
            else
            {
                
                switch (id)
                {
                    case 9 :
                    case 4 :
                        NormalAttackCommand.Attack = cardData.value;
                        NormalAttackCommand.MonsterId = monsterId;
                        App.Interface.SendCommand("NormalAttackCommand");
                        HelpCommand.Help = cardData.value;
                        App.Interface.SendCommand("HelpCommand");
                        break;
                    case 11:
                        NormalAttackCommand.Attack = cardData.value;
                        NormalAttackCommand.MonsterId = monsterId;
                        App.Interface.SendCommand("NormalAttackCommand");
                        AttackRoleCommand.Attack = 3;
                        App.Interface.SendCommand("AttackRoleCommand");
                        break;
                    case 3:
                        AddCardCommand.AddNum = cardData.value;
                        App.Interface.SendCommand("AddCardCommand");
                        break;
                    case 10: 
                    case 13:
                        AllAttackCommand.Attack = cardData.value;
                        App.Interface.SendCommand("AllAttackCommand");
                        break;
                    case 12: 
                        AttackByHPCommand.MonsterId = monsterId;
                        App.Interface.SendCommand("AttackByHPCommand");
                        break;
                }
            }
           
        }
    } 
}