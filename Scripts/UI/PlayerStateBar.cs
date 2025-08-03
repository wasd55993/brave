using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    [Header("组件获取")]
    public Image hp;
    public Image hpHurt;
    public Image power;

    private void Update()
    {
        //受伤缓慢减小
        if (hpHurt.fillAmount > hp.fillAmount)
        {
            hpHurt.fillAmount -= Time.deltaTime;
        }
    }


    //血量变化
    //percentage = currentHP / MaxHP
    public void OnHpChange(float percentage)
    {
        hp.fillAmount = percentage;
    }
}
