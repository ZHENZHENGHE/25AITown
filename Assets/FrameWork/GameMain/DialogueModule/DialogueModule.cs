using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueModule 
{
  
  public static DialogueModule Instance = new DialogueModule();


  public void StartDialogue()
  {
    DialogueModel.Instance.GetRunningDialogue().OnTreeStart();
  }
 
  public void EndDialogue()
  {
    DialogueModel.Instance.GetRunningDialogue().OnTreeEnd();
  }
  
  public void NextDialogue()
  {
    DialogueModel.Instance.GetRunningDialogue().Update();
  }
  
  public void SaveLogDialogue()
  {
    
  }
  
  public void JumpDialogue()
  {
    
  }
  
  public void FastDialogue()
  {
    
  }
  //面板方法
  //加载背景图片
  public void LoadBgTexture(string pictureName){}
  //播放背景音乐
  public void PlayBgMusic(string soundName){}
  //播放背景音效
  public void PlayFxMusic(string soundName){}
  //显示文本
  public void ShowContent(string content){}
  //播放人物动画
  public void PlayRoleAnime(int role,int id){}
  //加载人物图片
  public void LoadRoleTexture(int id,int emoId,int pos){}
}
