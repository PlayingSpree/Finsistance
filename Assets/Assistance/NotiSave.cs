using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotiSave : MonoBehaviour
{
    public NotiControl notification;
    public Dropdown hour;
    public Dropdown min;
    public void preSave()
    {
        NotiControl.Noti data = new NotiControl.Noti(hour.value, min.value);
        notification.AddNotification(data);
        Clear();
    }

    public void Clear()
    { 
        hour.value = 0;
        min.value = 0;
    }
}
