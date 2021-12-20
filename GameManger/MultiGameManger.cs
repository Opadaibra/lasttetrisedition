using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptes.Messages;
using UnityEngine;

public class MultiGameManger : GameManger
{
    public NetworkManger NetworkManger
    {
        get => _networkManger;
        set => _networkManger = value;
    }

    public class Pair<T, TH>
    {
        public T first;
        public TH second;

        public Pair(T t, TH th)
        {
            this.first = t;
            this.second = th;
        }
    }

    [SerializeField] private GameBoardFactory _gameBoardFactory;
    private NetworkManger _networkManger;
    private Dictionary<string, Pair<GameBoard, int>> _playersStates;

    private static Dictionary<string, List<Vector3>> _playersBoardPositionByView =
        new Dictionary<string, List<Vector3>>()
        {
            {
                "TwoD", new List<Vector3>()
                {
                    new Vector3(7.27f, 20.05f, -0.001f), new Vector3(3.625f, 20.05f, -0.001f),
                    new Vector3(-0.06f, 20.05f, -0.001f)
                }
            },
            {
                "ThreeD", new List<Vector3>()
                {
                    new Vector3(-0.55f, 14f, 9.4f), new Vector3(-3.84f, 14f, 6.13f), new Vector3(-7.4f, 14f, 2.7f)
                }
            }
        };

    private static Dictionary<string, float> _elementSizeByView = new Dictionary<string, float>()
    {
        {"TwoD", 0.355f},
        {"ThreeD", 0.75f}
    };

    private Info _messageToSend;

    public override void UseItem(Item item)
    {
        /*if (_player.IsAvilable(item))
            {
                item.logic(_gameBoard);
                _player.ReduceItem(item);
                //update number of item on screen
            }*/
        //for Test
        item.logic(gameBoard);
        UpdateShadowShapePosition();
        //check if item is earth quake and add lines to message
    }

    public override void CheckBoard()
    {
        //make this abstract
        var lines = gameBoard.Check();
        var numberOfLines = lines.Count;
        // _messageToSend._desLine = lines.ToArray();
        gameBoard.Clear(lines);
        if (numberOfLines > 0)
            CalcScore(numberOfLines);
//        FindObjectOfType<SceneManger>().UpdateView();
    }

    public override void Hold()
    {
        // i can't bro
        throw new NotImplementedException();
    }

    private void Start()
    {
      //  DontDestroyOnLoad(this);
        _playerId = player.GetNameWithSubId();
        ShapeFactory = Instantiate(ShapeFactory, ShapeFactoryPositionByView[view], Quaternion.identity);
        currentShape = ShapeFactory.GetShape(style, view);
        GenerateShadowShape(currentShape);
        UpdateShadowShapePosition();
        _gameBoardFactory = Instantiate(_gameBoardFactory);
        //initialize and fill the palyers states map
        _playersStates = new Dictionary<string, Pair<GameBoard, int>>();
        StartGame startGameMessage = _networkManger.StartGameMessage;
        for (int i = 0; i < startGameMessage._maxPlayer-1; i++)
        {
           // sceneManger.SetPlayersBoardVisible(startGameMessage._maxPlayer-1);
            GameBoard currGameBoard =
                _gameBoardFactory.GetGameBoard(startGameMessage._boardView, startGameMessage._gameStyle);
            currGameBoard.transform.position = _playersBoardPositionByView[startGameMessage._boardView][i];
            currGameBoard.SetElementSize(_elementSizeByView[startGameMessage._boardView]);
            _playersStates.Add(startGameMessage._playersId[i], new Pair<GameBoard, int>(currGameBoard, 0));
        }

        //update score on screen
    }

    private string _playerId;
    private readonly float _timeToSimulate = 0.1f;
    private readonly float _timeToSend = 0.1f;
    private float _lastSimulate;
    private float _lastSend;

    private void Update()
    {
        if (!gameBoard.IsEnd())
        {
            if (Time.time - lastFall >= 0.4f)
            {
                lastFall = Time.time;
                if (gameBoard.ValidMove(currentShape, Vector3.down, 0))
                {
                    currentShape.Move(Vector3.down);
                    UpdateShadowShapePosition();
                }
                else
                {
                    Destroy(shadowShape.gameObject);
                    gameBoard.SetShape(currentShape);
                    if (!gameBoard.IsEnd())
                    {
                        if (!canHold)
                            canHold = true;
                        currentShape = GetNewShape();
                        GenerateShadowShape(currentShape);
                        UpdateShadowShapePosition();
                    }
                }
            }

            GetAction();
            CheckBoard();
            if (Time.time - _lastSend >= _timeToSend)
            {
                _lastSend = Time.time;
                string[] boardStateAndColorCode = gameBoard.Encode(currentShape);
                string boardStyleCode = gameBoard._style.Encode();
                Info mes = new Info();
                mes._currScore = game.CurrScore;
                mes._playerId = _playerId;
                mes._boardStateCode = boardStateAndColorCode[0];
                mes._boardElementsColorCode = boardStateAndColorCode[1];
                mes._boardStyleCode = boardStyleCode;
                Debug.Log("Style in send is : " + style);
                _networkManger.ToSend = mes;
                Debug.Log(mes.Encode());


            }

            if (Time.time - _lastSimulate >= _timeToSimulate)
            { 
                SimulatePlayersState();
            }
        }

        else
        {
            Debug.Log("Loseeeeeee");
        }
    }

    public void SimulatePlayersState()
    {
        foreach (var currPlayerId in _playersStates.Keys)
        {
            Info lastPlayerReceivedMessage = (Info) _networkManger.ReceivedBuffer[currPlayerId];
//            Debug.Log("reaceived buffer in Manger" + _networkManger.ReceivedBuffer.Values.ToArray()[0].Encode());
            if (lastPlayerReceivedMessage != null)
            {
                Debug.Log("to simulate the id :  " + currPlayerId);
                Debug.Log("curr messssagee : " + _networkManger.ReceivedBuffer[currPlayerId]);
                var lastBoardStateCode = lastPlayerReceivedMessage._boardStateCode;
                var lastBoardColorCode = lastPlayerReceivedMessage._boardElementsColorCode;
                var lastBoardStyleCode = lastPlayerReceivedMessage._boardStyleCode;
                Pair<GameBoard, int> state = _playersStates[currPlayerId];
                state.first.Decode(lastBoardStateCode, lastBoardColorCode, ShapeFactory);
                state.first._style.Decode(lastBoardStyleCode);
                state.second = lastPlayerReceivedMessage._currScore;
                //update score by scene mnager
            }
        }
    }
}