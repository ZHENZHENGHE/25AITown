using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCostManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int Food_Cost = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FoodPriceInit(int cost)
    {
        Food_Cost = cost;
    }

    public void CostForFood()
    {
        //用户金币消耗操作
        GameObject coinButton = GameObject.FindWithTag("Coin");//找不到coin，因为mainUI被false了！
        TextMeshProUGUI cointext = coinButton.GetComponent<TextMeshProUGUI>();
        int intValue = 0;

        if (int.TryParse(cointext.text, out intValue))
        {
            intValue -= Food_Cost;
        }
        cointext.text = intValue.ToString();
    }
}
