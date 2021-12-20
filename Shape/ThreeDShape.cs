using UnityEngine;

public class ThreeDShape : Shape
{
    public override void ChangeColor(Color c)
    {
        for (int i = 0; i < transform.childCount; i++)
            for(int j=0;j<transform.GetChild(i).childCount;j++) 
                transform.GetChild(i).GetChild(j).transform.GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,c.a);
    }

    public override void ChangeColor(Color c, int sortingOrder)
    {
        //default sorting 4
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                SpriteRenderer currSquareSpriteRenderer =
                    transform.GetChild(i).GetChild(j).transform.GetComponent<SpriteRenderer>();
                currSquareSpriteRenderer.sortingOrder = sortingOrder;
            }
        }
        ChangeColor(c);
    }

    public override void ChangeTexture(Sprite s)
    { 
        for (int i = 0; i < transform.childCount; i++) 
            for (int j = 0; j < transform.GetChild(i).childCount; j++) 
                 transform.GetChild(i).GetChild(j).transform.GetComponent<SpriteRenderer>().sprite = s;
    }

   /* public  override Shape creatCopy()
    {
        ThreeDShape shape = Instantiate(this , transform.position , gameObject.transform.rotation);
        shape.transform.position = this.transform.position;
        return shape;
    }*/
}