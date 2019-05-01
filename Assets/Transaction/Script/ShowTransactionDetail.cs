using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTransactionDetail : MonoBehaviour
{
    public TransactionControl.TransactionData transaction;
    public TMP_Text typeText;
    public TMP_Text tagText;
    public TMP_InputField amountText;
    public TransactionControl transactionControl;
    Button button;

    // Start is called before the first frame update
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Load(TransactionControl.TransactionData data)
    {
        transaction = data;
        amountText.text = data.amount.ToString("F");
        if (data.type == 1)
        {
            typeText.text = "Income";
        }
        else if (data.type == 2)
        {
            typeText.text = "Outcome";
        }

        if (data.tag == 1)
        {
            tagText.SetText("Food");
        }
        else if (data.tag == 2)
        {
            tagText.SetText("Drink");
        }
    }

    public void Save()
    {
        if (amountText.text.Substring(0,1) == "-")
        {
            amountText.text = "Enter Positive Number";
        }
        else
        {
            transactionControl.EditData(transaction, int.Parse(amountText.text));
            amountText.text = "";
            GetComponent<Canvas>().enabled = false;
        }

    }

    public void CheckInput()
    {
        if (amountText.text == "")
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
    public void Delete()
    {
        transactionControl.DeleteData(transaction);
    }
}
