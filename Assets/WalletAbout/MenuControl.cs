using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string SceneToChange = "menu";
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneToChange, LoadSceneMode.Single);
    }
}
