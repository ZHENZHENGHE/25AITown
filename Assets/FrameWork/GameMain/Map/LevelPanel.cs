using System;
using System.Collections.Generic;
using BFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace BFramework.UI
{
    

public class LevelPanel: PanelBase
{
    [SerializeField] int maxLevel = 0;
    [SerializeField] int maxCount = 0;
    public int iSeed = 1;
    private List<GameObject> lines = new List<GameObject>();
    public Random random;
    [SerializeField] private Material lineMaterial;
    MapNode[][] mapNodes;
    [SerializeField] EveryLevel[] levels;
    [SerializeField] Image born;
    [SerializeField] Image boss;
    private void Awake()
    {
        //加载图片资源
    }
    protected override void OnConfig()
    {
        config = new PanelConfig()
        {
            panel = Resname.UI("LevelPanel"),
            resNames = {Resname.Texture("store"),Resname.Texture("box"),Resname.Texture("boss"),Resname.Texture("fire"),Resname.Texture("normal_monster"),Resname.Texture("wenhao")}
        };
    }

    protected override void OnInit()
    {
       AddEvent<UpdateLevel>(Pass);
    }

    protected override void OnHide()
    {
        RemoveEvent<UpdateLevel>(Pass);
    }

    protected override void OnShow()
    {
        Generate();
    }

    public void  Generate()
    {
        Clear();
        init();
        CreateMap();
        ShowRooms();
        SetLevelsPosition();
        ShowLine();
        foreach (var m in mapNodes[0])
        {
            m.isPass = true;
            m.btn.interactable = true;
        }
    }

    
    public void HideOrShowAll(bool isShow)
    {
        foreach (var line in lines)
        {
                
            line.gameObject.SetActive(isShow);
        }
        foreach (var level in levels)
        {
                
            level.gameObject.SetActive(isShow);
        }
    }
    private void Clear()
    {
        
        if (lines.Count!=0)
        {
            iSeed = 1;
            foreach (var line in lines)
            {
                
               DestroyImmediate(line);
            }
            lines.Clear();
            for (int i = 0; i < maxLevel; i++)
            {
                mapNodes[i] = levels[i].nodes;
                for (int j = 0; j < mapNodes[0].Length; j++)
                {
                    mapNodes[i][j].level = 0;
                    mapNodes[i][j].value = 0;
                    mapNodes[i][j].isUsed = false;
                    mapNodes[i][j].gameObject.SetActive(false);
                    mapNodes[i][j].right = null;
                    mapNodes[i][j].left = null;
                    mapNodes[i][j].type = 0;
                }
            }
        }
   
    
    }

    void init()
    {
        if (iSeed==1)
        {
            iSeed = UnityEngine.Random.Range(0, 1000000);
        }
        else
        {
            DateTime currentTime = DateTime.Now;
            iSeed =  currentTime.Second + currentTime.Minute+currentTime.Hour;
        }
        random = new Random(iSeed);
        mapNodes = new MapNode[maxLevel][];
        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = levels[i].nodes;
            for (int j = 0; j < mapNodes[0].Length; j++)
            {
                mapNodes[i][j].level = i;
                mapNodes[i][j].value = j;
            }
        }
    }
    void CreateMap()
    {
        CreateTheFirstLevel();
        for (int i = 0; i < maxLevel - 1; i++)
        {
            ProcessThisLevel(i);
        }
    }
    void ProcessThisLevel(int currentLevel)
    {
       
        MapNode beUsedNode;
        bool canLeft = true;
        bool canRight = true;
        int firstNum = 0;
        int secondNum = 0;
        int nextLevelLeftPoint = 0;
        for (int i = 0; i < maxCount; i++)
        {
            if (mapNodes[currentLevel][i].isUsed)
            {
                if (mapNodes[currentLevel + 1][i].isUsed || nextLevelLeftPoint >= i)
                {
                    canLeft = false;
                }
                if (i >= maxCount - 1 || nextLevelLeftPoint >= maxCount - 1)
                {
                    canRight = false;
                }
                if (i == 0)
                {
                    canLeft = false;
                }
                if (canLeft && canRight)
                {
                    firstNum = random.Next(3);
                    secondNum = random.Next(3);
                }
                else if (canRight || canLeft)
                {
                    firstNum = random.Next(2);
                    secondNum = random.Next(2);
                }
                //确保firstNum <= secondNum
                if (firstNum > secondNum)
                {
                    firstNum = firstNum ^ secondNum;
                    secondNum = firstNum ^ secondNum;
                    firstNum = firstNum ^ secondNum;
                }
                beUsedNode = mapNodes[currentLevel + 1][nextLevelLeftPoint + firstNum];
                beUsedNode.isUsed = true;
                mapNodes[currentLevel][i].left = beUsedNode;
                beUsedNode = mapNodes[currentLevel + 1][nextLevelLeftPoint + secondNum];
                beUsedNode.isUsed = true;
                mapNodes[currentLevel][i].right = beUsedNode;
                nextLevelLeftPoint += secondNum;
                mapNodes[currentLevel][i].right = mapNodes[currentLevel][i].left == mapNodes[currentLevel][i].right ? null : mapNodes[currentLevel][i].right;
                firstNum = 0;
                secondNum = 0;
                canLeft = true;
                canRight = true;
            }
        }
    }


    void CreateTheFirstLevel()
    {
        var thisLevelCount = 5;
        for (int i = 0; i < thisLevelCount; i++)
        {
            mapNodes[0][random.Next(maxCount)].isUsed = true;
        }
    }

    public void Pass(UpdateLevel e)
    {
        foreach (var m in mapNodes[e.level])
        {
            m.isPass = false;
            if (m.btn)
            {
                m.btn.interactable = false;
            }
           
        }
        if (mapNodes[e.level][e.value].left)
        {
            mapNodes[e.level][e.value].left.isPass = true;
            mapNodes[e.level][e.value].left.btn.interactable = true; 
        }

        if (mapNodes[e.level][e.value].right)
        {
            mapNodes[e.level][e.value].right.isPass = true;
            mapNodes[e.level][e.value].right.btn.interactable = true;
        }
       
    }

    public void ShowRooms()
    {
        for (int i = 0; i < mapNodes.Length; i++)
        {
            for (int j = 0; j < mapNodes[0].Length; j++)
            {
                mapNodes[i][j].gameObject.SetActive(mapNodes[i][j].isUsed);
                if (mapNodes[i][j].isUsed)
                {
                    var type = mapNodes[i][j].Init();
                    switch (type)
                    {
                        case 1://普通
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("normal_monster"));
                            break;
                        case 2://精英
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("boss"));
                            break;
                        case 3://商队
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("store"));
                            break;
                        case 4://休息
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("fire"));
                            break;
                        case 5://boss
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("boss"));
                            break;
                        case 6://wenhao
                            mapNodes[i][j].img.sprite = LoadTexture(Resname.Texture("wenhao"));
                            break;
                        default:
                            break;
                
                    }
                   
                }
            }
        }
    }
    public void SetLevelsPosition()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetRoomsPosition(random);
        }
    }
    public void ShowLine()
    {
        var layer = LayerMask.NameToLayer("line");
        var cv =GameObject.Find("Canvas").gameObject.transform;
        for (int i = 0; i < mapNodes.Length; i++)
        {
          
            for (int j = 0; j < mapNodes[0].Length; j++)
            {
                if (mapNodes[i][j].isUsed)
                {
                    if (i == 0)
                    {
                        GameObject line = new GameObject();
                        line.layer = layer;
                        line.transform.SetParent(transform);
                        lines.Add(line);
                        LineRenderer ren = line.AddComponent<LineRenderer>();
                        ren.material = lineMaterial;
                        Vector3[] current = { born.transform.position, mapNodes[i][j].transform.position };
                        ren.SetPositions(current);
                    }
                    if (i == mapNodes.Length-1)
                    {
                        GameObject line = new GameObject();
                        line.layer = layer;
                        line.transform.SetParent(transform);
                        lines.Add(line);
                        LineRenderer ren = line.AddComponent<LineRenderer>();
                        ren.material = lineMaterial;
                        Vector3[] current = { boss.transform.position, mapNodes[i][j].transform.position };
                        ren.SetPositions(current);
                    }
                    if (mapNodes[i][j].left != null)
                    {
                        GameObject line = new GameObject();
                        line.layer = layer;
                        line.transform.SetParent(transform);
                        lines.Add(line);
                        LineRenderer ren = line.AddComponent<LineRenderer>();
                        ren.material = lineMaterial;
                        Vector3[] current = { mapNodes[i][j].transform.position, mapNodes[i][j].left.transform.position };
                        ren.SetPositions(current);
                    }
                    if (mapNodes[i][j].right != null)
                    {
                        GameObject line = new GameObject();
                        line.layer = layer;
                        line.transform.SetParent(transform);
                        lines.Add(line);
                        LineRenderer ren = line.AddComponent<LineRenderer>();
                        ren.material = lineMaterial;
                         Vector3[] current = { mapNodes[i][j].transform.position, mapNodes[i][j].right.transform.position };
                        ren.SetPositions(current);
                    }
                }

               
            }
        }
    }

}


}