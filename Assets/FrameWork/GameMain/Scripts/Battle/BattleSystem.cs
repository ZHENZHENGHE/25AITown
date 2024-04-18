

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BFramework
{
    public interface IBattleSystem:ISystem
    {
        public void AllCardToReady();

        public void InitAllCard();

    }
    
    [System("BattleSystem",Lifetime.Singleton)]
    public class BattleSystem:AbstractSystem,IBattleSystem
    {
        [Inject("BattleModel")] 
        private IBattleModel battleModel;

        protected override void OnInit()
        {
            
        }
        public void AllCardToReady()
        {
            battleModel.AllCardToReady();
        }
        public void InitAllCard()
        {
            battleModel.InitAllCard();
        }
        
    }
}

