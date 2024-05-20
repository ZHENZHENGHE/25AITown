using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCostManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int Food_Cost = 0;
    public int Mushroom = 0;
    public int life_number = 0;
    private int _taskOneReward = 50;
    private int _taskTwoReward = 50;
    private int _taskThreeReward = 50;
    private int _taskFourReward = 60;
    private int _taskFiveReward = 60;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CostForFood()
    {
        //用户金币消耗操作
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue -= Food_Cost;
        }
        cointext.text = intValue.ToString();
        //用户生命值增加操作
        GameObject lifeButton = GameObject.FindWithTag("LifeNumber");
        TextMeshProUGUI life_number_text = lifeButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(life_number_text.text, out int _intValue))
        {
            _intValue += life_number;
            if (_intValue > 100)
            {
                _intValue = 100;
            }
        }
        life_number_text.text = _intValue.ToString();
    }
    public void PickMushroom()
    {
        //采集蘑菇金币增加操作
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += Mushroom;
        }
        cointext.text = intValue.ToString();
    }
    public void TaskOneReward()
    {
        //认识居民任务完成获得50金币
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += _taskOneReward;
        }
        cointext.text = intValue.ToString();
    }
    public void TaskTwoReward()
    {
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += _taskTwoReward;
        }
        cointext.text = intValue.ToString();
    }
    public void TaskThreeReward()
    {
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += _taskThreeReward;
        }
        cointext.text = intValue.ToString();
    }
    public void TaskFourReward()
    {
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += _taskFourReward;
        }
        cointext.text = intValue.ToString();
    }
    public void TaskFiveReward()
    {
        GameObject coinButton = GameObject.FindWithTag("Coin");
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();

        if (int.TryParse(cointext.text, out int intValue))
        {
            intValue += _taskFiveReward;
        }
        cointext.text = intValue.ToString();
    }
}
