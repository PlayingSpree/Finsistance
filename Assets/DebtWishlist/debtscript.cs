using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class debtscript : MonoBehaviour
{
    public InputField inputname;
    public InputField inputcost;
    public Text nametext;
    public Text costtext;
    public Button saveit;
    public Button addebtback;
    public Button back;
    public Button debtadd;
    public Button paydebt;
    public DebtToSave debt = new DebtToSave();
    List<string> dropdownlist = new List<string>(){};
    public Dropdown dropdown1;
    public Canvas page1;
    public Canvas page2;

    public void Start()
    {
        page1.enabled = true;
        page2.enabled = false;
        dropdownlistupdate();
    }

    public void gotoaddpage() {
        page1.enabled = false;
        page2.enabled = true;
    }

    public void backtodebtscene() {
        page1.enabled = true;
        page2.enabled = false;
    }


    public void SaveData()//เซฟดาต้า
    {
        string filePath = Path.Combine(Application.persistentDataPath, "DebtData.json"); // Path + File Name
        string jsonData = JsonUtility.ToJson(debt); // Object -> json
        File.WriteAllText(filePath, jsonData); // json -> File

    }

    public void presssave()//กดเซฟ
    {
        if (inputname.text != "" && inputcost.text != "" && inputcost.text.Substring(0, 1) != "-")
        {
            debt.debtlist.Add(new DebtToSaveClass(inputname.text, inputcost.text));
            SaveData();
            updatedebtlist();
            inputname.text = "";
            inputcost.text = "";
        }
        else {
            saveit.interactable = false;

            inputname.text = "";
            inputcost.text = "";
            saveit.interactable = true;

        }
        
    }

    public void updatedebtlist()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "DebtData.json"); // Path + File Name
        if (File.Exists(filePath))
        {
            string loadData = File.ReadAllText(filePath); // File -> json
            debt = JsonUtility.FromJson<DebtToSave>(loadData); // json -> Object 
        }

        dropdownlist.Clear();
        foreach (DebtToSaveClass item in debt.debtlist)
        {
            dropdownlist.Add(item.savename);
        }
        dropdownlistupdate();
    }
    public void dropdownlistupdate()
    {
        dropdown1.ClearOptions();
        dropdown1.AddOptions(dropdownlist);
        if (dropdown1.options.Count != 0)
        {
            nametext.text = debt.debtlist[0].savename;
            costtext.text = debt.debtlist[0].savemoney;
        }
        else
        {
            nametext.text = "";
            costtext.text = "";
        }
    }
    public void dropdownclickevent(Dropdown change)
    {
        nametext.text = debt.debtlist[change.value].savename;
        costtext.text = debt.debtlist[change.value].savemoney;

    }
    public void dropdownclickeventdelete(Dropdown change)
    {
        if (debt.debtlist.Count > 0)
        {
            debt.debtlist.RemoveAt(change.value);
            SaveData();
            updatedebtlist();
            nametext.text = "";
            costtext.text = "";
        }
    }
    public void dropdownclickeventpaid(Dropdown change)
    {
        if (debt.debtlist.Count > 0)
        {
            dropdownclickeventdelete(change);
            GameData.AddToken(100);
        }
    }

    [System.Serializable]
    public class DebtToSave
    {
        public List<DebtToSaveClass> debtlist = new List<DebtToSaveClass>();
    }
    [System.Serializable]
    public class DebtToSaveClass
    {
        public string savename;
        public string savemoney;

        public DebtToSaveClass(string savename, string savemoney)
        {
            this.savename = savename;
            this.savemoney = savemoney;
            
        }
    }

}
