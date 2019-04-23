using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameVar;

public class ShopScript : MonoBehaviour
{
    //Ref
    public Transform shopListParent;
    public GameObject shopPanelPrefab;
    public GameScript gameScript;

    void Start()
    {
        foreach (Item.ItemInfo item in gameScript.gameData.itemInfo)
        {
            GameObject g = Instantiate(shopPanelPrefab,shopListParent);
            ItemPanelScript i = g.GetComponent<ItemPanelScript>();
            i.itemName.text = item.name;
            i.itemPrice.text = item.price.ToString();
            i.itemImage.sprite = item.sprite;
        }
    }

    //Shop
    public void PressShopButton()
    {
        gameScript.mainScene.UIanimator.SetTrigger("Change");
        gameScript.mainScene.UIanimator.SetInteger("NextUI", 2);
    }

    public void PressShopExitButton()
    {
        gameScript.mainScene.UIanimator.SetTrigger("Change");
        gameScript.mainScene.UIanimator.SetInteger("NextUI", 0);
    }
}
