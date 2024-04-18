using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetMusicBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        MusicManager music = GameObject.FindWithTag("Music").GetComponent<MusicManager>();
        music.button = GameObject.FindWithTag("ButtonTag").GetComponent<Button>();
        music.button.onClick.AddListener(music.OnMusicButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
