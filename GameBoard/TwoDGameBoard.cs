using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine.UIElements;

public class TwoDGameBoard : GameBoard
{
    //Attributes
    private GameObject[,] _board;

    [SerializeField] private int width = 10;

    [SerializeField] private int height = 20;

    [SerializeField] private float squareSize = 1f;


    //methods
    private void Awake()
    {
        _board = new GameObject[height, width];
    }

    public override List<int> Check()
    {
        var lines = new List<int>();
        for (var i = 0; i < height; i++)
            if (CheckLine(ind: i))
                lines.Add(i);
        Lines += lines.Count;
        return lines;
    }

    public override void Clear(List<int> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            ClearLine(lines[i]);
            for (int j = i; j < lines.Count; j++)
                lines[j]--;
        }
    }

    public override bool ValidMove(Shape shape, Vector3 step, float angle)
    {
        for (int i = 0; i < shape.gameObject.transform.childCount; i++)
        {
            
            Vector3 matrixPos;
            if (angle == 0)
                matrixPos = WorldToMatrixPoint(shape.transform.GetChild(i).transform.position + step * squareSize);
            else
            {
                GameObject g = new GameObject();
                g.transform.position = shape.transform.GetChild(i).transform.position;
                g.transform.RotateAround(shape.rotatePoint + shape.transform.position, step, angle);
                Vector3 newPosition = g.transform.position;
                matrixPos = WorldToMatrixPoint(newPosition);
                Destroy(g);
            }
            
            if (matrixPos.x > width - 1 || matrixPos.x < 0 || matrixPos.y > height || matrixPos.y < 0)
                return false;
            if (matrixPos.y < height && matrixPos.x < width)
            {
                if (_board[(int) matrixPos.y, (int) matrixPos.x] != null)
                    return false;
            }
            
        }

        return true;
    }

    public override GameObject GetElement(params int[] cord)
    {
        return _board[cord[0], cord[1]];
    }

    public override int GetHeight()
    {
        return height;
    }
    
    public override float GetElementSize()
    {
        return squareSize;
    }

    public override void SetElementSize(float size)
    {
        squareSize = size;
    }

    public override void SetShape(Shape shape)
    {
        for (int i = 0; i < shape.gameObject.transform.childCount; i++)
        {
            Vector3 matrixPos = WorldToMatrixPoint(shape.gameObject.transform.GetChild(i).transform.position);
            _board[(int) matrixPos.y, (int) matrixPos.x] = shape.gameObject.transform.GetChild(i).gameObject;
        }
    }

    public override bool CheckAny(int level, bool onlyMiddleElements)
    {
        int bound = onlyMiddleElements ? ((width / 2) + 2) : width;
        for (int i = onlyMiddleElements ? ((width / 2) - 2) : 0; i < bound; i++)
            if (_board[level, i] != null)
                return true;
        return false;
    }

    public override string[] Encode([NotNull] Shape currShape)
    {
        var boardState = new StringBuilder(width * height);
        var boardColors = new StringBuilder(width * height);
        //make all board state element is empty and all board places color is Non
        for (int i = 0; i < width * height; i++)
        {
            boardState.Append('0');
            boardColors.Append('n');
        }

        //fill board state string and board colors
        for (int i = 0; i < currShape.gameObject.transform.childCount; i++)
        {
            var currChildMatrixPos = WorldToMatrixPoint(currShape.transform.GetChild(i).transform.position);
            if (currShape.transform.GetChild(i).position.y >= height * squareSize)
                continue;
            var currIndex = (int) (currChildMatrixPos.y * width + currChildMatrixPos.x);
            var currSpriteRenderer = currShape.transform.transform.GetChild(i).GetComponent<SpriteRenderer>();
            var currColorCode = GetColorsKeyByValue(currSpriteRenderer.color);
            boardState[currIndex] = '1';
            boardColors[currIndex] = currColorCode;
//            Debug.Log(currColorCode);
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (_board[i, j])
                {
                    boardState[i * width + j] = '1';
                    boardColors[i * width + j] = GetColorsKeyByValue(_board[i, j].GetComponent<SpriteRenderer>().color);
                }
            }
        }

        return new[] {boardState.ToString(), boardColors.ToString()};
    }

    public override string[] Encode()
    {
        var boardState = new StringBuilder(width * height);
        var boardColors = new StringBuilder(width * height);
        //make all board state element is empty and all board places color is Non
        for (int i = 0; i < width * height; i++)
        {
            boardState.Append('0');
            boardColors.Append('n');
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (_board[i, j])
                {
                    boardState[i * width + j] = '1';
                    boardColors[i * width + j] = GetColorsKeyByValue(_board[i, j].GetComponent<SpriteRenderer>().color);
                }
            }
        }
        return new[] {boardState.ToString(), boardColors.ToString()};
    }

    public override void Decode(string boardState, string boardColors, ShapeFactory shapeFactory)
    {
        Debug.Log(boardState.Length);
        for (int i = 0; i < height * width; i++)
        {
            int yPos = i / width;
            int xPos = i % width;

            if (_board[yPos, xPos])
            {
                Destroy(_board[yPos, xPos].gameObject);
            }

            if (boardState[i] == '1')
            {
                var block = shapeFactory.GetMainBlock("TwoD", _style.ToString());
                //  Debug.Log(yPos + ", " + xPos);
                block.transform.localScale = new Vector3(squareSize,squareSize,squareSize);
               // block.transform.localScale = new Vector3(0.355f, 0.355f, 0.355f);
                block.transform.position = MatrixToWorldPoint(new Vector3(xPos, yPos, 0));
                if (boardColors[i] != 'A' && boardColors[i] != 'n')
                    block.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ColorsMap[boardColors[i]];
                //Debug.Log(boardColors[i]);
                _board[i / width, i % width] = block.gameObject;
            }
        }
    }

    //helper methods
    private bool CheckLine(int ind)
    {
        for (var i = 0; i < width; i++)
            if (_board[ind, i] == null)
                return false;
        return true;
    }

    private void ClearLine(int ind)
    {
        for (int i = 0; i < width; i++)
        {
            if (_board[ind, i] != null)
            {
                Destroy(_board[ind, i]);
                _board[ind, i] = null;
            }
        }

        //FindObjectOfType<sceneManger>().BreackLine2dVFX(ind);
        for (int i = ind + 1; i < height; i++)
        {
            BringDownRow(i);
        }
    }

    private void BringDownRow(int ind)
    {
        for (int i = 0; i < width; i++)
        {
            GameObject currSquare = _board[ind, i];
            if (currSquare != null)
            {
                currSquare.transform.position += new Vector3(0, -squareSize, 0);
                _board[ind - 1, i] = currSquare;
                _board[ind, i] = null;
            }
        }
    }

    private Vector3 WorldToMatrixPoint(Vector3 worldPoint)
    {
        var matrixXPos = Mathf.RoundToInt(worldPoint.x / squareSize);
        var matrixYPos = Mathf.RoundToInt(worldPoint.y / squareSize);
        var matrixZPos = Mathf.RoundToInt(worldPoint.z / squareSize);
        var matrixPoint = new Vector3(matrixXPos, matrixYPos, matrixZPos);
        return matrixPoint;
    }

    private Vector3 MatrixToWorldPoint(Vector3 matrixPoint)
    {
        //  Vector3 resizedMatrixPoint = matrixPoint * 0.355f;
     //   float f = 0.355f;
     float f = squareSize;  
        var position = transform.position;
        var worldXPos = matrixPoint.x * f + position.x;
        var worldYPos = matrixPoint.y * f + position.y;
        var worldZPos = matrixPoint.z * f + position.z;
        var worldPoint = new Vector3(worldXPos, worldYPos, worldZPos);
        return worldPoint;
    }

    public void printBoard()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (_board[i, j] != null)
                {
                    Debug.Log("Row : " + i + "Col : " + j);
                }
            }
        }
    }

    public void printBoardEncode(string code)
    {
        Debug.Log("Start");
        for (int i = 0; i < height * width; i++)
        {
            if (code[i] == '1')
                Debug.Log(i + " : " + code[i]);
        }

        Debug.Log("Done");
    }
}