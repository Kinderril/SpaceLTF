using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{

    public Camera Camera;
//    public Camera MiniBackgroundCamera;
    public SpriteRenderer SpriteRenderer;
    private Sprite _spriteGlobal;
    public List<Sprite> BackgroundsGlobal = new List<Sprite>();
    public List<Sprite> BackgroundsBattle = new List<Sprite>();

    void Awake()
    {
        _spriteGlobal = BackgroundsGlobal.RandomElement();
        SpriteRenderer.sprite = _spriteGlobal;
    }
    public void EndBattleGame()
    {
        SpriteRenderer.sprite = _spriteGlobal;
        CheckSize();
    }

    private void CheckSize()
    {
        float cameraHeight = Camera.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.aspect * cameraHeight, cameraHeight);
        cameraSize = cameraSize / Camera.orthographicSize;
        Vector2 spriteSize = SpriteRenderer.sprite.bounds.size;
        if (spriteSize.y > spriteSize.x)
        {
            Debug.LogError("wrong background sprite! " + SpriteRenderer.sprite.name);
            return;
        }
        var camDim = cameraSize.x / cameraSize.y;
        var backDim = spriteSize.x / spriteSize.y;
        Debug.Log(" cam dims:" + camDim + "   " + backDim + "    cameraSize:" + cameraSize + "   spriteSize:" + spriteSize);
        if (camDim > backDim)
        {
//            MiniBackgroundCamera.orthographicSize= 
                Camera.orthographicSize = spriteSize.x / cameraSize.x;
        }
        else
        {
//            MiniBackgroundCamera.orthographicSize =
                Camera.orthographicSize = spriteSize.y / cameraSize.y;
        }

        //        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        {
           
            // Landscape (or equal)

//            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
//            scale *= cameraSize.y / spriteSize.y;
        }

        //        transform.position = Vector2.zero; // Optional
//        transform.localScale = scale;
    }

    public void StartGame()
    {
        SpriteRenderer.sprite = BackgroundsBattle.RandomElement();
        CheckSize();
        Camera.enabled = true;
//        MiniBackgroundCamera.enabled = true;
        SpriteRenderer.enabled = true;
    }

//    void Update()
//    {
//        if (Camera.enabled)
//        {
//
//        }
//    }

    public void EndGame()
    {
        Camera.enabled = false;
//        MiniBackgroundCamera.enabled = false;
        SpriteRenderer.enabled = false;
    }

}

