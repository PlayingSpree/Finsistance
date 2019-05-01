using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TransactionControl : MonoBehaviour
{
    public DataToSaveList transactionDataList = new DataToSaveList();

    public Transform transactionButtonParent;
    public GameObject transactionButton;
    public List<GameObject> transactionButtonList = new List<GameObject>();

    public ShowTransactionDetail TransactionDetail;

    public Dropdown dropdown;
    List<string> dropdownList = new List<string>()
    {
        "Daily", "Weekly", "Monthly", "Annual"
    };

    public int showType;

    // Start is called before the first frame update
    private void Start()
    {

        SetDropdown();

        if (File.Exists(Path.Combine(Application.persistentDataPath, "TransactionData.json")))
            LoadData();

        /*
        DateTime currentDate = DateTime.Now;
        long time = currentDate.Ticks;
        AddData(time, 2, 1, 50);
        */
    }

    private void SaveData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "TransactionData.json");
        string jsonData = JsonUtility.ToJson(transactionDataList);
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "TransactionData.json");
        string jsonDataLoad = File.ReadAllText(filePath);
        this.transactionDataList = JsonUtility.FromJson<DataToSaveList>(jsonDataLoad);
        UpdateTransactionButton();
    }

    void UpdateTransactionButton()
    {
        foreach (GameObject item in transactionButtonList)
        {
            Destroy(item);
        }
        transactionButtonList.Clear();

        int currentMonth = DateTime.Now.Month;

        foreach (TransactionData item in transactionDataList.dataList)
        {
            DateTime load = new DateTime(item.date);
            long ticksSpan = DateTime.Now.Ticks - load.Ticks;
            var timeSpan = TimeSpan.FromTicks(ticksSpan);

            int days = (int)timeSpan.TotalDays;
            // Assuming 30 day months, rounded to nearest
            //int approximateMonths = (int)Math.Round(span.TotalDays / 30.0);

            //Debug.Log("Days Range is \n" + days);

            if (showType == 0 && load.Day == DateTime.Now.Day)
                CreateTransactionButtonObject(item);
            else if (showType == 1 && days <= 7)
            {
                CreateTransactionButtonObject(item);
            }
            else if (showType == 2 && days <= 30)
            {
                CreateTransactionButtonObject(item);
            }
            else if (showType == 3 && days <= 365)
            {
                CreateTransactionButtonObject(item);
            }
        }
    }

    void CreateTransactionButtonObject(TransactionData data)
    {
        TransactionDisplay t = Instantiate(transactionButton,transactionButtonParent).GetComponent<TransactionDisplay>();
        t.SetText(data);
        transactionButtonList.Add(t.gameObject);
        t.transactionControl = this;
    }
    public void AddData(long date, int type, int tag, int amount)
    {
        transactionDataList.dataList.Add(new TransactionData(date, type, tag, amount));
        SaveData();
        UpdateTransactionButton();
    }
    
    public void EditData(TransactionData data, int amount)
    {
        data.amount = amount;
        SaveData();
        UpdateTransactionButton();
    }
    
    public void TransactionDetailOpen(TransactionData data)
    {
        TransactionDetail.GetComponent<Canvas>().enabled = true;
        TransactionDetail.Load(data);
    }

    public void DeleteData(TransactionData data)
    {
        transactionDataList.dataList.Remove(data);
        TransactionDetail.GetComponent<Canvas>().enabled = false;
        SaveData();
        UpdateTransactionButton();

    }

    public void SetDropdown()
    {
        dropdown.ClearOptions();
        this.showType = 0;
        dropdown.AddOptions(dropdownList);
    }

    public void DropDownClickEvent(Dropdown change)
    {
        if (change.value == 0)
            this.showType = 0;
        else if (change.value == 1)
            this.showType = 1;
        else if (change.value == 2)
            this.showType = 2;
        else if (change.value == 3)
            this.showType = 3;

        UpdateTransactionButton();
    }

    [System.Serializable]
    public class DataToSaveList
    {
        public List<TransactionData> dataList = new List<TransactionData>();
    }
    [System.Serializable]
    public class TransactionData
    {
        public long date;
        public int type;        // 0 = income 1 = outcome
        public int tag;         // 0 food 1 drink 2 Transport 3 Shopping 4 Entertainment 5 Housing 6 Digital 7 Medical 8 Misc. (Misc. aka ETC)
                                // 0 Pocket Money 1 Salary 2 Loan 3 Angpao 4 Refund 5 Misc.
        public float amount;

        public TransactionData(long date, int type, int tag, float amount)
        {
            this.date = date;
            this.type = type;
            this.tag = tag;
            this.amount = amount;
        }
    }
}
