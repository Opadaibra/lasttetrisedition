using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold_Controller : MonoBehaviour
{
    private void OnMouseDown()
    {
        FindObjectOfType<GameManger>().Hold();
    }
}
