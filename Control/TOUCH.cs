using UnityEngine;
public class TOUCH :Control
{
    private Touch _touch; 
    private float _swaptimestart;
    private float _swaptimesEnd;
    private float _swaptime;
    public override Vector3 MoveRight()
    {

   
        if (Input.touchCount == 1
            && Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > Mathf.Abs(Input.GetTouch(0).deltaPosition.y)
           && Input.GetTouch(0).deltaPosition.x > 0)
        {
            
            return Vector3.right;
        }
        else
            return Vector3.zero;
    }
    public override Vector3 MoveLeft()
    {
        if(Input.touchCount==1&&Mathf.Abs(Input.GetTouch(0).deltaPosition.x)>Mathf.Abs(Input.GetTouch(0).deltaPosition.y)
                            && Input.GetTouch(0).deltaPosition.x<0)
            return Vector3.left;
        else
            return Vector3.zero;
    }  
    public override Vector3 MoveForward() 
    {
        if(Input.touchCount==2&&Mathf.Abs(Input.GetTouch(1).deltaPosition.x)<Mathf.Abs(Input.GetTouch(1).deltaPosition.y)
                           && Input.GetTouch(1).deltaPosition.y >0&&Input.GetTouch(0).phase==TouchPhase.Stationary)
            return Vector3.forward;
        else
         return Vector3.zero;
    }
    public override Vector3 MoveBackward() 
    {
        if(Input.touchCount==2&&Mathf.Abs(Input.GetTouch(1).deltaPosition.x)<Mathf.Abs(Input.GetTouch(1).deltaPosition.y)
                              &&Input.GetTouch(0).phase==TouchPhase.Ended && Input.GetTouch(1).deltaPosition.y <0&&Input.GetTouch(0).phase==TouchPhase.Stationary)
            return Vector3.back;
        else
            return Vector3.zero;
    }
    public override Vector3 MoveDown()
    {

        if(Input.touchCount==1&&Mathf.Abs(Input.GetTouch(0).deltaPosition.x)<Mathf.Abs(Input.GetTouch(0).deltaPosition.y)
        && Input.GetTouch(0).deltaPosition.y<0)
            return Vector3.down;
        else
            return Vector3.zero;
    }
  
    public override Vector3 RotateX()
    {
        if(Input.touchCount==2&&Mathf.Abs(Input.GetTouch(1).deltaPosition.x)>Mathf.Abs(Input.GetTouch(1).deltaPosition.y)
                              &&Input.GetTouch(1).phase==TouchPhase.Ended
                              && Input.GetTouch(1).deltaPosition.x >0)
            return Vector3.left;
        else
            return Vector3.zero;
    }
    public override Vector3 RotateY() 
    {
        if(Input.touchCount==2&&Mathf.Abs(Input.GetTouch(1).deltaPosition.x)>Mathf.Abs(Input.GetTouch(1).deltaPosition.y)
                              &&Input.GetTouch(1).phase==TouchPhase.Ended
                              && Input.GetTouch(1).deltaPosition.x <0)
            return Vector3.up;
        else
            return Vector3.zero;
    }

    public override Vector3 RotateZ()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _swaptimestart = Time.time;
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                _swaptimesEnd = Time.time;
                _swaptime = _swaptimesEnd - _swaptimestart;
                if(_swaptime<0.2)
                {
                 
                        return Vector3.forward;
                }
            }
        }
        return Vector3.zero;
    }
    
}
