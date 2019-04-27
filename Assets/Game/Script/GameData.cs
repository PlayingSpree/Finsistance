using GameVar;
using UnityEngine;
using System.Collections.Generic;

//Loaded Data=====
public class GameData
{
    public bool LoadGameData(List<Sprite> itemSprite)
    {
        itemInfo = new List<Item.ItemInfo>(Item.ItemCount);
        for (int i = 0; i < Item.ItemCount; i++)
        {
            itemInfo.Add(new Item.ItemInfo((Item.ItemType)i, itemSprite[i]));
        }
        roomInfo = new RoomInfo(RoomInfo.RoomType.Apartment5x5);
        placedItems = new List<Item>() { new Item(Item.ItemType.Bed3, Item.Rotation.RightDown, new Vector2Int(0, 3)), new Item(Item.ItemType.Drawer, Item.Rotation.LeftDown, new Vector2Int(1, 4)) };
        token = 10000;
        return true;
    }
    //Data
    public List<Item.ItemInfo> itemInfo;
    //Game
    public int token;
    public RoomInfo roomInfo;
    public List<Item> placedItems;

    //UI
    public enum UIenum
    {
        Main,
        GameEdit
    }
}

//Class & Hard-Coded Data=====
namespace GameVar
{
    //Game
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
    public class Item
    {
        public Item(ItemType type, Rotation rotation, Vector2Int position)
        {
            this.type = type;
            this.rotation = rotation;
            this.position = position;
        }

        public const int ItemCount = 4;
        public enum ItemType
        {
            Bed1,
            Bed2,
            Bed3,
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
                        name = "Bed 1";
                        price = 1000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Bed2:
                        name = "Bed 2";
                        price = 10000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Bed3:
                        name = "Bed 3";
                        price = 30000;
                        size = new Vector2Int(1, 2);
                        break;
                    case ItemType.Drawer:
                        name = "Drawer";
                        price = 3000;
                        size = new Vector2Int(1, 2);
                        break;
                }
                this.sprite = sprite;
            }
            public Vector2Int size;
            public int price;
            public string name;
            public Sprite sprite;
        }
    }
}