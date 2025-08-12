using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    private SpriteRenderer currentSprite;
    public Sprite openSprite;
    public Sprite closeSprite;
    private bool isDone = false;
    private BoxCollider2D currentCollider;

    void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        currentCollider = GetComponent<BoxCollider2D>();
    }

    void OnEnable()
    {
        currentSprite.sprite = isDone ? openSprite : closeSprite;
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            OpenTreasureChest();
            GetComponent<AudioDefination>()?.PlayAudio();
        }
    }

    public void OpenTreasureChest()
    {
        currentSprite.sprite = openSprite;
        isDone = true;
        gameObject.tag = "Untagged";
        currentCollider.enabled = false;
    }
}
