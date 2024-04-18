using BFramework.UI;
using UnityEngine;

namespace BFramework
{
public class GameStart : MonoBehaviour
{
    public static IOCContainer mContainer = new IOCContainer();
    private static IFramework mApp;
    private void Awake()
    {
        mContainer.ScanAndRegisterFromAssembly();
        Config.Init();
        mApp = App.Interface;
        var c  = mApp.GetSystem<ISystem>("qSystem");
        c.Init();
      
        GameObject newObject = new GameObject("Timer");
       newObject.AddComponent<Timer>();
       newObject.AddComponent<GameFsm>();
    }

    void Start()
    {
        PanelManager.Instance.ShowPanel(typeof(StartPanel));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public interface IqSystem : ISystem
{

}
[Controller("qSystem",Lifetime.Singleton)]
public class qSystem : AbstractSystem,IqSystem
{
    [Inject("aModel")]
    private IaModel a;
    protected override void OnInit()
    {
        Debug.Log(App.Interface.SendCommand<int>("IncreaseCountCommand"));
    }
    // private void Init()
    // {
    //     Debug.Log("qSysteminit"+GetHashCode());
    // }
}
[Command("IncreaseCountCommand")]
public class IncreaseCountCommand : AbstractCommand<int> 
{
    [Inject("aModel")]
    private IaModel a;
    protected override int OnExecute()
    {
        a.Say();
        return 1;
    }
}
public interface IaModel : IModel
{
    void Say();
    
}
[Controller("aModel",Lifetime.Singleton)]
public class aModel : AbstractModel,IaModel
{
   
    // private void Init()
    // {
    //     Debug.Log("aModel"+GetHashCode());
    // }
  
    protected override void OnInit()
    {
        
    }

    public void Say()
    {
        Debug.Log("aModel"+GetHashCode());
    }
}
}