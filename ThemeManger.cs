using System;
using UnityEngine;
using UnityEngine.Sprites;
public class ThemeManger :MonoBehaviour
{
    public Sprite []mood;
    public static int  Indexformood=0;
    public void Dark()
    {
        Indexformood=0;
    }
    public void Light()
    {
        Indexformood = 1;
    }

    private void Start()
    {
        Indexformood = FindObjectOfType<GameSettings>().Theme;
        SwapBackground();
    }

    public void SwapBackground()
    {
        transform.GetComponent<UnityEngine.UI.Image>().sprite = mood[Indexformood];
    }   
}
