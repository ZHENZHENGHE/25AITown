using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DialogueModel
{
    //正在运行的树
    public DialogueTree Tree;
    //所有树
    public  static Dictionary<int,DialogueTree> Trees = new Dictionary<int, DialogueTree>();
    public static Dictionary<string,DialogueTree> Trees2 = new Dictionary<string, DialogueTree>();
    public static DialogueModel Instance = new DialogueModel();
    [MenuItem("Tool/InitModel")]
    public static void InitModel()
    {
        var datas = AssetDatabase.LoadAllAssetsAtPath("Assets/DialogueModule/DialogueData");
        foreach (var data in datas)
        {
            if (data is DialogueTree)
            {
                Trees.Add(data.GetInstanceID(),data as DialogueTree);
            }
        }
    }
    [MenuItem("Custom/Find ScriptableObjects")]
    public static void FindScriptableObjects()
    {
        string folderPath = "Assets/DialogueModule/DialogueData"; // 替换为你想要查找的文件夹路径
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            if (scriptableObject != null)
            {
                // 在这里处理找到的ScriptableObject
                Debug.Log("Found ScriptableObject: " + scriptableObject.name);
                Trees2.Add(scriptableObject.name,scriptableObject as DialogueTree);
            }
        }
    }
    public DialogueTree GetRunningDialogue()
    {
        return Tree!=null?Tree:null;
    }
    public DialogueTree GetDialogueById(int id)
    {
        Trees.TryGetValue(id, out var tree);
        return tree;
    }
    public DialogueTree GetDialogueByName(string name)
    {
        Trees2.TryGetValue(name, out var tree);
        return tree;
    }
    //加载背景图片
    public Node.Bg GetBgTexture()
    {
        return Tree.runningNode.bg;
    }
    //播放背景音乐
    public string GetBgMusic()
    {
        return Tree.runningNode.musicBg;
    }
    //播放背景音效
    public string GetFxMusic()
    {
        return Tree.runningNode.musicFx;
    }
    //显示文本
    public string GetShowContent()
    {
        return Tree.runningNode.dialogueContent;
    }
    //播放人物动画
    public string GetRoleAnime()
    {
        return Tree.runningNode.roleAnime;
    }
    //加载人物图片
    public string GetRoleTexture()
    {
        return Tree.runningNode.role;
    }
    //加载人物位置
    public int GetRolePos()
    {
        return Tree.runningNode.rolePosition;
    }
}
