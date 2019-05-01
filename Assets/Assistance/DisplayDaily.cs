using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDaily : MonoBehaviour
{

    public AssistanceControl.DailyIncome data;
    public AssistanceControl assistControl;
    public Text nameText;

    public void SetText(AssistanceControl.DailyIncome data, AssistanceControl assistanceControl)
    {
        nameText.text = data.name;
        assistControl = assistanceControl;
        this.data = data;
    }

    public void OnClickButton()
    {
        assistControl.WalletDetailOpen(data);
    }
}
