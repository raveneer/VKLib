using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaydeSpriteEffect : MonoBehaviour
{
    public SpriteRenderer OriginSpriteRenderer;
    public SpriteRenderer Renderer;
    private Sprite _cachedSprite;
    public int DelayFrame = 2;

    private Queue<Sprite> delaySpritesQueue = new Queue<Sprite>();

    private void Update()
    {
        if (delaySpritesQueue.Count < DelayFrame)
        {
            delaySpritesQueue.Enqueue(OriginSpriteRenderer.sprite);
        }
        else
        {
            var sp = delaySpritesQueue.Dequeue();
            Renderer.sprite = sp;
            delaySpritesQueue.Enqueue(OriginSpriteRenderer.sprite);
        }

    }
    
    
}
