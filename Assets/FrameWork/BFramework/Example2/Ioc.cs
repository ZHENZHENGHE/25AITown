using UnityEngine;

namespace BFramework.Example
{
   
        
    public interface IAnimal
    {
        void Say();
    }
    public interface ICar
    {
        void Say();
    }
    [Controller("Cat", Lifetime.Singleton)]
    public class Cat : IAnimal
    {
        public Cat()
        {
            throw new System.NotImplementedException();
        }

        public void Say()
        {
            Debug.Log("miao" + GetHashCode());
        }
    }
    [Controller("Dog",Lifetime.Transient)]
    public class Dog : IAnimal
    {
        public void Say()
        {
            Debug.Log("woof"+GetHashCode());
        }
    }
    [Controller]
    public class Bird : IAnimal
    {
        public void Say()
        {
            Debug.Log("zhi"+GetHashCode());
        }
    }
    [Controller( lifetime:Lifetime.Singleton)]
    public class Lion : IAnimal
    {
        public void Say()
        {
            Debug.Log("ooo"+GetHashCode());
        }
    }
    [Model("Dar",Lifetime.Singleton)]
    public class Dar : ICar
    {
        public void Say()
        {
            Debug.Log("DDDD"+GetHashCode());
        }
    }
    [Controller("animal")]
    public class Animal:IAnimal
    {
        
        [Inject("Cat")]
        private IAnimal c;
        [Inject("Dar")]
        private ICar d;
        [Inject("Dog")]
        private IAnimal e;
        public void Say()
        {
            Debug.Log("Say Hi"+GetHashCode());
            c.Say();
            d.Say();
            e.Say();
           
        }
    }
    public class Ioc : MonoBehaviour
    {
        private void Start()
        {
            var container = new IOCContainer();
            container.ScanAndRegisterFromAssembly();
            
            // var animal = container.Resolve<IAnimal>("Dog");
            // animal.Say();
            // animal= container.Resolve<IAnimal>("Cat");
            // animal.Say();
            // animal= container.Resolve<IAnimal>("Cat");
            // animal.Say();
            // animal= container.Resolve<IAnimal>();
            // animal.Say();
            // var a= container.Resolve<ICar>("Dar");
            // a.Say();
            var animal= container.Resolve<IAnimal>("animal");
            animal.Say();
            // animal= container.Resolve<IAnimal>("animal");
            // animal.Say();
        }
    }
}