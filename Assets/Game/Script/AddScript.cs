using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddScript : MonoBehaviour
{
    public Transform typeButtonParent;
    public GameObject typeButton;
    public TMP_InputField inputField;
    public Image image;

    int selectedType;
    int selectedBag;
    bool income = false;
    List<GameObject> typeButtonObject = new List<GameObject>();
    public void PressAddButton()
    {
        ClearField();
    }
    public void PressCancleButton()
    {
        ClearField();
    }

    void ClearField()
    {
        income = false;
        inputField.text = "";
        image.color = new Color(1f, 0.835f, 0.835f);
        PressOutcomeButton();
    }

    public void PressIncomeButton()
    {
        selectedType = 0;
        image.color = new Color(0.835f, 1f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < 5; i++)
        {
            CreateTypeButton(i.ToString()+"I");
        }
    }
    public void PressOutcomeButton()
    {
        selectedType = 0;
        image.color = new Color(1f, 0.835f, 0.835f);
        RemoveAllObjectFromList(typeButtonObject);
        for (int i = 0; i < 6; i++)
        {
            CreateTypeButton(i.ToString()+"O");
        }
    }

    void CreateTypeButton(string text)
    {
        GameObject g = Instantiate(typeButton, typeButtonParent);
        typeButtonObject.Add(g);
        TMP_Text t = g.GetComponentInChildren<TMP_Text>();
        t.SetText(text);
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