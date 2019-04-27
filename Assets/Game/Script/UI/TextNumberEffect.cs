using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextNumberEffect : MonoBehaviour
{
    public TMP_Text text;
    public int number;
    public float numberText = 0;

    private void Start()
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {
        if (number != numberText)
        {
            numberText += (number - numberText) * Time.deltaTime * 10;
            if (Mathf.Abs(numberText - number) < 10)
            {
                numberText = number;
            }
            text.SetText(((int)numberText).ToString());
        }
    }
}
