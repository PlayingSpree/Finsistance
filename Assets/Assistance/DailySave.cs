using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class DailySave : MonoBehaviour
{
    public InputField nameIn;
    public InputField amount;
    public Button weekButton;
    public Toggle[] day;
    public Dropdown date;
    public Dropdown hour;
    public Dropdown minute; 
    public AssistanceControl control;
    public GameObject monthSelect;
 
    public void preSave()
    {
        int i = 0;
        bool button;
        if (monthSelect.activeSelf) button = true;
        else button = false;

        AssistanceControl.DailyIncome data = new AssistanceControl.DailyIncome(nameIn.text, float.Parse(amount.text), button);
        Debug.Log(monthSelect.activeSelf);
        if (button)
        {
            data.date = date.value;
            data.hour = hour.value;
            data.minite = minute.value;
        }
        else
        {
            bool[] day = { false, false, false, false, false, false, false };
            foreach (Toggle item in this.day)
            {
                i++;
                if (item.isOn) { day[i] = true; }
            }
            data.day = day;

        }
        control.AddDailyIncome(data);
    }

    public void Clear()
    {
        nameIn.text = "";
        amount.text = "";
        weekButton.Select();
        foreach (Toggle item in day)
        { 
            item.isOn = false;
        }
        date.value = 0;
        hour.value = 0;
        minute.value = 0;
    }
}
