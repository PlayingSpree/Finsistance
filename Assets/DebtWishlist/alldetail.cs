using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class alldetail : MonoBehaviour
{
    public Button back;
    public Button gosave;
    public Button wishlistback;
    public Button clearit;
    public Canvas panel;
    public Canvas wishlist;
    public InputField name;
    public InputField money;
    public InputField des;
    public InputField kuy;
    public Text showdropdownclick;
    public Dropdown dropdown2;
    public Text showwishlistname;
    public Text showwishlistmoney;
    public Text showwishlistdes;
    public Button delete;
    public Button saveit;

    

    //--------------------------------------------

    public Text ftext;
    public Text listtext;
    public Text showdata;
    public Dropdown dropdown1;
    private string load;
    public DataToSave data = new DataToSave();
    public ArrayList arryList1 = new ArrayList();
    private int integer_Value_we_Want;

    //ของ Dropdown

    List<string> names = new List<string>() { };
    List<string> dropdownlist = new List<string>() { };
    List<string> dropdownlist2 = new List<string>() { };

    private void Start()
    {        
        panel.enabled = true;
        wishlist.enabled = false;  
        updatewishlist();
        gosave.onClick.AddListener(listtwish);
        back.onClick.AddListener(backpage);
    }

    public void cantpresssave() {
        if (money.text.Substring(0, 1) == "-")
        {
            saveit.interactable = false;
        }
        else {

            saveit.interactable = true;
        }

    }

    public void poppulatedrop() {
        dropdown1.AddOptions(names);
    }

    public void cleartext() {
        name.text = " ";
        money.text = " ";
        des.text = " ";
    }

    public void SaveData()//เซฟดาต้า
    {
        string filePath = Path.Combine(Application.persistentDataPath, "test.json"); // Path + File Name
        string jsonData = JsonUtility.ToJson(data); // Object -> json
        File.WriteAllText(filePath, jsonData); // json -> File

    }

    public void presssave()//กดเซฟ
    {
        if (name.text != "" && money.text != "" && money.text.Substring(0, 1) != "-")
        {
            

            data.dataList.Add(new DataToSaveClass(name.text, money.text, des.text));
            SaveData();
            updatewishlist();
            name.text = "";
            money.text = "";
            des.text = "";
            backpage();

        }
        else {
            saveit.interactable = false;
            name.text = "";
            money.text = "";
            des.text = "";
            saveit.interactable = true;
        }
    }

    public void clearinputfield() {
        name.text = "";
        money.text = "";
        des.text = "";

    }
    public void updatewishlist() {
        string filePath = Path.Combine(Application.persistentDataPath, "test.json"); // Path + File Name
        if (File.Exists(filePath))
        {
            string loadData = File.ReadAllText(filePath); // File -> json
            data = JsonUtility.FromJson<DataToSave>(loadData); // json -> Object 
        }
        
        dropdownlist2.Clear();
        foreach (DataToSaveClass item in data.dataList)
        {
            dropdownlist2.Add(item.savename);
        }
        dropdownlistupdate();

    }
    public void dropdownlistupdate() {
        dropdown1.ClearOptions();
        dropdown1.AddOptions(dropdownlist2);
        if (dropdown1.options.Count != 0)
        {
            showwishlistname.text = data.dataList[0].savename;
            showwishlistmoney.text = data.dataList[0].savemoney;
            showwishlistdes.text = data.dataList[0].savedes;
        }
        else
        {
            showwishlistname.text = "";
            showwishlistmoney.text = "";
            showwishlistdes.text = "";
        }
    }
    public void firstpage()//แสดงหน้าแรก
    {
        updatewishlist();
        panel.enabled = true;
        wishlist.enabled = false;
    }

    public void dropdownclickevent(Dropdown change) {
        showwishlistname.text = data.dataList[change.value].savename;
        showwishlistmoney.text = data.dataList[change.value].savemoney;
        showwishlistdes.text = data.dataList[change.value].savedes;
        string filePath = Path.Combine(Application.persistentDataPath, "test.json"); // Path + File Name
        string loadData = File.ReadAllText(filePath); // File -> json
        data = JsonUtility.FromJson<DataToSave>(loadData); // json -> Object
        //data.dataList[index]
        
        
    }
    //-------------------------------------------------------
    public void dropdownclickeventdelete(Dropdown change)
    {
        data.dataList.RemoveAt(change.value);
        SaveData();
        updatewishlist();
        showwishlistmoney.text = "";
        showwishlistname.text = "";
        showwishlistdes.text = "";      
    }

    public void dropdownclickeventedit(Dropdown change) {
        letsave();
        name.text = showwishlistname.text;
        money.text = showwishlistmoney.text;
        des.text = showwishlistdes.text;
        data.dataList.RemoveAt(change.value);       
        data.dataList[change.value] = new DataToSaveClass(name.text, money.text, des.text);
        SaveData();
        updatewishlist();
        name.text = "";
        money.text = "";
        des.text = "";

    }

    public void backpage()//ปุ่มกลับเมื่ออยู่หน้าที่ 2 
    {
            panel.enabled = true;
            wishlist.enabled = false;
    }
    public void listtwish()//แสดงเฉพาะหน้า Wishlist(3)
    {
        panel.enabled = false;
        wishlist.enabled = true;   
    }
    public void letsave()//ไปหน้าที่ Wishlist(3)
    {       
        panel.enabled = false;
        wishlist.enabled = true;
    }
    public void goo()//ไปหน้าที่ 2 
    {
        panel.enabled = false;
        wishlist.enabled = false;
    }
    [System.Serializable]
    public class DataToSave
    {
        public List<DataToSaveClass> dataList = new List<DataToSaveClass>();
    }
    [System.Serializable]
    public class DataToSaveClass
    {
        public string savename;
        public string savemoney;
        public string savedes;

        public DataToSaveClass(string savename, string savemoney, string savedes)
        {
            this.savename = savename;
            this.savemoney = savemoney;
            this.savedes = savedes;
        }
    }

    


}
