﻿﻿using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  using UnityEngine.TestTools;
[Serializable]
  public abstract class Item: MonoBehaviour
{


    public string _itemName;

    public string get_itemName()
    {
        return _itemName;}

    public void set_itemName(string name)
    {
        this._itemName = name;

    }

    public int price;

    public int get_price()
    {
        return price;
    }

    public void set_price(int price)
    {
        this.price = price;
    }


    public int level;

    public int get_level()
    {
        return level;
    }

    public void set_level(int level)
    {
        this.level = level;
    }
    
    private void OnMouseDown()
    {
        FindObjectOfType<GameManger>().UseItem(this);
    }
    
    public abstract void Upgrade();
    public abstract void logic(GameBoard gb);
    
}