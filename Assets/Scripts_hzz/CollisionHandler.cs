

using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    public GameObject MainCavas_UI;
    public GameObject Foods_Cavas;
    public GameObject restaurant_Cavas;
    // Start is called before the first frame update
    void Start()
    {
        // MainCavas_UI.SetActive(true);
        Foods_Cavas.SetActive(false);
        restaurant_Cavas.SetActive(false);
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
