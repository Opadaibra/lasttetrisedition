﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  [Serializable]
public class Earthquick : Item
{
    private static Earthquick instance;
    private Earthquick()
    {
        this._itemName = "earthquick";
        this.price = 6;
        this.level = 1;
    }

    public static Earthquick getInstance()
    {
        if (instance == null)
        { 
            GameObject myObj=new GameObject("Earthquick"); 
            myObj.AddComponent<Earthquick>();
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
        if (this.level == 2 )
        {
            this.price =8;
        }
        else if (this.level==3 )
        {
            this.price = 10;

        }

    }

    public override void logic(GameBoard gb)
    {
        if (this.level==1)
        {
            gb.Clear(new List<int>() { 0 }); 
        }
        else if (this.level == 2)
        {
            gb.Clear(new List<int>() { 0,1 });
        }
        else if (this.level == 3)
        {
            gb.Clear(new List<int>(){ 0, 1 ,2 });
        }

    }
}
