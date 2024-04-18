using System.Collections;
using BFramework.UI;
using UnityEngine;

namespace BFramework
{
    public class BattleFsm : MonoBehaviour
    {
        public enum States
        {
            My,
            Enemy,
            Over
        }

        public IBattleModel battleModel;
        public static bool isWin;
        public FSM<States> FSM = new FSM<States>();
        // Start is called before the first frame update
        void Start()
        {
            battleModel = App.Interface.GetModel<IBattleModel>("BattleModel");
            FSM.State(States.My)
                .OnCondition(() => FSM.CurrentStateId == States.Enemy)
                .OnEnter(() =>
                {
                    BattleModel.CanTouch = true;
                    var role =  battleModel.GetRole();
                    role?.SetEnergy(Role._Maxenergy);

                    EventManager.Global.Send(new AddCard(battleModel.GetTakeCardNum()));
                    Debug.Log("my turn ---------");
                })
                .OnUpdate(() =>
                {
                    if (BattleOver())
                    {
                        return;
                    }
                    if (battleModel.GetEndTurn())
                    {
                        FSM.ChangeState(States.Enemy);
                        battleModel.SetEndTurn(false);
                    }
                })
                .OnExit(() =>
                {
                    BattleModel.CanTouch = false;
                   
                });
            FSM.State(States.Enemy)
                .OnCondition(() => FSM.CurrentStateId == States.My) .OnEnter(() =>
                {
                    Debug.Log("Enemy turn ---------");
                    var ms = battleModel.GetMonsters();
                    foreach (var kv in ms)
                    {
                        var m = kv.Value;
                        m.isOver = false;
                    }
                    StartCoroutine(MonsterTurn());

                }).OnUpdate(() =>
                {
                    if (BattleOver())
                    {
                        return;
                    }
                
                    var Next = true;
                    var ms = battleModel.GetMonsters();
                    foreach (var kv in ms)
                    {
                        var m = kv.Value;
                        if (m.isOver == false)
                        {
                            Next = false;
                        }
                    }
                    if (Next)
                    {
                        FSM.ChangeState(States.My);
                    }
                   
                })
                .OnFixedUpdate(() =>
                {

                }).OnExit(() =>
                {
                   

                });
            FSM.State(States.Over).OnCondition(() => FSM.CurrentStateId != States.Over).OnEnter(() =>
            {
                EventManager.Global.Send(new UpdateLevel(battleModel.GetLevel(),battleModel.GetRoomId()));
                PanelManager.Instance.ShowPanel<WinPanel>(nextHide:false);
              
            }).OnExit(() =>
            {
                   

            });
                

            FSM.StartState(States.My);
        }

        public IEnumerator MonsterTurn()
        {
            foreach (var kv in battleModel.GetMonsters())
            {
                var m = kv.Value;
                m.Attack();
                yield return new WaitUntil(() => m.isOver);
            }
        }
        private void Update()
        {
            FSM.Update();
        }

        private bool BattleOver()
        {
            var role = battleModel.GetRole();
            if (role!=null)
            {
                if (role.GetCurHp()<=0)
                {
                    isWin = false;
                    FSM.ChangeState(States.Over);
                    Debug.Log("lost");
                    return true;
                }
            }

            var ms = battleModel.GetMonsters();
            if (ms.Count == 0)
            {
                isWin = true;
                FSM.ChangeState(States.Over);
                Debug.Log("win");
                return true;
            }

            return false;
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