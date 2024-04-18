
namespace BFramework
{
    [Command("NormalAttackCommand",Lifetime.Singleton)]
    public class NormalAttackCommand:AbstractCommand,ICommand
    {
        public static int MonsterId;
        public static int Attack;
        protected override void OnExecute()
        {
            EventManager.Global.Send(new ATTACK()
            {
                attack = Attack,
                monster = MonsterId
            });
        }
    }
    [Command("NormalDefCommand",Lifetime.Singleton)]
    public class NormalDefCommand:AbstractCommand,ICommand
    {
        [Inject("BattleModel")]
        public IBattleModel battleModel;
      
        public static int Def;
        protected override void OnExecute()
        {
            var role = battleModel.GetRole();
            role.SetDef(role.GetDef() + Def);
        }
    }
    [Command("HelpCommand",Lifetime.Singleton)]
    public class HelpCommand:AbstractCommand,ICommand
    {
        [Inject("BattleModel")]
        public IBattleModel battleModel;
      
        public static int Help;
        protected override void OnExecute()
        {
            var role = battleModel.GetRole();
            role.UpdateHp(Help);
        }
    }
    [Command("AddCardCommand",Lifetime.Singleton)]
    public class AddCardCommand:AbstractCommand,ICommand
    {
        public static int AddNum;
        protected override void OnExecute()
        {
            EventManager.Global.Send(new AddCard(AddNum));
        }
    }
    [Command("AllAttackCommand",Lifetime.Singleton)]
    public class AllAttackCommand:AbstractCommand,ICommand
    {
        [Inject("BattleModel")]
        public IBattleModel battleModel;
        public static int Attack;
        
        protected override void OnExecute()
        {
            foreach (var kv in battleModel.GetMonsters())
            {
                EventManager.Global.Send(new ATTACK()
                {
                    attack = Attack,
                    monster = kv.Key
                });
            }
        }
    }
    [Command("AttackByHPCommand",Lifetime.Singleton)]
    public class AttackByHPCommand:AbstractCommand,ICommand
    {
        [Inject("BattleModel")]
        public IBattleModel battleModel;
        public static int MonsterId;
        
        protected override void OnExecute()
        {
            EventManager.Global.Send(new ATTACK()
            {
                attack = battleModel.GetRole().GetCurHp()/4,
                monster = MonsterId
            });
        }
    }
    [Command("AttackRoleCommand",Lifetime.Singleton)]
    public class AttackRoleCommand:AbstractCommand,ICommand
    {
        [Inject("BattleModel")]
        public IBattleModel battleModel;
        
        public static int Attack;
        protected override void OnExecute()
        {
            var role = battleModel.GetRole();
            if (Attack>=role.GetDef())
            {
                Attack -= role.GetDef();
                role.SetDef(0);
                role.UpdateHp(-Attack);
            }
            else
            {
                role.SetDef(role.GetDef()-Attack);
            }
      
          
        }
    }
}

