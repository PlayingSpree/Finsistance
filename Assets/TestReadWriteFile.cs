using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.IO; // Using this

public class TestReadWriteFile : MonoBehaviour
{
    public TMP_InputField inputField; // Input field
    public TMP_Text textField; // Text output

    private DataToSave data = new DataToSave(); // Data object to save

    // Save to file
    void SaveData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "test.json"); // Path + File Name
        string jsonData = JsonUtility.ToJson(data); // Object -> json
        File.WriteAllText(filePath, jsonData); // json -> File
    }

    // Load from file
    void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "test.json"); // Path + File Name
        string loadData = File.ReadAllText(filePath); // File -> json
        data = JsonUtility.FromJson<DataToSave>(loadData); // json -> Object
        UpdateData();
    }



    // Update Text field and output data in object
    void UpdateData()
    {
        inputField.text = data.saveString;
        textField.text = string.Format("saveInt = {0} \nsaveArray = {{{1}}}\nsaveFloat = {2}", data.saveInt, string.Join(",", data.saveArray), data.saveFloat);
    }

    // Save button press
    public void PressSave()
    {
        data.saveString = inputField.text; // User input text
        // Edit some Data
        data.saveInt++;
        data.saveFloat *= data.saveInt;
        data.saveArray[data.saveInt % data.saveArray.Length] += (int)data.saveFloat;
        // Save sata
        SaveData();
    }

    // Load button press
    public void PressLoad()
    {
        LoadData();
    }
}

// Data Class
[System.Serializable]
public class DataToSave
{
    public int saveInt = 10; // int
    public int[] saveArray = new int[] {1,2,4,8,16,32,64,128}; // array
    public float saveFloat = 3.14f; // float
    public string saveString; // string (in text field)
}