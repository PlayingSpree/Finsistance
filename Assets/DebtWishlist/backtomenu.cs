using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class backtomenu : MonoBehaviour
{
    public string SceneToChange = "debt";
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneToChange, LoadSceneMode.Single);
    }
}