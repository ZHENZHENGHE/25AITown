using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hzz_PlayerPrefs : MonoBehaviour
{
    private const string PlayerPositionKey = "PlayerPosition";
    private const string PlayerCoin = "PlayerCoin";
    private const string PlayerLife = "PlayerLife";
    private const string TaskOne = "TaskOne";
    private const string TaskTwo = "TaskTwo";
    private const string TaskThree = "TaskThree";
    private const string TaskFour = "TaskFour";
    private const string TaskFive = "TaskFive";
    private float timer = 0f; // 计时器
    // Start is called before the first frame update
    void Start()
    {
        // 检查是否有保存的位置数据
        if (PlayerPrefs.HasKey(PlayerPositionKey))
        {
            // 从PlayerPrefs中加载保存的位置
            string savedPosition = PlayerPrefs.GetString(PlayerPositionKey);
            Vector3 position = StringToVector3(savedPosition);
            transform.position = position;
        }
        //保存用户金币的数据
        if (PlayerPrefs.HasKey(PlayerCoin))
        {
            int cost = PlayerPrefs.GetInt(PlayerCoin);
            GameObject coinButton = GameObject.FindWithTag("Coin");
            TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();
            cointext.text = cost.ToString();
        }
        //保存用户金币的数据
        if (PlayerPrefs.HasKey(PlayerLife))
        {
            int life_number = PlayerPrefs.GetInt(PlayerLife);
            GameObject lifeButton = GameObject.FindWithTag("LifeNumber");
            TextMeshProUGUI lifetext = lifeButton.GetComponent<TextMeshProUGUI>();
            lifetext.text = life_number.ToString();
        }
        //保存用户第一次任务的数据
        if (!PlayerPrefs.HasKey(TaskOne))
        {
            PlayerPrefs.SetInt(TaskOne, 1);
            PlayerPrefs.Save();
        }
        //保存用户第二次任务的数据
        if (!PlayerPrefs.HasKey(TaskTwo))
        {
            PlayerPrefs.SetInt(TaskTwo, 1);
            PlayerPrefs.Save();
        }
        //保存用户第三次任务的数据
        if (!PlayerPrefs.HasKey(TaskThree))
        {
            PlayerPrefs.SetInt(TaskThree, 1);
            PlayerPrefs.Save();
        }
        //保存用户第四次任务的数据
        if (!PlayerPrefs.HasKey(TaskFour))
        {
            PlayerPrefs.SetInt(TaskFour, 1);
            PlayerPrefs.Save();
        }
        //保存用户第五次任务的数据
        if (!PlayerPrefs.HasKey(TaskFive))
        {
            PlayerPrefs.SetInt(TaskFive, 1);
            PlayerPrefs.Save();
        }
    }
    private void OnDestroy()
    {
        // 将当前位置保存到PlayerPrefs中
        string currentPosition = Vector3ToString(transform.position);
        PlayerPrefs.SetString(PlayerPositionKey, currentPosition);
        PlayerPrefs.Save();
    }

    private Vector3 StringToVector3(string input)
    {
        // 将字符串转换为Vector3
        string[] components = input.Split(',');
        float x = float.Parse(components[0]);
        float y = float.Parse(components[1]) - 0.5f;
        float z = float.Parse(components[2]);
        return new Vector3(x, y, z);
    }

    private string Vector3ToString(Vector3 input)
    {
        // 将Vector3转换为字符串
        string x = input.x.ToString();
        string y = input.y.ToString();
        string z = input.z.ToString();
        return string.Format("{0},{1},{2}", x, y, z);
    }
    // Update is called once per frame
    void Update()
    {
        //保存用户金币
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();
        int coin = int.Parse(cointext.text);
        PlayerPrefs.SetInt(PlayerCoin, coin);
        PlayerPrefs.Save();
        //保存用户生命值
        GameObject lifeButton = GameObject.FindWithTag("LifeNumber");
        TextMeshProUGUI lifetext = lifeButton.GetComponent<TextMeshProUGUI>();
        int life_number = int.Parse(lifetext.text);
        // 每隔1分钟，生命值减一
        timer += Time.deltaTime;
        if (timer >= 60f) 
        {
            timer = 0f; 
            life_number--; 
            if (life_number > 0)
            {
                lifetext.text = life_number.ToString();
                Debug.Log("当前生命值：" + life_number);
            }
            else
            {
                Debug.Log("玩家已死亡！");
                // 触发玩家死亡逻辑，例如游戏结束、重置场景等

                // 清除所有的 PlayerPrefs
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();

                // 退出游戏
                Application.Quit();

                // 如果在编辑器中运行，停止播放模式
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
        PlayerPrefs.SetInt(PlayerLife, life_number);
        PlayerPrefs.Save();
    }
}
