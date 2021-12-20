using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    [SerializeField] protected GameBoardFactory _gameBoardFactory;
    [SerializeField]protected Player player;
    [SerializeField]protected GameManger gameManger;
    [SerializeField]protected int currScore;
    [SerializeField]protected int difficultyLevel;
    [SerializeField]protected GameBoard gameBoard;
    [SerializeField]protected GameSettings gameSetting;
    [SerializeField]protected SceneManger sceneManger;

    public SceneManger SceneManger
    {
        get => sceneManger;
        set => sceneManger = value;
    }

    [SerializeField]protected string view;
    [SerializeField]protected string style;
    public GameSettings GameSetting
    {
        get { return gameSetting; }
        set { gameSetting = value; }
    }

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    public GameManger GameManger
    {
        get { return gameManger; }
        set { gameManger = value; }
    }

    public int CurrScore
    {
        get { return currScore; }
        set { currScore = value; }
    }

    public int DifficultyLevel
    {
        get { return difficultyLevel; }
        set { difficultyLevel = value; }
    }

    public GameBoard GameBoard
    {
        get { return gameBoard; }
        set { gameBoard = value; }
    }
    
    public string View
    {
        get { return view; }
        set { view = value; }
    }

    public string Style
    {
        get { return style; }
        set { style = value; }
    }

    /*   private void Start()
       {
           DontDestroyOnLoad(this);
       }
   
       public void setGameBoard()
       {
           if (View1 != "" && Style1 != "")
               gameBoard = FindObjectOfType<GameBoardFactory>().GetGameBoard(View1, Style1);
       }*/
}