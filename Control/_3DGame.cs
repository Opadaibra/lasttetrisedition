using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3DGame : ControlComposit
{
    public _3DGame(Control control)
    {
        this.control = control;
    }
    public override Vector3 MoveRight() { return control.MoveRight(); }
    public override Vector3 MoveLeft() { return control.MoveLeft(); }
    public override Vector3 MoveDown() { return control.MoveDown(); }
    public override Vector3 MoveForward() { return control.MoveForward(); }
    public override Vector3 MoveBackward() { return control.MoveBackward(); }
    public override Vector3 RotateX() { return control.RotateX(); }
    public override Vector3 RotateY() { return control.RotateY(); }
    public override Vector3 RotateZ() { return control.RotateZ(); }
}
