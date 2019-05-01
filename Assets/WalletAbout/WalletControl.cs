using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalletControl : MonoBehaviour
{
    public DataToSaveList WalletDataList = new DataToSaveList();

    public Transform walletButtonParent;
    public GameObject walletButton;
    public List<GameObject> walletButtonList = new List<GameObject>();

    public ShowWalletDetail WalletDetail;

    // Start is called before the first frame update
    private void Start()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "WalletData.json")))
            LoadData();
    }

    private void SaveData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "WalletData.json");
        string jsonData = JsonUtility.ToJson(WalletDataList);
        File.WriteAllText(filePath, jsonData);
    }
    
    public void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "WalletData.json");
        string jsonDataLoad = File.ReadAllText(filePath);
        WalletDataList = JsonUtility.FromJson<DataToSaveList>(jsonDataLoad);
        UpdateWalletButton();
    }

    void UpdateWalletButton()
    {
        foreach (GameObject item in walletButtonList)
        {
            Destroy(item);
        }
        walletButtonList.Clear();
        foreach (WalletData item in WalletDataList.dataList)
        {
            CreateWalletButtonObject(item);
        }
    }

    void CreateWalletButtonObject(WalletData data)
    {
        WalletDisplay w = Instantiate(walletButton,walletButtonParent).GetComponent<WalletDisplay>();
        w.SetText(data);
        walletButtonList.Add(w.gameObject);
        w.walletControl = this;
    }

    public void AddData(int num,string name,string detail,float amount,DateTime date)
    {
        WalletDataList.dataList.Add(new WalletData(num,name,detail,amount, WalletDataList.idCount++));
        SaveData();
        UpdateWalletButton();
    }

    public void EditData(WalletData data,int num, string name, string detail, float amount, DateTime date)
    {
        data.name = name;
        data.detail = detail;
        data.amount = amount;
        SaveData();
        UpdateWalletButton();
    }

    public void WalletDetailOpen(WalletData data)
    {
        WalletDetail.GetComponent<Canvas>().enabled = true;
        WalletDetail.Load(data);
    }
    
    public void DeleteData(WalletData data)
    {
        WalletDataList.dataList.Remove(data);
        WalletDetail.GetComponent<Canvas>().enabled = false;
        SaveData();
        UpdateWalletButton();
    }

    [System.Serializable]
    public class DataToSaveList
    {
        public int idCount = 1;
        public List<WalletData> dataList = new List<WalletData>();
    }
    [System.Serializable]
    public class WalletData
    {
        public int type;   //1 wallet 2 account 3 card
        public string name;
        public string detail;
        public float amount;
        public int id;

        public WalletData(int type, string name, string detail, float amount, int id)
        {
            this.type = type;
            this.name = name;
            this.detail = detail;
            this.amount = amount;
            this.id = id;
        }
    }
}
