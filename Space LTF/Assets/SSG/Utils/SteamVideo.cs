using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;

public class SteamVideo : MonoBehaviour
{
    public RawImage RawImage;
    public VideoPlayer VideoPlayer;


    // Use this for initialization
    void Start()
    {
//        RawImage.texture = VideoPlayer.texture;
        VideoPlayer.Play();
    }



}
