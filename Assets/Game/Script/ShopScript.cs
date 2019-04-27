using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameVar;

public class ShopScript : MonoBehaviour
{
    //Ref
    public Transform shopListParent;
    public GameObject shopPanelPrefab;
    public GameScript gameScript;

    //Shop
    public List<GameObject> shopItemPanel;

    //Confirm
    public GameObject confirmPanel;
    public Transform confirmPanel_ItemPanel;
    GameObject itemPanelObject;
    public TMP_Text afterToken;
    Item.ItemInfo buyingItem;

    void UpdatePanel()
    {
        gameScript.RemoveAllObjectFromList(shopItemPanel);
        foreach (Item.ItemInfo item in gameScript.gameData.itemInfo)
        {
            shopItemPanel.Add(CreateShopItemPanel(item, shopListParent));
        }
    }

    GameObject CreateShopItemPanel(Item.ItemInfo item,Transform parent)
    {
        GameObject g = Instantiate(shopPanelPrefab, parent);
        ItemPanelScript i = g.GetComponent<ItemPanelScript>();
        i.item = item;
        i.itemName.text = item.name;
        i.itemPrice.text = item.price.ToString();
        if ((gameScript.gameData.token - item.price) >= 0)
        {
            i.itemPrice.color = Color.black;
        }
        else
        {
            i.itemPrice.color = Color.red;
        }
        i.itemImage.sprite = item.sprite;
        i.shopScript = this;
        return g;
    }

    //Shop
    public void PressShopButton()
    {
        UpdatePanel();
        gameScript.mainScene.UIanimator.SetTrigger("Change");
        gameScript.mainScene.UIanimator.SetInteger("NextUI", 2);
    }

    public void PressShopExitButton()
    {
        gameScript.mainScene.UIanimator.SetTrigger("Change");
        gameScript.mainScene.UIanimator.SetInteger("NextUI", 0);
    }

    public void PressShopItemButton(Item.ItemInfo itemInfo)
    {
        if ((gameScript.gameData.token - itemInfo.price) >= 0)
        {
            buyingItem = itemInfo;
            afterToken.SetText((gameScript.gameData.token - itemInfo.price).ToString());
            if (itemPanelObject != null)
            {
                Destroy(itemPanelObject);
            }
            itemPanelObject = CreateShopItemPanel(itemInfo, confirmPanel_ItemPanel);
            confirmPanel.SetActive(true);
        }
        else
        {
            gameScript.mainScene.ShowNoti("NOT ENOUGH TOKEN");
        }
    }

    public void PressShopBuyButton()
    {
        gameScript.editBoughtItem = buyingItem;
        gameScript.EnterEditMode();
        gameScript.mainScene.UIanimator.SetTrigger("Change");
        gameScript.mainScene.UIanimator.SetInteger("NextUI", 1);
    }
}
