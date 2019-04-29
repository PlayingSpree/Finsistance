using GameVar;
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
    public GameObject shopPanelPrefab;

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
        if (editMode)
        {
            //HoldCheck
            if (Input.touchCount > 0)
            {
                if (holdPos == Vector2.left)
                {
                    holdPos = Input.GetTouch(0).position;
                }
                holdTimer += Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (holdPos == Vector2.left)
                    {
                        holdPos = Input.mousePosition;
                    }
                    holdTimer += Time.deltaTime;
                }
                else
                {
                    holdPos = Vector2.left;
                    holdTimer = 0f;
                }
            }
            if (holdTimer > 1f)
            {
                Vector2 v;
                if (Input.touchCount > 0)
                {
                    v = Input.GetTouch(0).position;
                }
                else
                {
                    v = Input.mousePosition;
                }
                if (Vector2.Distance(holdPos, v) < 12f)
                {
                    if (editMovingItem != null)
                    {
                        if (editBoughtItem == editMovingItem)
                        {
                            mainScene.ShowNoti("CANNOT SELL UNBOUGHT ITEM");
                        }
                        else
                        {
                            //Sell
                            sellingItem = editMovingItem;
                            Item.ItemInfo item = gameData.GetItemInfoByType(editMovingItem.type);
                            sellingItemInfo = item;

                            GameObject g = Instantiate(shopPanelPrefab, confirmPanel_ItemPanel);
                            ItemPanelScript i = g.GetComponent<ItemPanelScript>();
                            i.item = item;
                            i.itemName.text = item.name;
                            i.itemPrice.text = item.price.ToString();
                            i.itemPrice.color = Color.black;
                            i.itemImage.sprite = item.sprite;

                            afterToken.SetText((gameData.token + (item.price / 5)).ToString());
                            sellPrice.SetText((item.price / 5).ToString());
                            if (itemPanelObject != null)
                            {
                                Destroy(itemPanelObject);
                            }
                            itemPanelObject = g;
                            confirmPanel.SetActive(true);
                        }
                    }
                }
                holdTimer = float.MinValue;
            }
            //MoveItem
            if (confirmPanel.activeInHierarchy == false)
            {
                if (editMovingItem == null)
                {
                    Vector2Int v = hoveredItem(false);
                    if (v != Vector2Int.left)
                    {
                        if ((v.x < gridMatrixItem.GetLength(0)) && v.y < (gridMatrixItem.GetLength(1)))
                            editMovingItem = gridMatrixItem[v.x, v.y];
                        if (editMovingItem != null)
                        {
                            editItemlastPos = editMovingItem.position;
                            editItemMoved = false;
                        }
                    }
                }
                else
                {
                    Vector2Int v = hoveredItem(true);
                    if (v == Vector2Int.left)
                    {
                        if (!editItemMoved)
                        {
                            editMovingItem.rotation += 1;
                            if ((int)editMovingItem.rotation > 3)
                            {
                                editMovingItem.rotation = 0;
                            }

                            editMovingItem.position = ClampItemPos(editMovingItem.position, editMovingItem);

                            UpdateItems();
                            GenerateGrid();
                        }
                        editMovingItem = null;
                    }
                    else
                    {
                        v = ClampItemPos(v, editMovingItem);
                        editMovingItem.position.Set(v.x, v.y);
                        if (editItemlastPos != editMovingItem.position)
                        {
                            editItemMoved = true;
                            UpdateItems();
                            GenerateGrid();
                        }
                        editItemlastPos = editMovingItem.position;
                    }
                }
            }
            else
            {
                editMovingItem = null;
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
    public Item.ItemInfo editBoughtItemInfo;
    Item editBoughtItem;
    Vector2Int editItemlastPos = Vector2Int.left;
    bool editItemMoved = false;
    bool editMode = false;
    bool editInvalidPlacement = false;
    List<Item> editBackup;

    public void PressEditButton()
    {
        EnterEditMode();

        mainScene.UIanimator.SetTrigger("Change");
        mainScene.UIanimator.SetInteger("NextUI", 1);
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
            mainScene.ShowNoti("FIX INVALID PLACEMENT");
        }
        else
        {
            if (editBoughtItemInfo != null)
            {
                mainScene.ShowNoti("ITEM BOUGHT", new Color(0f, 0.5f, 0f, 0.5f), new Color(0.815f, 1f, 0.815f, 0.815f));
                gameData.token -= editBoughtItemInfo.price;
                UpdateUI();
            }
            QuitEditMode();
            gameData.SaveGameData();
        }
    }

    public void PressEditCancleButton()
    {
        gameData.placedItems = editBackup;
        UpdateItems();
        QuitEditMode();
    }

    public void EnterEditMode()
    {
        editMode = true;
        editBackup = new List<Item>();
        foreach (Item item in gameData.placedItems)
        {
            editBackup.Add(new Item(item));
        }
        if (editBoughtItemInfo != null)
        {
            editBoughtItem = new Item(editBoughtItemInfo);
            gameData.placedItems.Add(editBoughtItem);
        }
        UpdateItems();
        GenerateGrid();
    }

    void QuitEditMode()
    {
        editBoughtItemInfo = null;
        editBoughtItem = null;
        editMode = false;
        mainScene.UIanimator.SetTrigger("Change");
        mainScene.UIanimator.SetInteger("NextUI", 0);
        RemoveAllObjectFromList(gridObject);
    }

    //ConfirmSell
    float holdTimer = 0;
    Vector2 holdPos;
    public GameObject confirmPanel;
    public Transform confirmPanel_ItemPanel;
    GameObject itemPanelObject;
    public TMPro.TMP_Text afterToken;
    public TMPro.TMP_Text sellPrice;
    Item sellingItem;
    Item.ItemInfo sellingItemInfo;

    public void PressSellButton()
    {
        mainScene.ShowNoti("ITEM SOLD", new Color(0.5f, 0.5f, 0f, 1f), new Color(1f, 1f, 0.815f, 0.815f));
        gameData.token += sellingItemInfo.price / 5;
        gameData.placedItems.Remove(sellingItem);

        sellingItem = null;
        sellingItemInfo = null;

        gameData.SaveGameData();
        UpdateUI();

        if (editBoughtItem != null)
        {
            gameData.placedItems.Remove(editBoughtItem);
        }
        editBackup = new List<Item>();
        foreach (Item item in gameData.placedItems)
        {
            editBackup.Add(new Item(item));
        }
        if (editBoughtItem != null)
        {
            gameData.placedItems.Add(editBoughtItem);
        }
        UpdateItems();
        GenerateGrid();
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

    Vector2Int hoveredItem(bool Clamp)
    {
        if (Input.touchCount > 0)
        {
            return ScreenPosToGrid(Input.GetTouch(0).position, Clamp);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                return ScreenPosToGrid(Input.mousePosition, Clamp);
            }
            else
            {
                return Vector2Int.left;
            }
        }
    }

    Vector2Int ScreenPosToGrid(Vector2 ScreenPos, bool Clamp)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(ScreenPos);
        worldPos -= gameData.roomInfo.offset;
        return new Vector2Int(Mathf.Clamp(Mathf.RoundToInt((worldPos.y * 2) - worldPos.x + ((gameData.roomInfo.size.x - 1) / 2)), 0, (Clamp ? (int)(gameData.roomInfo.size.x - 1) : int.MaxValue)), Mathf.Clamp(Mathf.RoundToInt(worldPos.x + (worldPos.y * 2) + ((gameData.roomInfo.size.y - 1) / 2)), 0, (Clamp ? (int)(gameData.roomInfo.size.y - 1) : int.MaxValue)));
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

    public void RemoveAllObjectFromList(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        list.Clear();
    }
}