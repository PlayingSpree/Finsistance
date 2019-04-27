﻿using GameVar;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    //Ref=====
    public MainSceneController mainScene;
    public List<Sprite> itemSprite;

    //Prefab=====
    public GameObject editFloor;
    public GameObject[] itemPrefab;

    //Data=====
    public GameData gameData = new GameData();

    //Update=====
    void Awake()
    {
        gameData.LoadGameData(itemSprite);
    }
    void Start()
    {
        UpdateUI();
        UpdateItems();
    }

    private void Update()
    {
        //EditMode
        if (editMovingItem == null)
        {
            if (editMode)
            {
                Vector2Int v = hoveredItem();
                if (v != Vector2Int.left)
                {
                    editItemlastPos = Vector2Int.left;
                    editMovingItem = gridMatrixItem[v.x, v.y];
                }
            }
        }
        else
        {
            Vector2Int v = hoveredItem();
            if (v == Vector2Int.left)
            {
                editMovingItem = null;
            }
            else
            {
                v = ClampItemPos(v, editMovingItem);
                editMovingItem.position.Set(v.x, v.y);
                if (editItemlastPos != editMovingItem.position)
                {
                    UpdateItems();
                    GenerateGrid();
                }
                editItemlastPos = editMovingItem.position;
            }
        }
    }

    //UI=====
    void UpdateUI()
    {
        mainScene.token.number = gameData.token;
    }

    //Items=====
    List<GameObject> itemObject = new List<GameObject>();
    void UpdateItems()
    {
        RemoveAllObjectFromList(itemObject);
        foreach (Item item in gameData.placedItems)
        {
            GameObject g = Instantiate(itemPrefab[(int)item.type], GetIsoPos(item.position), Quaternion.identity);
            if ((item.rotation == Item.Rotation.LeftDown) || (item.rotation == Item.Rotation.LeftUp))
                g.transform.localScale = new Vector3(-1, 1, 1);
            itemObject.Add(g);
        }
    }

    //EditMode=====
    List<GameObject> gridObject = new List<GameObject>();
    int[,] gridMatrix;
    Item[,] gridMatrixItem;
    Item editMovingItem;
    Item editBoughtItem;
    Vector2Int editItemlastPos = Vector2Int.left;
    bool editMode = false;
    bool editInvalidPlacement = false;
    List<Item> editBackup;

    public void PressEditButton()
    {
        editMode = true;
        editBackup = new List<Item>();
        foreach (Item item in gameData.placedItems)
        {
            editBackup.Add(new Item(item));
        }

        mainScene.UIanimator.SetTrigger("Change");
        mainScene.UIanimator.SetInteger("NextUI", 1);
        GenerateGrid();
    }

    void GenerateGrid()
    {
        RemoveAllObjectFromList(gridObject);
        UpdateGridMatrix();
        for (int l = 0; l < gameData.roomInfo.size.x; l++)
        {
            for (int r = 0; r < gameData.roomInfo.size.y; r++)
            {
                GameObject g = Instantiate(editFloor, GetIsoPos(l, r), Quaternion.identity);
                gridObject.Add(g);
                SpriteRenderer s = g.GetComponent<SpriteRenderer>();
                if ((l + r) % 2 == 0)
                {
                    switch (gridMatrix[l, r])
                    {
                        case 1:
                            s.color = new Color(0.5f, 0.5f, 0f, 0.2f);
                            break;
                        case 0:
                            s.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                            break;
                        default:
                            s.color = new Color(0.5f, 0f, 0f, 0.2f);
                            break;
                    }
                }
                else
                {
                    switch (gridMatrix[l, r])
                    {
                        case 1:
                            s.color = new Color(1f, 1f, 0f, 0.2f);
                            break;
                        case 0:
                            s.color = new Color(1f, 1f, 1f, 0.2f);
                            break;
                        default:
                            s.color = new Color(1f, 0f, 0f, 0.2f);
                            break;
                    }
                }
            }
        }
    }

    void UpdateGridMatrix()
    {
        editInvalidPlacement = false;
        gridMatrix = new int[(int)gameData.roomInfo.size.x, (int)gameData.roomInfo.size.y];
        gridMatrixItem = new Item[(int)gameData.roomInfo.size.x, (int)gameData.roomInfo.size.y];
        foreach (Item item in gameData.placedItems)
        {
            Vector2Int s = ItemSizeRotated(item);
            int il = item.position.x + s.x;
            int ir = item.position.y + s.y;
            for (int l = item.position.x; l < il; l++)
            {
                for (int r = item.position.y; r < ir; r++)
                {
                    gridMatrix[l, r] += 1;
                    if (gridMatrix[l, r] > 1)
                    {
                        editInvalidPlacement = true;
                    }
                    gridMatrixItem[l, r] = item;
                }
            }
        }
    }

    public void PressEditSaveButton()
    {
        if (editInvalidPlacement)
        {
            mainScene.notiText.SetText("FIX INVALID PLACEMENT");
            mainScene.UIanimator.SetTrigger("NotiShow");
        }
        else
        {
            editMode = false;
            mainScene.UIanimator.SetTrigger("Change");
            mainScene.UIanimator.SetInteger("NextUI", 0);
            RemoveAllObjectFromList(gridObject);
        }
    }

    public void PressEditCancleButton()
    {
        gameData.placedItems = editBackup;
        UpdateItems();

        editMode = false;
        mainScene.UIanimator.SetTrigger("Change");
        mainScene.UIanimator.SetInteger("NextUI", 0);
        RemoveAllObjectFromList(gridObject);
    }

    //Helper=====
    Vector2 GetIsoPos(Vector2Int v)
    {
        return GetIsoPos(v.x, v.y);
    }
    Vector2 GetIsoPos(int l, int r)
    {
        return new Vector2((((r - l) + ((gameData.roomInfo.size.x - gameData.roomInfo.size.y) / 2.0f)) / 2.0f) + gameData.roomInfo.offset.x, (((l + r) - ((gameData.roomInfo.size.y + gameData.roomInfo.size.x - 2) / 2.0f)) / 4.0f) + gameData.roomInfo.offset.y);
    }

    Vector2Int hoveredItem()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount > 0)
            {
                return ScreenPosToGrid(Input.GetTouch(0).position);
            }
            else
            {
                return Vector2Int.left;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                return ScreenPosToGrid(Input.mousePosition);
            }
            else
            {
                return Vector2Int.left;
            }
        }
    }

    Vector2Int ScreenPosToGrid(Vector2 ScreenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(ScreenPos);
        worldPos -= gameData.roomInfo.offset;
        return new Vector2Int(Mathf.Clamp(Mathf.RoundToInt((worldPos.y * 2) - worldPos.x + ((gameData.roomInfo.size.x - 1) / 2)), 0, (int)gameData.roomInfo.size.x - 1), Mathf.Clamp(Mathf.RoundToInt(worldPos.x + (worldPos.y * 2) + ((gameData.roomInfo.size.y - 1) / 2)), 0, (int)gameData.roomInfo.size.y - 1));
    }

    Vector2Int ClampItemPos(Vector2Int pos, Item item)
    {
        Vector2Int s = ItemSizeRotated(item);
        return new Vector2Int(Mathf.Clamp(pos.x, 0, (int)gameData.roomInfo.size.x - s.x), Mathf.Clamp(pos.y, 0, (int)gameData.roomInfo.size.y - s.y));
    }

    Vector2Int ItemSizeRotated(Item item)
    {
        if ((item.rotation == Item.Rotation.LeftDown) || (item.rotation == Item.Rotation.LeftUp))
        {
            return new Vector2Int(gameData.itemInfo[(int)item.type].size.y, gameData.itemInfo[(int)item.type].size.x);
        }
        else
        {
            return new Vector2Int(gameData.itemInfo[(int)item.type].size.x, gameData.itemInfo[(int)item.type].size.y);
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