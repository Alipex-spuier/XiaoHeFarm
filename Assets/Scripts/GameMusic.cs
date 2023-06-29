using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameMusic instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public AudioSource bgmAudio;
    public Slider volumeSlider;
    public void Update()
    {
        bgmAudio.volume = volumeSlider.value;
    }
}
