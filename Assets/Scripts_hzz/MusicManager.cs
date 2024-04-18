using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource audiosource;
    public Button button;
    public Sprite playSprite;
    // public Sprite pressedSprite;
    public Sprite pauseSprite;
    // SpriteState _spriteState;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        // button = GameObject.FindWithTag("ButtonTag").GetComponent<Button>();
        // button.onClick.AddListener(OnMusicButtonClick);
    }


    public void OnMusicButtonClick()
    {
        isPaused = !isPaused;
        if (isPaused)
        {

            audiosource.Pause();
            button.image.sprite = pauseSprite;
        }

        else
        {
            StartCoroutine(Music_UnPause());
            button.image.sprite = playSprite;
        }

    }
    IEnumerator Music_UnPause()

    {
        yield return new WaitForSeconds(0.6f);
        audiosource.UnPause();
    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
