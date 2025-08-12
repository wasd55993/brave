using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public SpriteRenderer saveShow;
    public Sprite saveShowDark;
    public Sprite saveShowLight;
    private bool isDone = false;
    private BoxCollider2D currentCollider;

    private void Awake()
    {
        currentCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        saveShow.sprite = isDone ? saveShowLight : saveShowDark;
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            saveShow.sprite = saveShowLight;

            //TODO:保存数据

            gameObject.tag = "Untagged";
            currentCollider.enabled = false;
        }
    }
}
