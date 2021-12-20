using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scriptes.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[Serializable]


public class MultiGame: Game
{
    
    private NetworkManger _networkManger;
    private void Start()
    {
        DontDestroyOnLoad(this);
        var currGameBoardFactory = Instantiate(_gameBoardFactory);
        gameBoard = currGameBoardFactory.GetGameBoard(view, style);
        gameManger = Instantiate(gameManger);
        currScore = 0;
        difficultyLevel = 0;
        
        //intialize game manger values
        gameManger.Game = this;
        gameManger.GameBoard = gameBoard;
        gameManger.View = view;
        gameManger.Style = style;
        gameManger.Player = player;
        ((MultiGameManger) gameManger).NetworkManger = _networkManger;
        ((MultiGameManger) gameManger).NetworkManger.PlayerId = player.GetNameWithSubId();
        //
        switch (view)
        {
            case "TwoD":
                gameManger.Control = new _2DGame(gameSetting.Control);
                break;
            case "ThreeD":
                gameManger.Control = new _3DGame(gameSetting.Control);
                break;
        }
        Destroy(currGameBoardFactory);
        
        
    }
    public NetworkManger NetworkManger
    {
        get => _networkManger;
        set => _networkManger = value;
    }

    // Update is called once per frame
    public void JoinToGame()
    {
        Debug.Log("join");
    }

    
}