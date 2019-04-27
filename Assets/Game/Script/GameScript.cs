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

    public void PressEditButton()
    {
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
                            s.color = new Color(0.5f, 0f, 0f, 0.2f);
                            break;
                        default:
                            s.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                            break;
                    }
                }
                else
                {
                    switch (gridMatrix[l, r])
                    {
                        case 1:
                            s.color = new Color(1f, 0f, 0f, 0.2f);
                            break;
                        default:
                            s.color = new Color(1f, 1f, 1f, 0.2f);
                            break;
                    }
                }
            }
        }
    }

    void UpdateGridMatrix()
    {
        gridMatrix = new int[(int)gameData.roomInfo.size.x, (int)gameData.roomInfo.size.y];
        foreach (Item item in gameData.placedItems)
        {
            int il, ir;
            if ((item.rotation == Item.Rotation.LeftDown) || (item.rotation == Item.Rotation.LeftUp))
            {
                il = gameData.itemInfo[(int)item.type].size.y;
                ir = gameData.itemInfo[(int)item.type].size.x;
            }
            else
            {
                il = gameData.itemInfo[(int)item.type].size.x;
                ir = gameData.itemInfo[(int)item.type].size.y;
            }
            for (int l = item.position.x; l < il; l++)
            {
                for (int r = item.position.y; r < ir; r++)
                {
                    gridMatrix[l, r] = 1;
                }
            }
        }
    }

    public void PressEditSaveButton()
    {
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

    void RemoveAllObjectFromList(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        list.Clear();
    }
}