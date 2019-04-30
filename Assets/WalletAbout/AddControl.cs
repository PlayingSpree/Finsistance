using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AddControl : MonoBehaviour
{
    public Canvas canvas ;
   
    public void Addbut()
    {
        if (canvas.enabled == false)
            canvas.enabled = true;
        else
            canvas.enabled = false;

        
    }
}
