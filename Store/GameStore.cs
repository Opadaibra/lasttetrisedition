﻿using System.Collections.Generic;
 using UnityEngine;

 public class GameStore:MonoBehaviour
    {
        [SerializeField] 
        private  List<Item> items =new List<Item>();
        public List<Item> getItemStore() 
        { 
            return items; 
        }
        public void setItemInfo(Item item) 
        { 
            if(item.get_itemName()=="earthquick")
                items.Add(Earthquick.getInstance());
            else if (item.get_itemName()=="blizzerd") 
            { 
                items.Add(Earthquick.getInstance()); 
            }
        }

        public void buy(Player player, Item item)
        {
            if (player.Coins>= item.get_price())
            {
                player.addItem(item);
                player.Coins -= item.price;
            }
        }

        public void Upgrade(Item item)
        {
            item.Upgrade();
        }
    }
