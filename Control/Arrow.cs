using UnityEngine;
using UnityEngine.UI;
public class Arrow : Control
{
    bool lbuttonclick = false, Rbuttonclick = false, bwbuttonclick=false,fwbuttonclicked=false,rotXbuttonclicked=false ,rotYbuttonclicked = false, rotZbuttonclicked = false,Dbuttonclick = false;
    private Button lbutton, Rbutton, Dbutton,bwbutton,fwbutton,rotXbutton,rotYbutton,rotZbutton;
     

        

  public  Arrow(Button[] buttons)
    {
        
     //  Button[] buttons = FindObjectsOfType<Button>();
       /* foreach (Button button in buttons)
        {
            if (button.name == "leftbutton")
            {
                lbutton = button;
                lbutton.onClick.AddListener(leftisclicked);
            }
            if (button.name == "Rightbutton")
            {
               
                Rbutton = button;
                Rbutton.onClick.AddListener(Rightisclicked);
            }
            if (button.name == "Backwordbutton")
            {
                bwbutton = button;
                bwbutton.onClick.AddListener(Backwordisclicked);
            }
            if (button.name == "Forwordbutton")
            {
                fwbutton = button;
                fwbutton.onClick.AddListener(forwordlisclicked);
            }
            if (button.name == "DownButton")
            {
                Dbutton = button;
                Dbutton.onClick.AddListener(Downisclicked);
            }
            if (button.name == "rotX")
            {
                rotXbutton = button;
                rotXbutton.onClick.AddListener(rotXisclicked);

            }
            if (button.name == "rotY")
            {
                rotYbutton = button;
                rotYbutton.onClick.AddListener(rotYisclicked);
            }
            if (button.name == "rotZ")
            {
                rotZbutton = button;
                rotZbutton.onClick.AddListener(rotZisclicked);
            }
        }*/
    }
    
    void leftisclicked()
    {
        //print("clicked");
        lbuttonclick = true;
    }
    void Rightisclicked()
    {

        Rbuttonclick = true;
    }
    void Downisclicked()
    {
        Dbuttonclick = true;
    }
    void Backwordisclicked()
    {
        bwbuttonclick = true;
    }
    void forwordlisclicked()
    {
        fwbuttonclicked=true;
    }
    void rotXisclicked()
    {
        rotXbuttonclicked = true;
    }
    void rotYisclicked()
    {
        rotYbuttonclicked = true;
    }
    void rotZisclicked()
    {
        rotZbuttonclicked = true;
    }
    public override Vector3 MoveRight()
    {
        if (Rbuttonclick==true)
        {
            Debug.Log("sad");
               Rbuttonclick = false;
            return Vector3.right;
        }
        else
        {
      
            return Vector3.zero;
        }
    } 
    public override Vector3 MoveLeft()
    {
        if (lbuttonclick)
         {
            lbuttonclick = false;
            return Vector3.left;
              }
        else
            return Vector3.zero;
    }

    public override Vector3 MoveDown()
    {
        if (Dbuttonclick)
        { 
            Dbuttonclick = false;
            return Vector3.down;
          }
        else
            return Vector3.zero;
    }
    public override Vector3 MoveForward()
    {
        if (fwbuttonclicked)
        {
            fwbuttonclicked = false;
            return Vector3.forward;
        }
        else
            return Vector3.zero;
    }
    public override Vector3 MoveBackward()
    {
        if (bwbuttonclick)
        {
            bwbuttonclick = false;
        return Vector3.back;
         }
        else
            return Vector3.zero;
    }
    public override Vector3 RotateX()
    {
        if (rotXbuttonclicked)
        {
            rotXbuttonclicked = false;
            return new Vector3(1, 0, 0);
        }
        else
            return Vector3.zero;
    }
    public override Vector3 RotateY()
    {
        if (rotYbuttonclicked)
        {
            rotYbuttonclicked = false;   
            return new Vector3(0, 1, 0);
        }
        else

            return Vector3.zero;
    }
    public override Vector3 RotateZ()
    {
        if (rotZbuttonclicked)
        {
            rotZbuttonclicked = false;
            return new Vector3(0, 0, 1);
        }
        else
            return Vector3.zero;

    }
}
