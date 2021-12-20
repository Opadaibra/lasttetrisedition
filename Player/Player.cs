﻿using System ;
using System.Collections ;
using System.Collections.Generic;
using System.Configuration;
 using System.Linq;
 using System.Runtime.InteropServices;
 using System.Text;
 using UnityEngine;

 [Serializable]
public class Player
{
    [Serializable]
    public struct ListElement
    {
        public ListElement(Item item, int number)
        {
            this.item = item;
            this.number = number;
        }
        public readonly Item item;
        public int number;
    }
    //attributes
    private string _name;

    private string _id;
    
    private int _coins;

    private int _bestScore;

    private  List<ListElement> _myItems;
    
    private string _nameAndSubId;
    
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public int Coins
    {
        get { return _coins; }
        set { _coins = value; }
    }

    public int BestScore
    {
        get { return _bestScore; }
        set { _bestScore = value; }
    }

    public List<ListElement> MyItems
    {
        get { return _myItems;}
        set { _myItems = value;}
    }

    //methods
    public Player(string name="Player")
    {
        _name = name ;
        _id = Guid.NewGuid().ToString("N").ToLower().Substring(0,15) ;
        _coins = 0 ;
        _myItems = new List<ListElement>();
        _bestScore = 0;
    }

    public void addItem(Item item)
    {
        foreach (var element in _myItems)
        {
            var listElement = element;
            if (listElement.item == item)
            {
                listElement.number++;
                return;
            }
        }
        _myItems.Add(new ListElement(item ,1));
    }
    
    public bool IsAvilable(Item item)
    {
        foreach (ListElement element in _myItems)
            if (element.item == item)
                if (element.number > 0)
                    return true;
        return false;
    }

    public string GetNameWithSubId()
    {
        if (!string.IsNullOrEmpty(_nameAndSubId))
            return _nameAndSubId;
        var nameAndSubId = _name + _id.Substring(0, 10);
        var oldByteArray = Encoding.ASCII.GetBytes(nameAndSubId);
        _nameAndSubId = Encoding.ASCII.GetString(oldByteArray.Where(b => b != 63).ToArray());
        return _nameAndSubId;
    }
    public override string ToString()
    {
        return $"Player Name : {_name}\nCoins : {_coins}\nBest Score : {_bestScore}";
    }
}