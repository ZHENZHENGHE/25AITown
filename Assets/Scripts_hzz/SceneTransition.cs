using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float fadeDuration = 0.6f; // 淡入淡出的持续时间
    public CanvasGroup fadeCanvasGroup; // 用于控制淡入淡出的CanvasGroup组件

    private void Start()
    {
        // 在开始时立即开始淡入效果
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
         fadeCanvasGroup.alpha = 0f; // 设置透明度为完全透明

        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration; // 每帧逐渐增加透明度
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // 设置透明度为完全不透明
    }

    private IEnumerator FadeOut(string sceneName)
    {

        fadeCanvasGroup.alpha = 1f; // 设置透明度为完全不透明

        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration; // 每帧逐渐减小透明度
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; // 设置透明度为完全透明

        SceneManager.LoadScene(sceneName); // 加载新场景
    }
}