using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletDisplay : MonoBehaviour {

    public WalletControl.WalletData data;
    public WalletControl walletControl;
    public TMP_Text nameText;
    public TMP_Text amountText;
    public TMP_Text typeText;
    public Button button;
    public void SetText(WalletControl.WalletData data) {

        this.data = data;
        nameText.SetText(data.name);
        amountText.SetText(data.amount.ToString());
        if (data.type == 1)
        {
            typeText.SetText("WALLET");
        }
        else if (data.type == 2)
        {
            typeText.SetText("ACCOUNT");
        }
        else
        {
            typeText.SetText("CARD");
        }
    }

    public void OnClickButton()
    {
        walletControl.WalletDetailOpen(data);
    }
}
