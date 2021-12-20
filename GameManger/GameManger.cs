using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManger : MonoBehaviour
{
    public Shape CurrentShape
    {
        get => currentShape;
        set => currentShape = value;
    }

    public Shape ShadowShape
    {
        get => shadowShape;
        set => shadowShape = value;
    }

    public Queue<Shape> NextShapes
    {
        get => nextShapes;
        set => nextShapes = value;
    }

    public Shape HoldShape
    {
        get => holdShape;
        set => holdShape = value;
    }

    public GameBoard GameBoard
    {
        get => gameBoard;
        set => gameBoard = value;
    }

    public string View
    {
        get => view;
        set => view = value;
    }

    public string Style
    {
        get => style;
        set => style = value;
    }

    public Control Control
    {
        get => control;
        set => control = value;
    }

    public float FallSpeed
    {
        get => fallSpeed;
        set => fallSpeed = value;
    }

    public Player Player
    {
        get => player;
        set => player = value;
    }

    public float TempScore
    {
        get => _tempScore;
        set => _tempScore = value;
    }

    public Game Game
    {
        get => game;
        set => game = value;
    }

    public bool CanHold
    {
        get => canHold;
        set => canHold = value;
    }

    public SceneManger SceneManger
    {
        get => sceneManger;
        set => sceneManger = value;
    }

    public float LastFall
    {
        get => lastFall;
        set => lastFall = value;
    }

    public float LastInputCheck
    {
        get => lastInputCheck;
        set => lastInputCheck = value;
    }

    public float[] SpeedByLevel
    {
        get => speedByLevel;
        set => speedByLevel = value;
    }

    public int[] NumberOfnextShapePerLevel
    {
        get => numberOfnextShapePerLevel;
        set => numberOfnextShapePerLevel = value;
    }

    public int[] ScorePerLinePerLevel
    {
        get => scorePerLinePerLevel;
        set => scorePerLinePerLevel = value;
    }

    [SerializeField] protected ShapeFactory ShapeFactory;
    
    //Attributes

    protected Shape currentShape;

    protected Shape shadowShape;

    protected Queue<Shape> nextShapes;

    protected Shape holdShape;

    protected GameBoard gameBoard;
    
    protected string view;

    protected string style;

    [SerializeField]protected Control control;

    protected float fallSpeed;

    protected Player player;

    private float _tempScore;

    protected Game game;

    protected bool canHold = true;

    protected  SceneManger sceneManger;
    
    //config parameters

    protected float lastFall = 0;

    protected float lastInputCheck = 0;

    protected float[] speedByLevel =  {1.2f, 0.9f, 0.5f};

    protected int[] numberOfnextShapePerLevel =  {3, 2, 1};

    protected int[] scorePerLinePerLevel =  {50, 100, 200};

    protected static Dictionary<string, Vector3> ShapeFactoryPositionByView =new Dictionary<string, Vector3>() 
    {
        {"ThreeD", new Vector3(2.25f,15,2.25f)},
        {"TwoD" , new Vector3(4.5f,20,-0.001f)}
    };
    //Properties
    //Methods

    public void GetAction()
    {
        Vector3 step;
        //Rotation
        if ((step = control.RotateX()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, -90))
            {
                currentShape.Rotate(step, -90f);
                shadowShape.Rotate(step, -90f);
                UpdateShadowShapePosition();
            }
        }
        else if ((step = control.RotateY()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, -90))
            {
                currentShape.Rotate(step, -90f);
                shadowShape.Rotate(step, -90f);
                UpdateShadowShapePosition();
            }
        }

        //Moving
        if ((step = control.MoveRight()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, 0))
            {
                if (Time.time - lastInputCheck >= 0.15f)
                {
                    lastInputCheck = Time.time;
                    currentShape.Move(step);
                    UpdateShadowShapePosition();
                }
            }
        }
        else if ((step = control.MoveLeft()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, 0))
            {
                if (Time.time - lastInputCheck >= 0.15f)
                {
                    lastInputCheck = Time.time;
                    currentShape.Move(step);
                    UpdateShadowShapePosition();
                }
            }
        }
        else if ((step = control.MoveDown()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, 0))
            {
                if (Time.time - lastInputCheck >= 0.03f)
                {
                    lastInputCheck = Time.time;
                    currentShape.Move(step);
                    UpdateShadowShapePosition();
                }
            }
        }
        else if ((step = control.MoveBackward()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, 0))
            {
                if (Time.time - lastInputCheck >= 0.1f)
                {
                    lastInputCheck = Time.time;
                    currentShape.Move(step);
                    UpdateShadowShapePosition();
                }
            }
        }
        else if ((step = control.MoveForward()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, 0))
            {
                if (Time.time - lastInputCheck >= 0.1f)
                {
                    lastInputCheck = Time.time;
                    currentShape.Move(step);
                    UpdateShadowShapePosition();
                }
            }
        }
        else if ((step = control.RotateZ()) != Vector3.zero)
        {
            if (gameBoard.ValidMove(currentShape, step, -90))
            {
                currentShape.Rotate(step, -90f);
                shadowShape.Rotate(step, -90f);
                UpdateShadowShapePosition();
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    public abstract void UseItem(Item item);

    public abstract void CheckBoard();

    public abstract void Hold();

    public void CalcScore(int numberOfLines)
    {
        //_game.CurrScore += numberOfLines * _scorePerLinePerLevel[_game.DifficultyLevel];
        //Mathf.RoundToInt((numberOfLines - 1) * 0.25f * numberOfLines * _scorePerLinePerLevel[_game.DifficultyLevel] +  _scorePerLinePerLevel[_game.DifficultyLevel] * numberOfLines) ;
        //_tempScore += ((numberOfLines - 1) * 0.25f) * numberOfLines *  _scorePerLinePerLevel[_game.DifficultyLevel] +  _scorePerLinePerLevel[_game.DifficultyLevel] * numberOfLines;
        //CheckLevel();
        //FindObjectOfType<sceneManger>().UpdateView();
    }

    public void CalcCoins()
    {
        if (_tempScore >= 1000)
        {
            player.Coins++;
            //Update number of coins
            _tempScore -= 1000;
        }
    }

    public Shape GetNewShape()
    {
     //   AudioSource.PlayClipAtPoint();
        //for three d test
        Destroy(shadowShape.gameObject);
        return ShapeFactory.GetShape(style, view);
        //Destroy Shadow Shape 
        //Get the new current shape from _nextShapes Queue and repositioning it.....
        var newShape = nextShapes.Dequeue();
        newShape.transform.position = ShapeFactory.transform.position + newShape.InstantiateDis;
        newShape.transform.localScale = new Vector3(1, 1, 1);
        //Reset his color
        newShape.ChangeColor(newShape.shapeColor);
        //Add new Shape to _nextShapes Queue
        if (nextShapes.Count < numberOfnextShapePerLevel[game.DifficultyLevel])
        {
            Shape toAdd = ShapeFactory.GetShape(style, view);
            toAdd.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            toAdd.ChangeColor(new Color(0, 0, 0, 125));
            nextShapes.Enqueue(toAdd);
        }
        //Update _nextShapes View on screen
        FindObjectOfType<SceneManger>().UpdateNextList();
        return newShape;
    }


    public void CheckLevel()
    {
        if (game.CurrScore >= 0 && game.CurrScore < 3000)
        {
            game.DifficultyLevel = 0;
            fallSpeed = 1.2f;
        }
        else if (game.CurrScore >= 3000 && game.CurrScore < 7500)
        {
            game.DifficultyLevel = 1;
            Queue<Shape> temp = new Queue<Shape>();
            temp.Enqueue(nextShapes.Dequeue());
            temp.Enqueue(nextShapes.Dequeue());
            Destroy(nextShapes.Dequeue().gameObject);
            nextShapes = temp;
            fallSpeed = 0.9f;
        }

        if (game.CurrScore >= 7500)
        {
            canHold = false;
            game.DifficultyLevel = 2;
            Queue<Shape> temp = new Queue<Shape>();
            temp.Enqueue(nextShapes.Dequeue());
            Destroy(nextShapes.Dequeue().gameObject);
            nextShapes = temp;
            fallSpeed = 0.4f;
            if (game.CurrScore >= 1000)
            {
                fallSpeed = 0.6f;
            }
        }

        //FindObjectOfType<SceneManger>().UpdateNextList();
        //FindObjectOfType<sceneManger>().UpdateView();
    }
    protected void GenerateShadowShape(Shape shape)
    {
        Shape s = shape.CreatCopy();
        shadowShape = s;
        s.ChangeColor(new Color32(0,0,0,75),4);
    }

    protected void UpdateShadowShapePosition()
    {
        shadowShape.gameObject.transform.position = currentShape.gameObject.transform.position;
        for (float i = Mathf.RoundToInt(shadowShape.transform.position.y); i >= 0; i = i - 0.75f)
            if (this.gameBoard.ValidMove(shadowShape, Vector3.down, 0))
            {
                shadowShape.Move(Vector3.down);
            }
    }
}