using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class AddScript : MonoBehaviour
{
    public Transform typeButtonParent;
    public GameObject typeButton;
    public TMP_InputField inputField;
    public Image image;
    public TMP_Text walletType;
    public TMP_Text walletName;
    public GameObject walletObject;
    public Transform walletObjectParent;
    public GameObject walletPanel;
    public Button saveButton;
    WalletControl.WalletData selectedWallet;
    WalletControl.DataToSaveList walletDataList;
    int selectedType = -1;
    bool income = false;
    List<GameObject> typeButtonObject = new List<GameObject>();
    public void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "WalletData.json");
        string jsonDataLoad = File.ReadAllText(filePath);
        walletDataList = JsonUtility.FromJson<WalletControl.DataToSaveList>(jsonDataLoad);
        foreach (WalletControl.WalletData item in walletDataList.dataList)
        {
            AddWalletSelectButtonScript s = Instantiate(walletObject, walletObjectParent).GetComponent<AddWalletSelectButtonScript>();
            s.SetText(item,this);
        }
        ClearField();
    }
    public void PressAddButton()
    {
        //Update Transaction
        TransactionControl.DataToSaveList transactionDataList = new TransactionControl.DataToSaveList();
        string filePath = Path.Combine(Application.persistentDataPath, "TransactionData.json");
        if (File.Exists(filePath))
        {
            string jsonDataLoad = File.ReadAllText(filePath);
            transactionDataList = JsonUtility.FromJson<TransactionControl.DataToSaveList>(jsonDataLoad);
        }

        transactionDataList.dataList.Add(new TransactionControl.TransactionData(System.DateTime.Now.Ticks, income?0:1, selectedType, float.Parse(inputField.text)));

        string jsonData = JsonUtility.ToJson(transactionDataList);
        File.WriteAllText(filePath, jsonData);
        //Update Wallet
        if (selectedWallet != null) {
            WalletControl.DataToSaveList walletDataList = new WalletControl.DataToSaveList();
            filePath = Path.Combine(Application.persistentDataPath, "WalletData.json");
            string jsonDataLoad = File.ReadAllText(filePath);
            walletDataList = JsonUtility.FromJson<WalletControl.DataToSaveList>(jsonDataLoad);

            walletDataList.dataList.Find(x => x.id == selectedWallet.id).amount += income ? float.Parse(inputField.text) : -float.Parse(inputField.text);

            jsonData = JsonUtility.ToJson(walletDataList);
            File.WriteAllText(filePath, jsonData);
        }
        ClearField();
    }
    public void PressCancleButton()
    {
        ClearField();
    }

    void ClearField()
    {
        income = false;
        inputField.text = "";
        image.color = new Color(1f, 0.835f, 0.835f);
        saveButton.interactable = false;
        PressOutcomeButton();
        PressWalletSelectNoneButton();
    }

    public void PressIncomeButton()
    {
        income = true;
        selectedType = -1;
        image.color = new Color(0.835f, 1f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < TransactionDisplay.IncomeTagSize; i++)
        {
            CreateTypeButton(TransactionDisplay.TagToString(0,i),i);
        }
    }
    public void PressOutcomeButton()
    {
        income = false;
        selectedType = -1;
        image.color = new Color(1f, 0.835f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < TransactionDisplay.OutcomeTagSize; i++)
        {
            CreateTypeButton(TransactionDisplay.TagToString(1, i), i);
        }
    }

    public void PressWalletSelectButton(WalletControl.WalletData wallet)
    {
        selectedWallet = wallet;
        walletPanel.SetActive(false);
        walletName.SetText(wallet.name);
        if (wallet.type == 1)
        {
            walletType.SetText("Wallet");
        }
        else if (wallet.type == 2)
        {
            walletType.SetText("Account");
        }
        else if (wallet.type == 3)
        {
            walletType.SetText("Card");
        }
        else
        {
            walletType.SetText("Unknown");
        }
    }

    public void PressWalletSelectNoneButton()
    {
        selectedWallet = null;
        walletType.SetText("Select");
        walletName.SetText("Wallet");
    }
    public void PressTypeSelectButton(int type)
    {
        selectedType = type;
    }

    void CreateTypeButton(string text,int type)
    {
        GameObject g = Instantiate(typeButton, typeButtonParent);
        typeButtonObject.Add(g);
        TMP_Text t = g.GetComponentInChildren<TMP_Text>();
        t.SetText(text);
        AddTypeButton a = g.GetComponent<AddTypeButton>();
        a.type = type;
        a.addScript = this;
    }
    public void CheckInput()
    {
        saveButton.interactable = false;
        float f;
        if (inputField.text != "" && float.TryParse(inputField.text,out f))
        {
            if(f > 0f)
            {
                saveButton.interactable = true;
            }
        }
    }
    void RemoveAllObjectFromList(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        list.Clear();
    }
}