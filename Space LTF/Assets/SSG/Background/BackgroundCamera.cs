using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{

    public Camera Camera;
//    public Camera MiniBackgroundCamera;
    public SpriteRenderer SpriteBattleRenderer;
    public SpriteRenderer SpriteMenuRenderer;
    private Sprite _spriteGlobal;
    public List<Sprite> BackgroundsGlobal = new List<Sprite>();
    public List<Sprite> BackgroundsBattle = new List<Sprite>();

    void Awake()
    {
        _spriteGlobal = BackgroundsGlobal.RandomElement();
        // SpriteBattleRenderer.sprite = _spriteGlobal;
        SpriteMenuRenderer.sprite = _spriteGlobal;
        ActivateSprites(false);
    }
    public void EndBattleGame()
    {
        ActivateSprites(false);
        CheckSize(false);
    }

    private void ActivateSprites(bool battle)
    {

        SpriteBattleRenderer.gameObject.SetActive(battle);
        SpriteMenuRenderer.gameObject.SetActive(!battle);
    }

    private void CheckSize(bool isBattle)
    {
        float cameraHeight = Camera.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.aspect * cameraHeight, cameraHeight);
        cameraSize = cameraSize / Camera.orthographicSize;
        var sprite = isBattle ? SpriteBattleRenderer : SpriteMenuRenderer;
        Vector2 spriteSize = sprite.sprite.bounds.size;
        if (spriteSize.y > spriteSize.x)
        {
            Debug.LogError("wrong background sprite! " + SpriteBattleRenderer.sprite.name);
            return;
        }
        var camDim = cameraSize.x / cameraSize.y;
        var backDim = spriteSize.x / spriteSize.y;
        // Debug.Log(" cam dims:" + camDim + "   " + backDim + "    cameraSize:" + cameraSize + "   spriteSize:" + spriteSize);
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
        SpriteBattleRenderer.sprite = BackgroundsBattle.RandomElement();
        CheckSize(true);
        Camera.enabled = true;
        ActivateSprites(true);
    }
}

