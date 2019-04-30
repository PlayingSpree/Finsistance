using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowWalletDetail : MonoBehaviour
{
    public WalletControl.WalletData wallet;
    public Text type;
    public InputField namew;
    public InputField detail;
    public InputField amount;
    public WalletControl walletControl;
    Button button;
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Load(WalletControl.WalletData data)
    {
        wallet = data;
        namew.text = data.name;
        amount.text = data.amount.ToString("F");
        detail.text = data.detail;
        if (data.type == 1)
        {
            type.text = "WALLET";
        }
        else if (data.type == 2)
        {
            type.text = "ACCOUNT";
        }
        else
        {
            type.text = "Card";
        }
    }

    public void Save()
    {
        walletControl.EditData(wallet, wallet.type, namew.text, detail.text, float.Parse(amount.text), System.DateTime.Now);
        namew.text = "";
        detail.text = "";
        amount.text = "";
        GetComponent<Canvas>().enabled = false;
    }

    public void CheckInput()
    {
        if (namew.text == "")
        {
            ShowError();
        }
        else if (detail.text == "")
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
    public void Delete()
    {
        walletControl.DeleteData(wallet);
    }
}
