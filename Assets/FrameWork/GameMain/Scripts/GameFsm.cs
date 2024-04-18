using System.Collections;
using System.Collections.Generic;
using BFramework.UI;
using UnityEngine;

namespace BFramework
{
    public class GameFsm : MonoBehaviour
    {
         public enum States
        {
            Battle,
            Home,
            Level,
            Rest,
            Store
        }

        public IBattleModel battleModel;
        public FSM<States> FSM = new FSM<States>();
        void Start()
        {
            EventManager.Global.Register<OverBattle>(BattleOver);
            EventManager.Global.Register<StartBattle>(BattleStart);
            battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            FSM.State(States.Battle)
                .OnCondition(() => FSM.CurrentStateId != States.Battle)
                .OnEnter(() =>
                {
                    PanelManager.Instance.ShowPanel<BattlePanel>(nextHide:false,callback:(() =>
                    {
                        //PanelManager.Instance.HideAllPanel();
                    }));
                })
                .OnUpdate(() =>
                {
                  
                })
                .OnExit(() =>
                {
                    battleModel.LevelClear();
                    //PanelManager.Instance.HidePanelIme<BattlePanel>();
                });
            FSM.State(States.Home)
                .OnCondition(() => FSM.CurrentStateId != States.Home) .OnEnter(() =>
                {
                    battleModel.Clear();
                    //PanelManager.Instance.HidePanel<LevelPanel>();
                   PanelManager.Instance.ShowPanel<StartPanel>(nextHide:false,callback:(() =>  PanelManager.Instance.HideAllPanel()));
                }).OnUpdate(() =>
                {
                   
                   
                   
                })
                .OnFixedUpdate(() =>
                {

                }).OnExit(() =>
                {
                   

                });
            FSM.State(States.Level)
                .OnCondition(() => FSM.CurrentStateId != States.Level) .OnEnter(() =>
                {
                    PanelManager.Instance.HidePanelIme<BattlePanel>();
                    
                }).OnUpdate(() =>
                {
                    
                })
                .OnFixedUpdate(() =>
                {

                }).OnExit(() =>
                {
                   

                });
            FSM.StartState(States.Home);
        }

       
        private void Update()
        {
            FSM.Update();
        }
        
        private void BattleOver(OverBattle e)
        {
           
            if (e.isWin)
            {
                FSM.ChangeState(States.Level);
            }
            else
            {
                FSM.ChangeState(States.Home);
            }
        }
        private void BattleStart(StartBattle e)
        {
            FSM.ChangeState(States.Battle);
        }
        private void FixedUpdate()
        {
            FSM.FixedUpdate();
        }
        
        private void OnDestroy()
        {
            FSM.Clear();
        }
    } 
}

