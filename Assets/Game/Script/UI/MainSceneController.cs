using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    //Ref=====
    public GameScript gameScript;
    public Animator UIanimator;

    public TextNumberEffect token;

    public TMPro.TMP_Text notiText;
    public Image notiBG;

    public void ShowNoti(string text)
    {
        ShowNoti(text, Color.red, new Color(1f, 0.815f, 0.815f, 0.815f));
    }

    public void ShowNoti(string text, Color color, Color colorBG)
    {
        notiBG.color = colorBG;
        notiText.color = color;
        notiText.SetText(text);
        UIanimator.SetTrigger("NotiShow");
    }

    public void ChangeScene(int scene)
    {
        if(scene == 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("AssistanceScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }
}
