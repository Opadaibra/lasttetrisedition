using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DGame : ControlComposit
{
    public _2DGame(Control control)
    {
        this.control = control;
    }
    public override Vector3 MoveRight() { return control.MoveRight(); }
    public override Vector3 MoveLeft() { return control.MoveLeft(); }
    public override Vector3 MoveDown() { return control.MoveDown(); }
    public override Vector3 MoveForward() { return Vector3.zero; }
    public override Vector3 MoveBackward() { return Vector3.zero; }
    public override Vector3 RotateX() { return Vector3.zero; }
    public override Vector3 RotateY() { return Vector3.zero; }
    public override Vector3 RotateZ() { return control.RotateZ();}
 }
