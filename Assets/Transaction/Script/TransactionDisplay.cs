using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransactionDisplay : MonoBehaviour
{
    public TransactionControl.TransactionData data;
    public TransactionControl transactionControl;
    public TMP_Text tagText;
    public TMP_Text amountText;
    public Button button;
    public Color color;
    public ColorBlock colorBlock;
    public GameObject transactionButton;

    public void SetText(TransactionControl.TransactionData data)
    {
        this.data = data;
        amountText.SetText(data.amount.ToString());
        
        if (data.type == 0)
        {
            button = transactionButton.GetComponent<Button>();
            colorBlock = button.colors;
            colorBlock.normalColor = new Color(0, 255, 0);
            button.colors = colorBlock;
            
        }
        else if (data.type == 1)
        {
            button = transactionButton.GetComponent<Button>();
            colorBlock = button.colors;
            colorBlock.normalColor = new Color(255, 0, 0);
            button.colors = colorBlock;
        }
        tagText.SetText(TagToString(data.type, data.tag));
        
    }

    public void OnClickButton()
    {
        transactionControl.TransactionDetailOpen(data);
    }
    public const int IncomeTagSize = 6;
    public const int OutcomeTagSize = 9;
    public static string TagToString(int type,int tag)
    {
        // 0 = income 1 = outcome
        // 0 food 1 drink 2 Transport 3 Shopping 4 Entertainment 5 Housing 6 Digital 7 Medical 8 Misc. (Misc. aka ETC)
        // 0 PocketMoney 1 Salary 2 Loan 3 Angpao 4 Refund 5 Misc.
        if (type == 1)
        {
            if (tag == 0)
            {
                return "Food";
            }
            else if (tag == 1)
            {
                return "Drink";
            }
            else if (tag == 2)
            {
                return "Transport";
            }
            else if (tag == 3)
            {
                return "Shopping";
            }
            else if (tag == 4)
            {
                return "Entertainment";
            }
            else if (tag == 5)
            {
                return "Housing";
            }
            else if (tag == 6)
            {
                return "Digital";
            }
            else if (tag == 7)
            {
                return "Medical";
            }
            else if (tag == 8)
            {
                return "Misc.";
            }
            return "None";
        }
        else
        {
            if (tag == 0)
            {
                return "Pocket Money";
            }
            else if (tag == 1)
            {
                return "Salary";
            }
            else if (tag == 2)
            {
                return "Loan";
            }
            else if (tag == 3)
            {
                return "Angpao";
            }
            else if (tag == 4)
            {
                return "Refund";
            }
            else if (tag == 5)
            {
                return "Misc.";
            }
            return "None";
        }
    }
}
