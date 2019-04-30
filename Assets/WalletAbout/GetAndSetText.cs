using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAndSetText : MonoBehaviour
{
    public int type;
    public InputField namew;
    public InputField detail;
    public InputField amount;
    public WalletControl walletControl;
    Button button;
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setget()
    {
        walletControl.AddData(type, namew.text, detail.text, float.Parse(amount.text), System.DateTime.Now);
        namew.text = "";
        detail.text = "";
        amount.text = "";
    }

    public void SetCancel()
    {
        namew.text = "";
        detail.text = "";
        amount.text = "";
    }

    public void CheckInput()
    {
        if (namew.text.Trim() == "")
        {
            ShowError();
        }
        else if (amount.text == "")
        {
            ShowError();
        }
        else if (amount.text == "")
        {
            ShowError();
        }
        else
        {
            button.interactable = true;
        }
    }
    public void ShowError()
    {
        button.interactable = false;
    }
}
