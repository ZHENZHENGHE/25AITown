using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ButtonEvent_hzz : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ComfirmPurchaseCanvas;

    public GameObject TaskCanvas;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTerrianButtonClick()
    {
        // SceneManager.LoadScene("Terrian");
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        sceneTransition.FadeToScene("Terrian");
    }

    public void ReturnHomeBtn()
    {
        StartCoroutine(WaitforReturnHome());
    }
    public void ReturnTerrianBtn()
    {
        SceneManager.LoadScene("Terrian");
    }
    public void ReturnEnterGameBtn()
    {
        StartCoroutine(WaitforEnterGame());
    }
    //进入开发者场景Btn
    public void OnDeveloperInformationBtn()
    {
        StartCoroutine(WaitforClickMusic());
    }
    //关闭Foods_Cavas画布
    public void OnCloseFoodsCavasBtn()
    {
        StartCoroutine(OnCloseFoodsCavasBtn_IE());
    }
    //关闭Restaurant_Cavas画布
    public void OnCloseRestaurantCavasBtn()
    {
        StartCoroutine(OnCloseRestaurantCavasBtn_IE());
    }
    //关闭确认购买食物Canvas ---------yes
    public void OnCloseComfirmPurchaseFoodsBtn()
    {
        StartCoroutine(OnCloseComfirmPurchaseFoodsBtn_IE());
    }
    //关闭确认购买食物Canvas ---------no
    public void OnCloseComfirmPurchaseFoodsBtn2()
    {
        StartCoroutine(OnCloseComfirmPurchaseFoodsBtn2_IE());
    }
    //关闭neighbor对话canvas
    public void OnCloseNeighborCanvas()
    {
        StartCoroutine(OnCloseNeighborCanvas_IE());
    }
    //选择食物
    public void OnChooseFood(int cost)
    {
        StartCoroutine(OnChooseFoods_IE(cost));
    }
    public void OnChooseFood_life(int life)
    {
        StartCoroutine(OnChooseFoods_life_IE(life));
    }
    //任务
    public void OnTaskSet(int index)
    {
        ComfirmPurchaseCanvas.SetActive(true);
        TextMeshProUGUI TextMeshPro = GameObject.FindWithTag("TaskText").GetComponent<TextMeshProUGUI>();
        if (index == 1)
        {
            if (PlayerPrefs.GetInt("TaskOne") == 0)
            {
                TextMeshPro.text = "任务已完成！";
            }
            else TextMeshPro.text = "快去认识小镇中的居民吧！靠近小镇中的居民你将会获得他们的人物详细信息，每认识一位居民将获得5金币的奖励！";
        }
        if (index == 2)
        {
            TextMeshPro.text = "text02";
        }
        if (index == 3)
        {
            TextMeshPro.text = "text03";
        }
        if (index == 4)
        {
            TextMeshPro.text = "text04";
        }
        if (index == 5)
        {
            TextMeshPro.text = "text05";
        }
    }
    //打开任务面板
    public void OnOpenTaskCanvas()
    {
        StartCoroutine(OnOpenTaskCanvas_IE());
    }
    //关闭任务面板
    public void OnCloseTaskCanvas()
    {
        StartCoroutine(OnCloseTaskCanvas_IE());
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    IEnumerator OnCloseNeighborCanvas_IE()
    {
        yield return new WaitForSeconds(0.3f);
        Canvas canvas = GameObject.FindWithTag("neighbor_Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
        // Time.timeScale = 1f; // 将时间缩放比例设为1，恢复游戏
    }
    IEnumerator OnCloseTaskCanvas_IE()
    {
        yield return new WaitForSeconds(0.4f);
        TaskCanvas.SetActive(false);
    }
    IEnumerator OnOpenTaskCanvas_IE()
    {
        yield return new WaitForSeconds(0.4f);
        TaskCanvas.SetActive(true);
    }
    IEnumerator OnCloseComfirmPurchaseFoodsBtn2_IE()
    {
        yield return new WaitForSeconds(0.4f);
        ComfirmPurchaseCanvas.SetActive(false);
    }
    IEnumerator OnCloseFoodsCavasBtn_IE()
    {
        yield return new WaitForSeconds(0.4f);
        Canvas canvas = GameObject.FindWithTag("Foods_Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
        CollisionHandler scripts = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();
        scripts.SetMainCavas_UI();
    }
    IEnumerator OnCloseRestaurantCavasBtn_IE()
    {
        yield return new WaitForSeconds(0.4f);
        Canvas canvas = GameObject.FindWithTag("restaurant_Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
        CollisionHandler scripts = GameObject.FindWithTag("Player").GetComponent<CollisionHandler>();
        scripts.SetMainCavas_UI();
    }
    IEnumerator OnCloseComfirmPurchaseFoodsBtn_IE()
    {
        yield return new WaitForSeconds(0.4f);
        ComfirmPurchaseCanvas.SetActive(false);
        GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
        PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
        script.CostForFood();
        // CostForFood();
    }
    IEnumerator OnChooseFoods_IE(int cost)
    {
        yield return new WaitForSeconds(0.4f);
        GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
        PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
        script.Food_Cost = cost;
        ComfirmPurchaseCanvas.SetActive(true);
    }
    IEnumerator OnChooseFoods_life_IE(int life)
    {
        yield return new WaitForSeconds(0.4f);
        GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
        PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
        script.life_number = life;
    }
    IEnumerator WaitforClickMusic()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("developerInformation");
    }
    IEnumerator WaitforEnterGame()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("EnterGame");

    }

    IEnumerator WaitforReturnHome()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("EnterGame");

    }
}
