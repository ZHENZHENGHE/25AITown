
using System;
using BFramework;

public struct UISHOW
{
    public int age;
}
public struct ATTACK
{
    public int attack;
    public int monster;
}
public struct Hp
{
}
public struct Def
{
    
}
public struct Money
{
}
public struct Energy
{
}
public struct OverBattle
{
    public bool isWin;
    public OverBattle(bool b)
    {
        isWin = b;
    }
}
public struct AddCard
{
    public int AddNum;
    public AddCard(int n)
    {
        AddNum = n;
    }
}
public struct UpdateCardText
{
}
public struct StartBattle
{
}
public struct UpdateLevel
{
    public int level;
    public int value;
    public UpdateLevel(int l,int v)
    {
        level = l;
        value = v;
    }
}