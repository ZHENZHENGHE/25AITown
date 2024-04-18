using System;
using System.Collections.Generic;
using BFramework.UI;
using UnityEngine;


namespace BFramework
{
    #region Framework

    public interface IFramework
    {
        
        T GetSystem<T>(string name) where T : class, ISystem;
        
        T GetModel<T>(string name) where T : class, IModel;
        
        T GetUtility<T>(string name) where T : class, IUtility;
        
        public T SendPCommand<T,U,K>(string name);
        
        public void SendPCommand(string name);
        void SendCommand(string name);
   
        TResult SendCommand<TResult>(string name);
        
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);
        
        void RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    public abstract class Framework<T> : IFramework where T : Framework<T>, new()
    {
        
        private IOCContainer mContainer = GameStart.mContainer;
        private EventManager mEventManager= new EventManager();

        // public static Action<T> OnRegisterPatch = architecture => { };

        protected static T mFramework;

        public static IFramework Interface
        {
            get
            {
                if (mFramework == null)
                {
                    MakeSureInit();
                }

                return mFramework;
            }
        }


        static void MakeSureInit()
        {
            if (mFramework == null)
            {
                mFramework = new T();
                mFramework.Init();

                //OnRegisterPatch?.Invoke(mArchitecture);
            }
        }

        protected abstract void Init();


        public TSystem GetSystem<TSystem>(string name) where TSystem : class, ISystem => mContainer.Resolve<TSystem>(name);
     
			
        public TModel GetModel<TModel>(string name) where TModel : class, IModel =>mContainer.Resolve<TModel>(name);
    
        
        public TUtility GetUtility<TUtility>(string name) where TUtility : class, IUtility => mContainer.Resolve<TUtility>(name);
        public TResult SendCommand<TResult>(string name) => ExecuteCommand<TResult>(name);
        
        public void SendCommand(string name) => ExecuteCommand(name);
        protected virtual void ExecuteCommand(string name) =>mContainer.Resolve<ICommand>(name).Execute();
        protected virtual TResult ExecuteCommand<TResult>(string name) =>mContainer.Resolve<ICommand<TResult>>(name).Execute();
        
        
        public A SendPCommand<A,B,C>(string name) => ExecutePCommand<A,B,C>(name);
        
        public void SendPCommand(string name) => ExecutePCommand(name);
        protected virtual void ExecutePCommand(string name) =>mContainer.Resolve<IProcedureCommand>(name).Execute();
        protected virtual A ExecutePCommand<A,B,C>(string name) =>mContainer.Resolve<IProcedureCommand<A,B,C>>(name).Execute();
        
       
        
        public void SendEvent<TEvent>() where TEvent : new() => mEventManager.Send<TEvent>();
        
        public void SendEvent<TEvent>(TEvent e) => mEventManager.Send<TEvent>(e);
        
        public void RegisterEvent<TEvent>(Action<TEvent> onEvent) => mEventManager.Register<TEvent>(onEvent);
        
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent) => mEventManager.UnRegister<TEvent>(onEvent);
        
    }
    
    #endregion
    
    #region Command

    public interface ICommand 
    {
        void Execute();
    }
  
    public interface ICommand<TResult> 
    {
        TResult Execute();
    }
    
    public interface IProcedureCommand:ICommand
    {
        new void Execute();
        void BeforeExecute();
        void AfterExecute();
    }
    public interface IProcedureCommand<T,U,K>:ICommand<T> 
    {
        new T Execute();
        U BeforeExecute();
        K AfterExecute();
    }

    public abstract class AbstractCommand : ICommand
    {
        public IFramework frame = App.Interface;
        void ICommand.Execute() => OnExecute();

        protected abstract void OnExecute();
    }

    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        public IFramework frame = App.Interface;
        TResult ICommand<TResult>.Execute() => OnExecute();

        protected abstract TResult OnExecute();
    }
    public abstract class AbstractPCommand : IProcedureCommand
    {
        public IFramework frame = App.Interface;
        void IProcedureCommand.Execute()
        {
            OnBeforeExecute();
            OnExecute();
            OnAfterExecute();
        }
        void IProcedureCommand.BeforeExecute() => OnBeforeExecute();
        void IProcedureCommand.AfterExecute() => OnAfterExecute();
        void ICommand.Execute() =>OnExecute();
        protected abstract void OnExecute();
        protected abstract void OnBeforeExecute();
        protected abstract void OnAfterExecute();

       
     
    }

    public abstract class AbstractPCommand<T,U,K> : IProcedureCommand<T,U,K>
    {
        public IFramework frame = App.Interface;
        T IProcedureCommand<T,U,K>.Execute()
        {
            var A =OnBeforeExecute();
            var B = OnExecute();
            var C =OnAfterExecute();
            return OnResult(B, A, C);
        }
        T ICommand<T>.Execute() => OnExecute();
        U IProcedureCommand<T,U,K>.BeforeExecute()=> OnBeforeExecute();
        K IProcedureCommand<T,U,K>.AfterExecute()=> OnAfterExecute();
        protected abstract T OnExecute();
        protected abstract U OnBeforeExecute();
        protected abstract K OnAfterExecute();
        protected abstract T OnResult(T a, U b, K c);

    }

    #endregion
    
    #region BindableProperty

    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        new T Value { get; set; }
        void SetValueWithoutEvent(T newValue);
    }

    public interface IReadonlyBindableProperty<T> : IEvent
    {
        T Value { get; }

        void RegisterWithInitValue(Action<T> action);
        void UnRegister(Action<T> onValueChanged);
        void Register(Action<T> onValueChanged);
    }

    public class BindableProperty<T> : IBindableProperty<T>
    {
        public BindableProperty(T defaultValue = default) => mValue = defaultValue;
        
        public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

        public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
        {
            Comparer = comparer;
            return this;
        }
        protected T mValue;
        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && Comparer(value, mValue)) return;

                SetValue(value);
                mOnValueChanged?.Invoke(value);
            }
        }

        protected virtual void SetValue(T newValue) => mValue = newValue;

        protected virtual T GetValue() => mValue;

        public void SetValueWithoutEvent(T newValue) => mValue = newValue;

        private Action<T> mOnValueChanged = (v) => { };

        public void Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
        }

        public void RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            Register(onValueChanged);
        }

        public void UnRegister(Action<T> onValueChanged) => mOnValueChanged -= onValueChanged;

        void IEvent.Register(Action onEvent)
        {
            void Action(T _) => onEvent();
        }
		
        public override string ToString() => Value.ToString();
    }
   

    #endregion
    
    #region Model

    public interface IModel
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        public IFramework frame = App.Interface;
        void IModel.Init() => OnInit();

        protected abstract void OnInit();
    }

    #endregion

    #region Utility

    public interface IUtility
    {
    }

    #endregion
    
    #region System

    public interface ISystem 
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        public IFramework frame = App.Interface;
        void ISystem.Init() => OnInit();

        protected abstract void OnInit();
    }

    #endregion
    
    #region Manager

    public interface IManager
    {
        void Init();
    }

    public abstract class AbstractManager : IManager
    {
        public IFramework frame = App.Interface;
        void IManager.Init() => OnInit();

        protected abstract void OnInit();
    }

    #endregion
  
    #region ECS

    public interface IComponent
    {
        void Init();
    }
    public interface IESystem
    {
        void Init();
    }
    public interface IEntity
    {
        void Init();
        void AddComponent(IComponent c);
        void RemoveComponent(Type t);
        IComponent GetComponent(Type t);
        bool HasComponent(Type t) ;
    }
    public abstract class AbstractESystem : MonoBehaviour,IESystem
    {
        public IFramework frame = App.Interface;
        public int id;
        void IESystem.Init() => OnInit();

        protected abstract void OnInit();
    }
    public abstract class AbstractComponent : MonoBehaviour,IComponent
    {
        public IFramework frame = App.Interface;
        public int id;
        void IComponent.Init() => OnInit();

        protected abstract void OnInit();
    }
    public abstract class AbstractIEntry: MonoBehaviour,IEntity
    {
        public IFramework Frame = App.Interface;
        public int id;
        public Dictionary<Type,IComponent> components = new Dictionary<Type,IComponent>();
        void IEntity.Init() => OnInit();
        public void AddComponent(IComponent c) 
        {
            components.Add(c.GetType(),c);
        }

        public void RemoveComponent(Type t) 
        {
            components.Remove(t);
        }

        public new IComponent GetComponent(Type t)
        {
            if (components.TryGetValue(t,out var c))
            {
                return c; 
            }
            return null;
        }

        public bool HasComponent(Type t) 
        {
            return components.ContainsKey(t);
        }

        protected abstract void OnInit();
    }
    #endregion
 
}