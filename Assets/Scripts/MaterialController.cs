using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite defaultSprite;
    public Sprite invisibleSprite;

    public void MakeInvisible() {
        sr.sprite = invisibleSprite;
    }

    public void MakeVisible() {
        sr.sprite = defaultSprite;
    }
}
