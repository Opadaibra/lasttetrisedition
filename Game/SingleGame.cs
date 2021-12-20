using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SingleGame : Game
{
    [SerializeField] private ShapeFactory loadShapeFactory;
    public string LoadedState;

    private void Start()
    {
        var currGameBoardFactory = Instantiate(_gameBoardFactory);
        gameManger = Instantiate(gameManger);
        if (string.IsNullOrEmpty(LoadedState))
        {
            
            gameBoard = Instantiate(_gameBoardFactory).GetGameBoard(view, style);
            Debug.Log("not loaded game");
            // initialize game values
            currScore = 0;
            difficultyLevel = 0;
            // initialize game manger values
           
        }
        else
        {
            SetGameStateAsJson(LoadedState);
        }
        gameManger.Game = this;
        gameManger.GameBoard = gameBoard;
        gameManger.View = view;
        //test
        //  gameManger.View = "TwoD";
        gameManger.Style = style;
        gameManger.Player = player;
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
    public bool DoesSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }

    private string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".sav");
    }


    //test for save and load
    public string GetGameStateAsJson()
    {
        var currGameState = new GameCheckPoint()
        {
            Style = style,
            View = view,
            CurrentScore = currScore,
            DifficultyLevel = difficultyLevel,
            GameBoardStateAsJson = gameBoard.GetBoardStateAsJson(),
           // GameMangerStateAsJson = "", //only for test
        };
        return JsonUtility.ToJson(currGameState);
    }

    public void SetGameStateAsJson(string gameStateAsJson)
    {
        Debug.Log("in Decode" + gameStateAsJson);
        var temp = JsonUtility.FromJson<GameCheckPoint>(gameStateAsJson);
        Debug.Log(temp.GameBoardStateAsJson);
        style = temp.Style;
        view = temp.View;
        currScore = temp.CurrentScore;
        difficultyLevel = temp.DifficultyLevel;
        gameBoard = Instantiate(_gameBoardFactory).GetGameBoard(view, style);
        ShapeFactory tempShapeFactory = Instantiate(loadShapeFactory);
        gameBoard.SetBoardStateAsJson(temp.GameBoardStateAsJson, tempShapeFactory);
        //set game manger state
        Destroy(tempShapeFactory.gameObject);
    }

    private struct GameCheckPoint
    {
        public string Style;
        public string View;
        public int CurrentScore;
        public int DifficultyLevel;
        public string GameBoardStateAsJson;
        public string GameMangerStateAsJson;
    }
}