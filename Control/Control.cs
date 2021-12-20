using UnityEngine;

public abstract class Control 
{
    //override
    
    public abstract Vector3 MoveRight();
    public abstract Vector3 MoveLeft();
    public abstract Vector3 MoveDown();
    public abstract Vector3 MoveForward();
    public abstract Vector3 MoveBackward();
    public abstract Vector3 RotateX();
    public abstract Vector3 RotateY();
    public abstract Vector3 RotateZ();


}
