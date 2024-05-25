using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mushroom01; // 要生成的GameObject
    public GameObject mushroom02; // 要生成的GameObject
    public GameObject mushroom03; // 要生成的GameObject
    public GameObject mushroom04; // 要生成的GameObject
    public GameObject mushroom05; // 要生成的GameObject
    public GameObject mushroom06; // 要生成的GameObject
    public float spawnInterval = 60f; // 生成间隔时间（10分钟）
    public Vector2 spawnAreaMin; // 生成区域的最小坐标
    public Vector2 spawnAreaMax; // 生成区域的最大坐标
    void Start()
    {
        StartCoroutine(SpawnObjectRoutine01());
        StartCoroutine(SpawnObjectRoutine02());
        StartCoroutine(SpawnObjectRoutine03());
        StartCoroutine(SpawnObjectRoutine04());
        StartCoroutine(SpawnObjectRoutine05());
        StartCoroutine(SpawnObjectRoutine06());
    }

    // Update is called once per frame
    void Update()
    {
        // 检测是否按下了 Escape 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 打印调试信息
            Debug.Log("玩家按下了 Esc 键，游戏即将退出。");

            // 退出游戏
            Application.Quit();

            // 如果在编辑器中运行，停止播放模式
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
    private IEnumerator SpawnObjectRoutine01()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom01 = Instantiate(mushroom01, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator SpawnObjectRoutine02()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom02 = Instantiate(mushroom02, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator SpawnObjectRoutine03()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom03 = Instantiate(mushroom03, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator SpawnObjectRoutine04()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom04 = Instantiate(mushroom04, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator SpawnObjectRoutine05()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom05 = Instantiate(mushroom05, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator SpawnObjectRoutine06()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 随机生成位置
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 实例化对象
            GameObject Mushroom06 = Instantiate(mushroom06, spawnPosition, Quaternion.identity);
        }
    }
}
