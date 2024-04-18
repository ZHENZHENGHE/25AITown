using System.Collections;
using System.Collections.Generic;
using BFramework;
using UnityEngine;

public class App : Framework<App>
{
    protected override void Init()
    {
       
    }
    protected override void ExecuteCommand(string name)
    {
        Debug.Log("Before " + name + "Execute");
        base.ExecuteCommand(name);
        Debug.Log("After " + name + "Execute");
    }

    protected override TResult ExecuteCommand<TResult>(string name)
    {
        Debug.Log("Before " + name + "Execute");
        var result =  base.ExecuteCommand<TResult>(name);
        Debug.Log("After " + name + "Execute");
        return result;
    }
    protected override void ExecutePCommand(string name)
    {
        Debug.Log("Before " + name + "Execute");
        base.ExecuteCommand(name);
        Debug.Log("After " + name + "Execute");
    }

    protected override T ExecutePCommand<T,U,K>(string name)
    {
        Debug.Log("Before " + name + "Execute");
        var result =  base.ExecutePCommand<T,U,K>(name);
        Debug.Log("After " + name + "Execute");
        return result;
    }
}
