using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AssistanceControl : MonoBehaviour
{
    public DataToSaveList transactionDataList = new DataToSaveList();
    public GameObject dailyDetail;
    public Transform dailyButtonParent;
    public GameObject dailyButton;
    public Text dailyDetailName;
    public Text dailyDetailAmount;
    public List<GameObject> dailyButtonList = new List<GameObject>();

    private void SaveDaily()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveDaily.json");
        string jsonData = JsonUtility.ToJson(transactionDataList);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Save completed\n" + jsonData);
    }

    public void LoadDaily()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveDaily.json");
        string jsonDataLoad = File.ReadAllText(filePath);
        transactionDataList = JsonUtility.FromJson<DataToSaveList>(jsonDataLoad);
        Debug.Log("Data loaded\n" + jsonDataLoad);
        UpdateDailyButton();
    }

    void UpdateDailyButton()
    {
        foreach (GameObject item in dailyButtonList)
        {
            Destroy(item);
        }
        dailyButtonList.Clear();
        foreach (DailyIncome item in transactionDataList.dataList)
        {
            CreateDailyButtonObject(item);
        }
    }

    void CreateDailyButtonObject(DailyIncome data)
    {
        GameObject g = Instantiate(dailyButton, dailyButtonParent);
        dailyButtonList.Add(g);
        g.GetComponent<DisplayDaily>().SetText(data,this);
    }

    public void AddDailyIncome(DailyIncome data)
    {
        transactionDataList.dataList.Add(data);
        SaveDaily();
        UpdateDailyButton();
    }

    DailyIncome dailyIncomeToDelete;
    public void DeleteDailyIncome()
    {
        transactionDataList.dataList.Remove(dailyIncomeToDelete);
        SaveDaily();
        UpdateDailyButton();
    }

    public void WalletDetailOpen(DailyIncome data)
    {
        dailyDetail.SetActive(true);
        dailyIncomeToDelete = data;
        dailyDetailName.text = data.name;
        dailyDetailAmount.text = data.amount.ToString();
    }

    [System.Serializable]
    public class DataToSaveList
    {
        public List<DailyIncome> dataList = new List<DailyIncome>();
    }
    [System.Serializable]
    public class DailyIncome
    {
        public string name;
        public float amount;
        public bool isMonthly;
        public bool[] day;
        public int date;
        public int hour;
        public int minite;

        public DailyIncome(string name, float amount, bool isMonthly)
        {
            this.name = name;
            this.amount = amount;
            this.isMonthly = isMonthly;
        }
    }
}