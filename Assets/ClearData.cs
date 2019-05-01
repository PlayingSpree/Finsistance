using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ClearData : MonoBehaviour
{
    public void ClearAllFile()
    {
        List<string> filePath = new List<string>();
        filePath.Add(Path.Combine(Application.persistentDataPath, "saveDaily.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "saveNoti.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "WishlistData.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "DebtData.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "GameData.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "TransactionData.json"));
        filePath.Add(Path.Combine(Application.persistentDataPath, "WalletData.json"));
        foreach (string item in filePath)
        {
            File.Delete(item);
        }
    }
}
