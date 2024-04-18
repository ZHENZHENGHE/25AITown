using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BFramework
{
      #region Attribute
    public enum Lifetime
    {
        Transient, // 瞬态模式，每次获取新实例
        Singleton, // 单例模式，全局共享同一个实例
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModelAttribute : Attribute
    {
        public string name { get; }
        public Lifetime lifetime { get; }
        public ModelAttribute(string name = null,Lifetime lifetime = Lifetime.Transient)
        {
            this.lifetime = lifetime;
            this.name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UtilityAttribute : Attribute
    {
        public string name { get; }
        public Lifetime lifetime { get; }
        public UtilityAttribute(string name = null,Lifetime lifetime = Lifetime.Transient)
        {
            this.lifetime = lifetime;
            this.name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public string name { get; }
        public Lifetime lifetime { get; }
        public CommandAttribute(string name = null,Lifetime lifetime = Lifetime.Transient)
        {
            this.lifetime = lifetime;
            this.name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : Attribute
    {
        public string name { get; }
        public Lifetime lifetime { get; }
        public SystemAttribute(string name = null,Lifetime lifetime = Lifetime.Transient)
        {
            this.lifetime = lifetime;
            this.name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ControllerAttribute : Attribute
    {
        public string name { get; }
        public Lifetime lifetime { get; }
        public ControllerAttribute(string name = null,Lifetime lifetime = Lifetime.Transient)
        {
            this.lifetime = lifetime;
            this.name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public string Name { get; }
        public InjectAttribute(string name = null)
        {
            Name = name;
        }
    }


    #endregion
        public class IOCContainer
    {
        private Dictionary<Type, (Type implementationType, Lifetime lifetime)> _container = new Dictionary<Type, (Type, Lifetime)>();
        private Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private Dictionary<string, (Type implementationType, Lifetime lifetime)> _containers = new Dictionary<string, (Type, Lifetime)>();
        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient) where TImplementation : TInterface
        {
            _container[typeof(TInterface)] = (typeof(TImplementation), lifetime);
        }
        public void Register<TInterface, TImplementation>(string name ,Lifetime lifetime = Lifetime.Transient) where TImplementation : TInterface
        {
            _containers[name] = (typeof(TImplementation), lifetime);
        }
      
       
        public TInterface Resolve<TInterface>(string name = null)
        {
            Type implementationType = null;
            Lifetime lifetime = Lifetime.Transient;
            if (string.IsNullOrEmpty(name))
            {
                if (_container.TryGetValue(typeof(TInterface), out var info))
                {
                    implementationType = info.implementationType;
                    lifetime = info.lifetime;
                }
            }
            else
            {
                if (_containers.TryGetValue(name, out var info))
                {
                    implementationType = info.implementationType;
                    lifetime = info.lifetime;
                }
            }

            if (implementationType == null)
            {
                throw new Exception($"No implementation found for {typeof(TInterface)}");
            }
            switch (lifetime)
            {
                case Lifetime.Transient:
                    return (TInterface)CreateInstance(implementationType);

                case Lifetime.Singleton:
                    if (_singletons.TryGetValue(implementationType, out var instance))
                    {
                        return (TInterface)instance;
                    }
                    else
                    {
                        var newSingletonInstance = Activator.CreateInstance(implementationType);
                        _singletons[implementationType] = newSingletonInstance;
                        InvokeInit(implementationType,newSingletonInstance);
                        Inject(implementationType, newSingletonInstance);
                        return (TInterface)newSingletonInstance;
                    }
                    
            }

            throw new Exception($"No implementation found for {typeof(TInterface)}");
        }

        private void Inject(Type implementationType,object newSingletonInstance)
        {
            MemberInfo[] ms = implementationType.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dependencys = ms
                .Where(m => m.GetCustomAttribute<InjectAttribute>() != null);
          
            foreach (var member in dependencys)
            {
                var dependency = Resolve(member);
                if (member is PropertyInfo property)
                {
                    property.SetValue(newSingletonInstance, dependency);
                }
                if (member is FieldInfo field)
                {
                    field.SetValue(newSingletonInstance, dependency);
                }  
            }
        }
        public object Resolve(MemberInfo member)
        {
            var name = member.GetCustomAttribute<InjectAttribute>().Name;
            Type implementationType = null;
            Lifetime lifetime = Lifetime.Transient;
            if (string.IsNullOrEmpty(name))
            {
                // if (member.ReflectedType == null)
                // {
                //     throw new Exception($"No implementation found for {typeof(object)}");
                // }
                // if (_container.TryGetValue(member.ReflectedType, out var info))
                // {
                //     implementationType = info.implementationType;
                //     lifetime = info.lifetime;
                // }
            }
            else
            {
                if (_containers.TryGetValue(name, out var info))
                {
                    implementationType = info.implementationType;
                    lifetime = info.lifetime;
                }
            }

            if (implementationType == null)
            {
                throw new Exception($"No implementation found for {typeof(object)}");
            }
            switch (lifetime)
            {
                case Lifetime.Transient:
                    return CreateInstance(implementationType);

                case Lifetime.Singleton:
                    if (_singletons.TryGetValue(implementationType, out var instance))
                    {
                        return instance;
                    }
                    else
                    {
                        var newSingletonInstance = Activator.CreateInstance(implementationType);
                        InvokeInit(implementationType,newSingletonInstance);
                        _singletons[implementationType] = newSingletonInstance;
                        return newSingletonInstance;
                    }
                    
            }
            throw new Exception($"No implementation found for {typeof(object)}");
        }

        private object CreateInstance(Type type)
        {
            var obj = Activator.CreateInstance(type);
            InvokeInit(type,obj);
            Inject(type, obj);
            return obj;
        }

        private void InvokeInit(Type type,object obj)
        {
            if (type.GetCustomAttribute<ControllerAttribute>()!= null ||type.GetCustomAttribute<ModelAttribute>()!= null||type.GetCustomAttribute<SystemAttribute>()!=null||type.GetCustomAttribute<SystemAttribute>()!=null)
            {
                var m =type.GetMethod("OnInit",BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (m != null)
                {
                    m.Invoke(obj, null);
                }
            }
        }
        public void ScanAndRegisterFromAssembly()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();
            foreach (var type in types)
            {
                var modelAttribute = type.GetCustomAttribute<ModelAttribute>();
                if (modelAttribute != null)
                {
                    Register(type, modelAttribute.lifetime,modelAttribute.name);
                }
                var utilityAttribute = type.GetCustomAttribute<UtilityAttribute>();
                if (utilityAttribute != null)
                {
                    Register(type, utilityAttribute.lifetime,utilityAttribute.name);
                }
                var systemAttribute = type.GetCustomAttribute<SystemAttribute>();
                if (systemAttribute != null)
                {
                    Register(type, systemAttribute.lifetime,systemAttribute.name);
                }
                var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>();
                if (controllerAttribute != null)
                {
                    Register(type, controllerAttribute.lifetime,controllerAttribute.name);
                }
                var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute != null)
                {
                    Register(type, commandAttribute.lifetime,commandAttribute.name);
                }
            }
        }

        public void Register(Type type, Lifetime lifetime,string name)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                Register(@interface, type, lifetime,name);
            }
            Register(type, type, lifetime,name);
        }

        private void Register(Type interfaceType, Type implementationType, Lifetime lifetime,string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _container[interfaceType] = (implementationType, lifetime);
            }
            else
            {
                _containers[name] = (implementationType, lifetime);
            }
        }
    }
    
}

