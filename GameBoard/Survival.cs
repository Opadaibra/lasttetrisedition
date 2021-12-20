using System;
using UnityEngine;

public class Survival:GameBoardStyle
{
    public override bool isEnd()
    {
        try
        {
            var highestLineIndex = gameBoard.GetHeight() - 1;
            var isItLastLineFilled = gameBoard.CheckAny(highestLineIndex, true)
                                     || gameBoard.CheckAny(highestLineIndex - 1, true);
            return isItLastLineFilled;
        }
        catch (Exception e)
        {
            Debug.Log("Exception in Survival isEnd method");
        }

        return false;
    }
    public override string ToString()
    {
        return "Survival";
    }
}