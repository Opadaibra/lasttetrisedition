﻿﻿using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  [Serializable]
public class Blizzerd : Item
{
    private int time = 0;
    private static Blizzerd  instance;
    private bool Clickable = true;

    private Blizzerd()
    {
        _itemName =" blizzerd";
        price = 5;
        level = 1;
    }

    public static Blizzerd getInstance()
    {
        if (instance == null)
        {
            GameObject myObj=new GameObject("Blizzerd");
            myObj.AddComponent<Blizzerd>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public override void Upgrade()
    {
        this.level++;
        if (this.level == 2)
        {
           
            this.price = 6
                ;

        }
        else if (this.level == 3)
        {
            this.price = 7;
        }
    }

    public override void logic(GameBoard gb)
    {if(this.level==1 )
        {
            time = 5;
        }
        else if (this.level == 2) 
        {
        time = 8; 
        }
        else if (this.level== 3 ) {
            time = 12;
        }
        StartCoroutine(FrezzAndWait(time));


    }
    IEnumerator FrezzAndWait(float time)
    {
        Clickable = false;
        transform.GetComponent<SpriteRenderer>().color = Color.gray;
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(time/2);
        Time.timeScale = 1.0f;
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        Clickable = true;
    }
}
