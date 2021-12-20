using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGameManger : GameManger
{
    //only for test
    [SerializeField] private GameBoard another;
    private GameBoard another1;
    private GameBoard another2;
    private GameBoard another3;

    private GameBoard another4;

    private GameBoard threeDTest;
    // Start is called before the first frame update
    public void Restart()
    {
        //    SceneManager.LoadScene(MainMnue);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public override void UseItem(Item item)
    {
        //Real Code
        /*if (_player.IsAvilable(item))
        {
            item.logic(_gameBoard);
            _player.ReduceItem(item);
            //update number of item on screen
        }*/
        //for Test
        item.logic(gameBoard);
        UpdateShadowShapePosition();
    }

    public override void CheckBoard()
    {
        //make this abstract
        var lines = gameBoard.Check();
        var numberOfLines = lines.Count;
        gameBoard.Clear(lines);
        if (numberOfLines > 0)
            CalcScore(numberOfLines);
//        FindObjectOfType<sceneManger>().UpdateView();
        //calcCoins(numberOfLines);
        //Update number of Lines
    }

    private float StartTime;

    public override void Hold()
    {
        if (canHold)
        {
            Destroy(shadowShape.gameObject);
            holdShape.ResetRotate();
            currentShape.ResetRotate();
            canHold = false;
            Shape temp = holdShape;
            temp.ChangeColor(temp.shapeColor);
            var transform1 = currentShape.transform;
            var transform2 = holdShape.transform;
            transform1.position = transform2.position;
            transform1.localScale = transform2.localScale;
            holdShape = currentShape;
            //Reset Transform
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.transform.position = FindObjectOfType<ShapeFactory>().transform.position + temp.InstantiateDis;
            currentShape = temp;
            GenerateShadowShape(currentShape);
            UpdateShadowShapePosition();
            holdShape.ChangeColor(new Color(0, 0, 0, 125));
        }
    }

    void Start()
    {
     //   DontDestroyOnLoad(this);
     Debug.Log("View : " + game.View);
     
        ShapeFactory = Instantiate(ShapeFactory,ShapeFactoryPositionByView[view],Quaternion.identity);
        currentShape = ShapeFactory.GetShape(style, view);
        GenerateShadowShape(currentShape);
        UpdateShadowShapePosition();
      //  threeDTest = FindObjectOfType<GameBoardFactory>().GetGameBoard(View, Style);
      //  threeDTest.transform.position = new Vector3(0.1f,15,8);
      /*  GameBoard = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another.transform.position = new Vector3(10.47f,20.889f,0);
        another1 = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another1.transform.position = new Vector3(6.743f,20.889f,0);
        another2 = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another2.transform.position = new Vector3(3.02f,20.889f,0);
        another3 = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another3.transform.position = new Vector3(-0.71f,20.889f,0);
        another4 = FindObjectOfType<GameBoardFactory>().GetGameBoard(View ,Style);
        another4.transform.position = new Vector3(-4.44f,20.889f,0);

        //_fallSpeed = _speedByLevel[_game.DifficultyLevel];
        /*   _holdShape = _shapeFactory.getShape(_style, _view);
           var transform1 = _holdShape.transform;
           transform1.position = new Vector3(-2.31f, 16, -0.001f);
           transform1.localScale = new Vector3(0.45f, 0.45f, 0.45f);
           _holdShape.changeColor(new Color(0,0,0,125));
           _nextShapes = new Queue<Shape>(3);*/
        /*   for (var i = 0; i < _numberOfNextShapePerLevel[_game.DifficultyLevel]; i++)
           {
               Shape next = _shapeFactory.getShape(_style, _view);
               next.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
               next.changeColor(new Color(0,0,0,125));
               _nextShapes.Enqueue(next);
           }*/
        //for three d test 
        //  FindObjectOfType<sceneManger>().UpdateNextList();
        //   CheckLevel();
        StartTime = Time.time;
    }

    private float _lastTimeEncode;

    private void Update()
    {
        // if(Time.time-StartTime<4) return;
        if (!gameBoard.IsEnd())
        {
            if (Time.time - lastFall >= 0.4)
            {
                lastFall = Time.time;
                if (gameBoard.ValidMove(currentShape, Vector3.down, 0))
                {
                    currentShape.Move(Vector3.down);
                    UpdateShadowShapePosition();
                }
                else
                {
                   // Destroy(shadowShape.gameObject);
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
        }
     //   if(Input.GetKeyDown(KeyCode.K)) ((ThreeDGameBoard)GameBoard).printBoard();
       // if (Time.time - _lastTimeEncode >= 0.4f)
       // {
         //     var board = gameBoard.Encode(currentShape);
//              threeDTest.Decode(board[0],board[1],ShapeFactory);
            //  ((ThreeDGameBoard)GameBoard).printBoardEncode(board[0]);
            // var boardState = board[0];
            //var boardColors = board[1];
            /*    another.Decode(board[0],board[1],ShapeFactory);
                another1.Decode(board[0],board[1],ShapeFactory);
                another2.Decode(board[0],board[1],ShapeFactory);
                another3.Decode(board[0],board[1],ShapeFactory);
                another4.Decode(board[0],board[1],ShapeFactory);
              */ //((TwoDGameBoard)GameBoard).printBoardEncode(GameBoard.Encode(CurrentShape)[0]);
       // }
        else

        {
               Debug.Log("Loseeeeeee");
        }
    }
}