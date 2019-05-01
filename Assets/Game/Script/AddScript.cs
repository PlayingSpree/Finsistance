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
    int selectedType;
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
        Debug.Log(selectedWallet+ "/"+ selectedType + "/" + float.Parse(inputField.text));
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
        selectedType = 0;
        image.color = new Color(0.835f, 1f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < 5; i++)
        {
            CreateTypeButton(i.ToString()+"I",i);
        }
    }
    public void PressOutcomeButton()
    {
        income = false;
        selectedType = 0;
        image.color = new Color(1f, 0.835f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < 6; i++)
        {
            CreateTypeButton(i.ToString()+"O",i);
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