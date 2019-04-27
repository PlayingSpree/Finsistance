using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanelScript : MonoBehaviour
{
    //Ref
    public Image itemImage;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemName;
    public GameVar.Item.ItemInfo item;
    public ShopScript shopScript;

    public void PressItemButton() {
        shopScript.PressShopItemButton(item);
    }
}
