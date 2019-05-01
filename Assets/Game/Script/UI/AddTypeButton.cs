using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTypeButton : MonoBehaviour
{
    public AddScript addScript;
    public int type;

    public void SelectType()
    {
        addScript.PressTypeSelectButton(type);
    }
}
