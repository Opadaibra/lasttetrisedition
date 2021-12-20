using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ThreeDGameBoard : GameBoard
{
    //Attributes
    public GameObject[,,] _board;

    [SerializeField] private int width = 5;

    [SerializeField] private int height = 10;

    [SerializeField] private int depth = 5;

    [SerializeField] private float cubeSize = 1.5f;

    //methods
    private void Awake()
    {
        _board = new GameObject[height, width, depth];
    }

    public override float GetElementSize()
    {
        return cubeSize;
    }

    public override void SetElementSize(float size)
    {
        cubeSize = size;
    }

    public override List<int> Check()
    {
        var plans = new List<int>();
        for (var i = 0; i < height; i++)
            if (CheckPlan(i))
                plans.Add(i);
        Lines += plans.Count;
        return plans;
    }

    public override void Clear(List<int> plans)
    {
        for (int i = 0; i < plans.Count; i++)
        {
            ClearPlan(i);
            for (int j = i; j < plans.Count; j++)
                plans[j]--;
        }
    }

    public override bool ValidMove(Shape shape, Vector3 step, float angle)
    {
        for (int i = 0; i < shape.gameObject.transform.childCount; i++)
        {
            Vector3 matrixPos;
            if (angle == 0)
            {
                matrixPos = WorldToMatrixPoint(shape.transform.GetChild(i).transform.position + step * cubeSize);
            }
            else
            {
                GameObject g = new GameObject();
                g.transform.position = shape.transform.GetChild(i).transform.position;
                g.transform.RotateAround(shape.rotatePoint + shape.transform.position, step, angle);
                Vector3 newPosition = g.transform.position;
                matrixPos = WorldToMatrixPoint(newPosition);
                Destroy(g);
            }

            if (matrixPos.x > width - 1 || matrixPos.x < 0  || matrixPos.y < 0 ||
                matrixPos.z > depth - 1 || matrixPos.z < 0)
                return false;
            if (matrixPos.y < height && matrixPos.x < width && matrixPos.z < depth)
            {
                if (_board[(int) matrixPos.y, (int) matrixPos.x, (int) matrixPos.z] != null)
                    return false;
            }
        }

        return true;
    }

    public override GameObject GetElement(params int[] cord)
    {
        return _board[cord[0], cord[1], cord[2]];
    }

    public override int GetHeight()
    {
        return height;
    }

    public override bool CheckAny(int level, bool onlyMiddleElements)
    {
        if (onlyMiddleElements)
            if (_board[level, width / 2, depth / 2])
                return true;
        for (int i = 0; i < depth; i++)
        for (int j = 0; j < width; j++)
            if (_board[(int) (level/cubeSize), j, i])
                return true;
        return false;
    }

    public override string[] Encode(Shape currShape)
    {
        var boardState = new StringBuilder(width * height * depth);
        var boardColors = new StringBuilder(width * height * depth);
        //make all board state element is empty and all board places color is Non
        for (int i = 0; i < width * height * depth; i++)
        {
            boardState.Append('0');
            boardColors.Append('n');
        }

        //fill board state string and board colors
        for (int i = 0; i < currShape.gameObject.transform.childCount; i++)
        {
            if (currShape.transform.GetChild(i).position.y >= height * cubeSize)
                continue;
            var currChildMatrixPos = WorldToMatrixPoint(currShape.transform.GetChild(i).transform.position);
            var currIndex = (int) (currChildMatrixPos.z * (width * height) + currChildMatrixPos.y * width +
                                   currChildMatrixPos.x);
            var currSpriteRenderer =
                currShape.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
            var currColorCode = GetColorsKeyByValue(currSpriteRenderer.color);
            boardState[currIndex] = '1';
            boardColors[currIndex] = currColorCode;
        }

        for (var i = 0; i < height; i++)
        {
            for (var k = 0; k < depth; k++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (!_board[i, j, k]) continue;
                    boardState[k * (width * height) + i * width + j] = '1';
                    boardColors[k * (width * height) + i * width + j] =
                        GetColorsKeyByValue(_board[i, j, k].transform.GetChild(0).GetComponent<SpriteRenderer>()
                            .color);
                }
            }
        }

        return new string[] {boardState.ToString(), boardColors.ToString()};
    }

    public override string[] Encode()
    {
        var boardState = new StringBuilder(width * height * depth);
        var boardColors = new StringBuilder(width * height * depth);
        //make all board state element is empty and all board places color is Non
        for (int i = 0; i < width * height * depth; i++)
        {
            boardState.Append('0');
            boardColors.Append('n');
        }
        for (var i = 0; i < height; i++)
        {
            for (var k = 0; k < depth; k++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (!_board[i, j, k]) continue;
                    boardState[k * (width * height) + i * width + j] = '1';
                    boardColors[k * (width * height) + i * width + j] =
                        GetColorsKeyByValue(_board[i, j, k].transform.GetChild(0).GetComponent<SpriteRenderer>()
                            .color);
                }
            }
        }

        return new string[] {boardState.ToString(), boardColors.ToString()};
    }

    public override void Decode(string boardState, string boardColors, ShapeFactory shapeFactory)
    {
        for (int i = 0; i < height * width * depth; i++)
        {
            int zPos = i / (width * height);
            int yPos = i % (width * height) / width;
            int xPos = i % (width * height) % width;

            if (_board[yPos, xPos, zPos])
            {
                Destroy(_board[yPos, xPos, zPos].gameObject);
            }

            if (boardState[i] == '1')
            {
                var block = shapeFactory.GetMainBlock("ThreeD", _style.ToString());
                block.transform.localScale = new Vector3(1f, 1f, 1f);
                block.transform.position = MatrixToWorldPoint(new Vector3(xPos, yPos, zPos));
                if (boardColors[i] != 'A' && boardColors[i] != 'n')
                {
                    Color c = ColorsMap[boardColors[i]];
                    block.ChangeColor(c);
                  /*  for(int k=0 ;k<6 ;k++)
                        block.transform.GetChild(k).GetComponent<SpriteRenderer>().color = new Color(c.r ,c.g ,c.b ,c.a);
                */}

                _board[yPos, xPos, zPos] = block.gameObject;
            }
        }
    }

    public override void SetShape(Shape shape)
    {
        for (int i = 0; i < shape.gameObject.transform.childCount; i++)
        {
            Vector3 matrixPos = WorldToMatrixPoint(shape.gameObject.transform.GetChild(i).transform.position);
            Debug.Log(matrixPos);
            _board[(int) (matrixPos.y), (int) (matrixPos.x), (int) (matrixPos.z)] =
                shape.gameObject.transform.GetChild(i).gameObject;
//            Debug.Log("On Set Shape : " + matrixPos);
        }

        /*  for (int i = 0; i < shape.transform.childCount; i++)
          {
              for (int j = 0; j < shape.transform.GetChild(i).childCount; j++)
                  shape.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = 3;
          }*/
    }

    //helper method
    private bool CheckPlan(int ind)
    {
        for (int j = 0; j < depth; j++)
        {
            for (int i = 0; i < width; i++)
                if (_board[ind, i, j] == null)
                    return false;
        }

        return true;
    }

    private void ClearPlan(int ind)
    {
        for (int j = 0; j < depth; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (_board[ind, i, j])
                {
                    Destroy(_board[ind, i, j]);
                    _board[ind, i, j] = null;
                }
            }
        }

        for (int i = ind + 1; i < height; i++)
        {
            BringPlanDown(i);
        }
    }

    private void BringPlanDown(int ind)
    {
        for (int j = 0; j < depth; j++)
        {
            for (int i = 0; i < width; i++)
            {
                GameObject currCube = _board[ind, i, j];
                if (currCube)
                    currCube.transform.Translate(0, -cubeSize, 0);
            }
        }
    }

    public Vector3 WorldToMatrixPoint(Vector3 worldPoint)
    {
        var matrixXPos = Mathf.RoundToInt(worldPoint.x / cubeSize);
        var matrixYPos = Mathf.RoundToInt(worldPoint.y / cubeSize);
        var matrixZPos = Mathf.RoundToInt(worldPoint.z / cubeSize);
        var matrixPoint = new Vector3(matrixXPos, matrixYPos, matrixZPos);
        return matrixPoint;
    }

    public void printBoard()
    {
        for (int k = 0; k < height; k++)

        for (int j = 0; j < depth; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (_board[k, i, j] != null)
                {
                    Debug.Log("height:" + k + "depth : " + j + "width : " + i);
                }
            }
        }
    }

    public void printBoardEncode(string code)
    {
        Debug.Log("Start");
        for (int i = 0; i < height * width * depth; i++)
        {
            if (code[i] == '1')
                Debug.Log(i + " : " + code[i]);
        }

        Debug.Log("Done");
    }

    private Vector3 MatrixToWorldPoint(Vector3 matrixPoint)
    {
        float f = 1f;
        var position = transform.position;
        var worldXPos = matrixPoint.x * f + position.x;
        var worldYPos = matrixPoint.y * f + position.y;
        var worldZPos = matrixPoint.z * f + position.z;
        var worldPoint = new Vector3(worldXPos, worldYPos, worldZPos);
        return worldPoint;
    }
}