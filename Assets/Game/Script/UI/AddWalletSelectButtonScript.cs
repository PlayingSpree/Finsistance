using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddWalletSelectButtonScript : MonoBehaviour
{
    public AddScript addScript;
    public WalletControl.WalletData wallet;
    public TMP_Text walletName;
    public TMP_Text walletType;
    public TMP_Text walletBalance;

    public void SetText(WalletControl.WalletData wallet, AddScript addScript)
    {
        this.addScript = addScript;
        this.wallet = wallet;
        walletName.SetText(wallet.name);
        if (wallet.type == 1)
        {
            walletType.SetText("Wallet");
        }
        else if (wallet.type == 2)
        {
            walletType.SetText("Account");
        }
        else if(wallet.type == 3)
        {
            walletType.SetText("Card");
        }
        else
        {
            walletType.SetText("Unknown");
        }
        walletBalance.SetText(wallet.amount.ToString("F"));
    }

    public void SelectWallet()
    {
        addScript.PressWalletSelectButton(wallet);
    }
}
