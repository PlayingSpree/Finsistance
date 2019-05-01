using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NotiControl : MonoBehaviour
{
    private void SaveNoti(Noti data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveNoti.json");
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Save completed\n" + jsonData);
    }

    public void AddNotification(Noti data)
    {
        SaveNoti(data);
    }

    
    public class Noti
    {
        public int hour;
        public int min;
        public Noti(int hour ,int min)
        {
            this.hour = hour;
            this.min = min;
        }
    }
}
