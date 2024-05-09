using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hzz_PlayerPrefs : MonoBehaviour
{
    private const string PlayerPositionKey = "PlayerPosition";
    private const string PlayerCoin = "PlayerCoin";
    private const string TaskOne = "TaskOne";
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
        //保存用户第一次任务的数据
        if (!PlayerPrefs.HasKey(TaskOne))
        {
            PlayerPrefs.SetInt(TaskOne, 1);
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

    }
}
