using GameVar;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

//Loaded Data=====
public class GameData
{
    //Data
    public List<Item.ItemInfo> itemInfo;
    //Game
    public int token;
    public RoomInfo roomInfo;
    public List<Item> placedItems;

    //Util
    public Item.ItemInfo GetItemInfoByType(Item.ItemType type)
    {
        foreach (Item.ItemInfo item in itemInfo)
        {
            if(item.itemType == type)
            {
                return item;
            }
        }
        return null;
    }

    //Save to file
    [System.Serializable]
    class DataToSave
    {
        public RoomInfo roomInfo = new RoomInfo(RoomInfo.RoomType.Apartment5x5);
        public List<Item> placedItems = new List<Item>() { new Item(Item.ItemType.Bed1, Item.Rotation.RightDown, new Vector2Int(0, 3)) };
        public int token = 10000;
    }
    public bool LoadGameData(List<Sprite> itemSprite)
    {
        //LoadInfo
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        itemInfo = new List<Item.ItemInfo>(Item.ItemCount);
        for (int i = 0; i < Item.ItemCount; i++)
        {
            itemInfo.Add(new Item.ItemInfo((Item.ItemType)i, itemSprite[i]));
        }
        //LoadFile
        DataToSave data = new DataToSave();
        if (File.Exists(filePath))
        {
            string loadData = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(loadData,data);
        }
        token = data.token;
        roomInfo = data.roomInfo;
        placedItems = data.placedItems;
        return true;
    }

    public void SaveGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        DataToSave data = new DataToSave();
        data.token = token;
        data.roomInfo = roomInfo;
        data.placedItems = placedItems;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    public static void AddToken(int token)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        DataToSave data = new DataToSave();
        if (File.Exists(filePath))
        {
            string loadData = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(loadData, data);
        }
        data.token += token;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }
}



//Class & Hard-Coded Data=====
namespace GameVar
{
    //Game
    [System.Serializable]
    public class RoomInfo
    {
        public RoomInfo(RoomType roomType)
        {
            size = new Vector2(5, 5);
            offset = new Vector2(0, -0.65f);
        }

        public enum RoomType
        {
            Apartment5x5
        }

        public Vector2 size;
        public Vector2 offset;
    }
    [System.Serializable]
    public class Item
    {
        public Item(ItemType type, Rotation rotation, Vector2Int position)
        {
            this.type = type;
            this.rotation = rotation;
            this.position = position;
        }

        public Item(Item i)
        {
            type = i.type;
            rotation = i.rotation;
            position = i.position;
        }

        public Item(ItemInfo editBoughtItem)
        {
            type = editBoughtItem.itemType;
            rotation = Rotation.LeftDown;
            position = Vector2Int.zero;
        }

        public const int ItemCount = 16;
        public enum ItemType
        {
            Bed1,
            Bed2,
            Bed3,
            Bed1c1,
            Bed2c1,
            Bed3c1,
            Bed1c2,
            Bed2c2,
            Bed3c2,
            Bed1c3,
            Bed2c3,
            Bed3c3,
            Bed1c4,
            Bed2c4,
            Bed3c4,
            Drawer
        }
        public ItemType type;
        public enum Rotation { LeftDown, RightDown, LeftUp, RightUp }
        public Rotation rotation;

        public Vector2Int position;

        //Data
        public class ItemInfo
        {
            public ItemInfo(ItemType type,Sprite sprite)
            {
                switch (type)
                {
                    case ItemType.Bed1:
                    case ItemType.Bed1c1:
                    case ItemType.Bed1c2:
                    case ItemType.Bed1c3:
                    case ItemType.Bed1c4:
                        name = "Bed 1";
                        price = 1000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Bed2:
                    case ItemType.Bed2c1:
                    case ItemType.Bed2c2:
                    case ItemType.Bed2c3:
                    case ItemType.Bed2c4:
                        name = "Bed 2";
                        price = 10000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Bed3:
                    case ItemType.Bed3c1:
                    case ItemType.Bed3c2:
                    case ItemType.Bed3c3:
                    case ItemType.Bed3c4:
                        name = "Bed 3";
                        price = 30000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Drawer:
                        name = "Drawer";
                        price = 3000;
                        size = new Vector2Int(1, 1);
                        break;
                }
                itemType = type;
                this.sprite = sprite;
            }
            public Vector2Int size;
            public int price;
            public string name;
            public Sprite sprite;
            public ItemType itemType;
        }
    }
}