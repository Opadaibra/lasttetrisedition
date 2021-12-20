﻿using UnityEngine;
 public abstract class GameBoardStyle:MonoBehaviour
    {
        protected GameBoard gameBoard ;
        public abstract bool isEnd() ;

        public virtual string Encode()
        {
            return "";
            
        }
        public virtual  void Decode(string code)
        {
        }
        public void Effect() { }
        public void setBoard(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }
    }