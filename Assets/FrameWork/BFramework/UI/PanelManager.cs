using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BFramework.UI
{
    

public class PanelManager : MonoBehaviour,IManager
{
    public static PanelManager Instance;
    // 上一次检测Panel释放的时间，一次检测，只会销毁一个Panel
    private float _lastCheckUninstallTime;
    private static float PANEL_DISPOSE_TIME = 2f;
    private int _resNum;
    private bool _numlock;
    // 每个几帧检测一次
    private static int PANEL_CHECK_DISPOSE_FRAME = 15;
    private Dictionary<Type, PanelBase> _panelDic = new Dictionary<Type, PanelBase>();
    private Dictionary<string, PanelBase> _stringPanelDic = new Dictionary<string, PanelBase>();
    private Dictionary<Type, GameObject> _releasePanelDic = new Dictionary<Type, GameObject>();
    private Stack<PanelBase> _backPanel = new Stack<PanelBase>();
    private GameObject _canvas;
    public void Init()
    {
      
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
    }
    private void Start()
    {
        _canvas = GetComponent<Canvas>().gameObject;
    }

    private void Update()
    {
        if (Time.frameCount % PANEL_CHECK_DISPOSE_FRAME == 0)
        {
            CheckUninstall();
        }
    }

   
    public async void ShowPanel(Type t,bool isAdd = true,bool nextHide = true,Action callback = null)
    {
        if (_backPanel.Count!=0)
        {
            if (nextHide&&_backPanel.TryPop(out var panelBase))
            {
                HidePanel(panelBase.GetType());
            }
            
        }
        if (t == null)
        {
            LogKit.E("无效类型窗口");
        }
        if (_panelDic.TryGetValue(t,out var panel))
        {
            _backPanel.Push(panel);
            if (panel.PanelState == PanelState.HIDE_OVER)
            {
                panel.Show();
                callback?.Invoke();
            }
        }
        else
        {
            if (_numlock)
            {
                return;
            }
            _numlock = true;
            GameObject prefabObj = await Addressables.LoadAssetAsync<GameObject>(Resname.UI(t.Name)).Task;
            _releasePanelDic.Add(t,prefabObj);
      
            var obj = Instantiate(prefabObj, _canvas.transform, false);
            PanelBase panelObj;
            if (isAdd)
            {
                panelObj  = (PanelBase)obj.AddComponent(t);
              
            }
            else
            {
                panelObj = obj.GetComponent<PanelBase>();
            }
            _backPanel.Push(panelObj);
            LoadAndShow(panelObj,callback);
        }
       
    }

    private void LoadAndShow(PanelBase panelObj,Action callback)
    {
        panelObj.View = panelObj.transform;
        panelObj.PanelState = PanelState.SHOW_START;
        panelObj.LoadConfig();
        _resNum = panelObj.config.resNames.Count;
       
        _stringPanelDic.Add(panelObj.config.panel,panelObj);
        LoadRes(panelObj);
        panelObj.gameObject.SetActive(false);
        StartCoroutine(LoadComplete(panelObj,callback));
    }
  
    private void LoadRes(PanelBase panel)
    {
        foreach (var res in panel.config.resNames)
        {
            Addressables.LoadAssetAsync<Sprite>(res).Completed+= (handle) =>
            {
                if (!panel.Res.TryGetValue(res,out var sprite))
                {
                    panel.Res.Add(res,handle.Result);
                }
                _resNum--;
            };
        }
        
    }

    public IEnumerator LoadComplete(PanelBase panel,Action callback)
    {
        while (_resNum>0)
        {
            yield return null;
        }
        _numlock = false;
        
        panel.Init();
        callback?.Invoke();
        _panelDic.Add(panel.GetType(),panel);
        yield return null;
      
    }
    public void HidePanel(Type t)
    {
        if (_panelDic.TryGetValue(t,out var panel))
        {
            panel.PanelState = PanelState.HIDE_START;
            panel.Hide();
            panel.PanelState = PanelState.HIDE_OVER;
        }
        //todo 添加待删除队列
    }
    public void HidePanelIme(Type t)
    {
        if (_panelDic.TryGetValue(t,out var panel))
        {
            panel.HideIme();
            _panelDic.Remove(panel.GetType());
            _stringPanelDic.Remove(panel.config.panel);
            DestroyImmediate(panel.gameObject);
            Addressables.Release(_releasePanelDic[panel.GetType()]);
            _releasePanelDic.Remove(panel.GetType());
        }
    }
    public void Back()
    {
        if (_backPanel.TryPop(out var panel))
        {
            HidePanel(panel.GetType());
        }
        if (_backPanel.TryPop(out var panel2))
        {
            ShowPanel(panel2.GetType());
        }
        
    }
    public void HidePanel<T>() where T : PanelBase
    {
        HidePanel(typeof(T));
    }
    public void HidePanelIme<T>() where T : PanelBase
    {
        HidePanelIme(typeof(T));
    }
    public void ShowPanel<T>(bool isAdd = true,bool nextHide = true,Action callback = null) where T : PanelBase
    {
        ShowPanel(typeof(T),isAdd,nextHide,callback);
    }
    public T GetPanel<T>() where T : PanelBase
    {
        if (_panelDic.TryGetValue(typeof(T), out var panel))
        {
            return (T) panel;
        }

        return null;
    }

    public PanelBase GetPanelByType(Type type)
    {
        return _panelDic.TryGetValue(type, out var panel) ? panel : null;
    }
    public PanelBase GetPanelByName(string panelName)
    {
        return _stringPanelDic.TryGetValue(panelName, out var panel) ? panel : null;
    }
    public bool HasPanel(Type type)
    {
        foreach (var panel in _panelDic.Values)
        {
            if (panel == null) continue;
            if (panel.GetType() == type) return true;
        }
        return false;
    }
    public void HideAllPanel()
    {
        foreach (var panel in _panelDic.Values)
        {
            HidePanel(panel.GetType());
        }
    }
    public void HideAllPanelIme()
    {
        foreach (var panel in _panelDic.Values)
        {
            HidePanelIme(panel.GetType());
        }
    }
    private void CheckUninstall()
    {
        if (_panelDic.Count <= 0) return;
        var deltaTime = Time.fixedTime - _lastCheckUninstallTime;
        _lastCheckUninstallTime = Time.fixedTime;
        PanelBase destroyPanel = null;
        foreach (var panel in _panelDic.Values)
        {
            if (panel.PanelState != PanelState.HIDE_OVER) continue;

            if (panel.config.notUseUninstall == false) continue;
            panel.hideTime += deltaTime;
            if (!(panel.hideTime > PANEL_DISPOSE_TIME)) continue;
            destroyPanel = panel;
            break;
        }
        if (null == destroyPanel) return;
        
        _panelDic.Remove(destroyPanel.GetType());
        _stringPanelDic.Remove(destroyPanel.config.panel);
        DestroyImmediate(destroyPanel.gameObject);
        Addressables.Release(_releasePanelDic[destroyPanel.GetType()]);
        _releasePanelDic.Remove(destroyPanel.GetType());
       
    }
}
}