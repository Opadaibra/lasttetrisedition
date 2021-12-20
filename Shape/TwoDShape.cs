using UnityEngine;

public class TwoDShape :Shape
{
    public override void ChangeColor(Color c)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).transform.GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,c.a);
    }

    public override void ChangeColor(Color c, int sortingOrder)
    {
        //1 is default
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteRenderer currSquareSpriteRenderer = transform.GetChild(i).transform.GetComponent<SpriteRenderer>();
            currSquareSpriteRenderer.sortingOrder = sortingOrder;
        }
        ChangeColor(c);
    }

    public override void ChangeTexture(Sprite s)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).transform.GetComponent<SpriteRenderer>().sprite = s;
    }


}