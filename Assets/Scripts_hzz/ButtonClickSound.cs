using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound;
    private Button button;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(PlayClickSound);

    }

    private void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
