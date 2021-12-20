﻿using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
 using System.Linq;

 public abstract class GameBoard : MonoBehaviour
{
    //Attributes
    private int _lines;
    //public for test only
   [SerializeField] public GameBoardStyle _style;
   
   protected static readonly Dictionary<char, Color> ColorsMap = new Dictionary<char, Color>()
   {
       {'a',new Color(0,  255/255.0f,209/255.0f,255/255.0f)},
       {'b',new Color(90/255.0f, 119/255.0f, 255/255.0f,255/255.0f)},
       {'c',new Color(0,  204/255.0f, 11/255.0f,255/255.0f)},
       {'d',new Color(255/255.0f,138/255.0f, 0,255/255.0f)},
       {'e',new Color(219/255.0f,5/255.0f,   5/255.0f,255/255.0f)},
       {'f',new Color(172/255.0f,0,  173/255.0f,255/255.0f)},
       {'g',new Color(251/255.0f,255/255.0f, 23/255.0f,255/255.0f)}
       
   };

   public abstract float GetElementSize();
   public abstract void SetElementSize(float size);
    //Properties
    public int Lines
     {
          get { return _lines; }
          set { _lines = value; }
     }
     //methods
     public abstract List<int> Check();
     
     public abstract void Clear(List<int> lines);
     
     public abstract bool ValidMove(Shape shape , Vector3 newPosition,float angle);
     
     public abstract GameObject GetElement(params int[] cord);
     
     public abstract int GetHeight();
     
     public abstract void SetShape(Shape shape);

     public abstract bool CheckAny(int level,bool onlyMiddleElement);

     public abstract string[] Encode(Shape currShape);
     
     public abstract string[] Encode();

     public abstract void Decode(string boardState, string boardColors,ShapeFactory shapeFactory);
     public bool IsEnd()
     {
         return _style.isEnd();
     }

     public void SetStyle(GameBoardStyle style)
     {
         _style = style;
     }

     protected static char GetColorsKeyByValue(Color value)
     {
         foreach (var key in ColorsMap.Keys)
             if (Math.Abs(value.r - ColorsMap[key].r) < 0.01f
             &&Math.Abs(value.g - ColorsMap[key].g) < 0.01f
             &&Math.Abs(value.b - ColorsMap[key].b) < 0.01f
             &&Math.Abs(value.a - ColorsMap[key].a) < 0.01f)
                 return key;
         return 'A';
     }

     
     //test for Save and load
     public string GetBoardStateAsJson()
     {
         var boardStateAndColors = Encode();
         var gameBoardStyleState = _style.Encode();
         var currGameBoardState = new GameBoardCheckPoint
         {
             GameBoardState = boardStateAndColors[0] ,
             GameBoardElementsColorState = boardStateAndColors[1] ,
             GameBoardStyleState = gameBoardStyleState ,
             GameBoardLines = _lines,
             ElementSize =  GetElementSize() 
         };
         return JsonUtility.ToJson(currGameBoardState);
     }
     public void SetBoardStateAsJson(string boardStateAsJson,ShapeFactory shapeFactory)
     {
         Debug.Log(boardStateAsJson);
         var temp = JsonUtility.FromJson<GameBoardCheckPoint>(boardStateAsJson);
         _lines = temp.GameBoardLines;
         Debug.Log(temp.GameBoardStyleState);
         _style.Decode(temp.GameBoardStyleState);
         Decode(temp.GameBoardState,temp.GameBoardElementsColorState,shapeFactory);;
         SetElementSize(temp.ElementSize);
     }

     private struct GameBoardCheckPoint
     {
         public int GameBoardLines;
         public float ElementSize;
         public string GameBoardState;
         public string GameBoardElementsColorState;
         public string GameBoardStyleState;
     }
     
}