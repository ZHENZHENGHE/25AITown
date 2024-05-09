

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{

    public GameObject MainCavas_UI;
    public GameObject Foods_Cavas;
    public GameObject restaurant_Cavas;
    public GameObject neighbor_Canvas;


    // Start is called before the first frame update
    void Start()
    {
        // MainCavas_UI.SetActive(true);
        if (Foods_Cavas) Foods_Cavas.SetActive(false);
        if (restaurant_Cavas) restaurant_Cavas.SetActive(false);
        if (neighbor_Canvas) neighbor_Canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMainCavas_UI()
    {
        MainCavas_UI.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_House"))
        {
            Debug.Log("Enter_Player_House");
            // SceneManager.LoadScene("PlayerIndoorScence");
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            sceneTransition.FadeToScene("PlayerIndoorScence");
        }
        if (collision.gameObject.CompareTag("IndoorScence01"))
        {
            Debug.Log("Enter_IndoorScence01");
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            sceneTransition.FadeToScene("IndoorScence01");
        }
        if (collision.gameObject.CompareTag("IndoorScence02"))
        {
            Debug.Log("Enter_IndoorScence02");
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            sceneTransition.FadeToScene("IndoorScence02");
        }
        if (collision.gameObject.CompareTag("IndoorScence03"))
        {
            Debug.Log("Enter_IndoorScence03");
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            sceneTransition.FadeToScene("IndoorScence03");
        }
        if (collision.gameObject.CompareTag("SuperMarket"))
        {
            Debug.Log("Enter_SuperMarket");
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            sceneTransition.FadeToScene("SuperMarket");
        }
        //Load  FoodCavas
        if (collision.gameObject.CompareTag("Cafe"))
        {
            MainCavas_UI.SetActive(false);
            Foods_Cavas.SetActive(true);
        }
        if (collision.gameObject.CompareTag("restaurant"))
        {
            MainCavas_UI.SetActive(false);
            restaurant_Cavas.SetActive(true);
        }
        if (collision.gameObject.CompareTag("neighbor001"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "John - 一个友善的教师，热衷于教授历史知识";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor002"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Emily - 一位充满活力的摄影师，喜欢捕捉大自然的美丽瞬间";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor003"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "David - 一名年轻的科学家，致力于研究环境保护和可持续发展";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor004"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Sarah - 一位热心的志愿者，经常参与社区活动和慈善事业";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor005"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Michael - 一名优秀的音乐家，擅长弹奏吉他和作曲";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor006"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Olivia - 一位独立的作家，她喜欢写小说和诗歌";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor007"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Daniel - 一个有创意的设计师，擅长创作独特的艺术品";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor008"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Sophia - 一位有爱心的医生，致力于帮助他人恢复健康";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor009"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Ethan - 一位冒险家，喜欢探索未知的地方并记录旅程";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor010"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Ava - 一名优秀的舞者，舞蹈是她生活中的激情";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor011"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Jacob - 一位年轻的企业家，正在建立自己的创业公司";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor012"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Mia - 一位有天赋的画家，她的画作充满了想象力和美感";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor013"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "William - 一位热爱运动的教练，善于激励人们达到最佳状态";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor014"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Isabella - 一位聪明的科学家，专注于医学研究和新药开发";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor015"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "James - 一位技术专家，对计算机编程和人工智能充满热情";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor016"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Charlotte - 一位有才华的演员，擅长诠释各种角色";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor017"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Benjamin - 一名热心的环保主义者，致力于推动可持续生活方式";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor018"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Harper - 一位善于沟通的市场营销专家，擅长品牌推广和市场策略";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor019"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Alexander - 一名热衷于社会公益的律师，为弱势群体争取权益";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor020"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Ella - 一位自由心灵的旅行家，她探索世界各地的文化和风景";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor021"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "        Sophia - 一位富有同情心的社工，致力于帮助弱势群体改善生活";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor022"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Noah - 一个有创造力的厨师，喜欢研究和创作美味的料理";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor023"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Liam - 一个热爱户外运动的冒险家，喜欢徒步旅行和攀登高峰";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }
        if (collision.gameObject.CompareTag("neighbor024"))
        {
            neighbor_Canvas.SetActive(true);
            Text neighbor_text = GameObject.FindWithTag("neighbor_text").GetComponent<Text>();
            neighbor_text.text = "Lily - 一位热爱美食的美食家，对各种口味和烹饪技巧都有深入的了解。她喜欢探索不同的菜系和食材，以创造出独特的美食体验";
            if (PlayerPrefs.GetInt("TaskOne") == 1)
            {
                GameObject CostManager_Scripts = GameObject.FindWithTag("PlayerCostManager");
                PlayerCostManager script = CostManager_Scripts.GetComponent<PlayerCostManager>();
                script.TaskOneReward();
                PlayerPrefs.SetInt("TaskOne", 0);
            }
        }

    }
    // void OnCollisionEnter(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player_House"))
    //     {
    //         Debug.Log("Enter_Player_House");
    //         SceneManager.LoadScene("PlayerIndoorScence");
    //     }
    // }
}
