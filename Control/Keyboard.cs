using UnityEngine;

public class Keyboard : Control
{
    public override Vector3 MoveRight()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Vector3.right;
        }
        else
            return Vector3.zero;

    }
    public override  Vector3 MoveLeft() 
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        
            return Vector3.left;
        else
            return Vector3.zero;
    }
 
    public override Vector3 MoveDown()
    {
        if (Input.GetKey(KeyCode.DownArrow))

            return Vector3.down;
        else
            return Vector3.zero;
    }
    public override Vector3 MoveForward() 
    {
        if (Input.GetKeyDown(KeyCode.W))

            return Vector3.forward;
        else
            return Vector3.zero;
    }
    public override Vector3 MoveBackward()
    {
        if (Input.GetKeyDown(KeyCode.S))

            return Vector3.back;
        else
            return Vector3.zero;
    }
    public override Vector3 RotateX() {
        if (Input.GetKeyDown(KeyCode.D))
        return new Vector3(1, 0, 0);
        else
            return Vector3.zero;
    }
    public override Vector3 RotateY() {
        if (Input.GetKeyDown(KeyCode.A))
            return new Vector3(0, 1, 0);
        else

            return Vector3.zero;
    }
    public override Vector3 RotateZ() 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return new Vector3(0, 0, 1);
        else
            return Vector3.zero;
    
    }
 
}
