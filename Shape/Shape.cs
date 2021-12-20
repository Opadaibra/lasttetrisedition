﻿using System;
using UnityEngine;
public abstract class Shape : MonoBehaviour
{
    

    [SerializeField] public Vector3 InstantiateDis = Vector3.zero;
    [SerializeField] public Vector3 rotatePoint;
    [SerializeField] public Color shapeColor;
    [SerializeField] private float SquareSize = 1;
    private Transform _transform;
    public abstract void ChangeColor(Color c);
    public abstract void ChangeColor(Color c,int sortingOrder);
    public abstract void ChangeTexture(Sprite s);
    public Vector3 GetPosition()
        {
            return transform.position;
        }
        public Quaternion GetRotation()
        {
            return transform.rotation ;
        }        
        public Vector3 GetScale()
        {
            return transform.localScale;
        }
        public void Move(Vector3 step)
        {
            transform.Translate(step*SquareSize);
        }
        public void Rotate(Vector3 cord, float angle)
        {
             for (int i = 0; i < transform.childCount; i++)
             {
                 transform.GetChild(i).RotateAround(rotatePoint+transform.position, cord, angle);
             }
        }
        public void ResetRotate()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Debug.Log("child " + i + transform.GetChild(i).eulerAngles);
               // transform.GetChild(i).eulerAngles = Vector3.zero;
               if(transform.GetChild(i).eulerAngles.x>0) 
                   transform.GetChild(i).RotateAround(rotatePoint+transform.position
                   ,new Vector3(1,0,0),-transform.GetChild(i).eulerAngles.x);
               if(transform.GetChild(i).eulerAngles.y>0) 
                   transform.GetChild(i).RotateAround(rotatePoint+transform.position
                   ,new Vector3(0,1,0),-transform.GetChild(i).eulerAngles.y);
               if(transform.GetChild(i).eulerAngles.z>0) 
                   transform.GetChild(i).RotateAround(rotatePoint+transform.position
                   ,new Vector3(0,0,1),-transform.GetChild(i).eulerAngles.z);
            }
        }

        public Shape CreatCopy()
        {
            var position = transform.position;
            Shape shape = Instantiate(this , position , gameObject.transform.rotation);
            shape.transform.position = position;
            return shape;
        }
}
    