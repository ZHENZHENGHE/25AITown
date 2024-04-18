using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace BFramework.UI
{
    #region panelconfig
    public enum PanelStyleEnum
    {
        FIX_SIZE,
        FULL_SCREEN,
    }

    public class PanelConfig
    {
        // Package name
        public string pkgName;

        // Resource name
        public List<string> resNames = new List<string>();

        // Panel name
        public string panel;
        
        public string path;

        // 加载依赖的UI资源
        public string[] depends;

        // 窗口是否全屏显示
        public PanelStyleEnum style = PanelStyleEnum.FIX_SIZE;

        // 背景图片资源路径，动态加载的，只有在style为FULL_SCREEN的情况下才会有作用
        public string bgUrl;
        
        public bool notUseUninstall = true;
        
        public UILayerEnum layer;

    }
    

    #endregion
    public class PanelCore :MonoBehaviour
    {
        public PanelConfig config { get; protected set; }

        /// <summary>
        /// 红点事件数据
        /// </summary>
        private List<string> _reminders;
        
        public void AddEvent<T>(Action<T> call)
        {
            EventManager.Global.Register<T>(call);
        }

        public void AddEvent<T, U>(Action<T, U> call)
        {
            EventManager.Global.Register<T,U>(call);
        }
        
        public void RemoveEvent<T>(Action<T> call)
        {
            EventManager.Global.UnRegister(call);
        }
 
      
       
    } 
}

