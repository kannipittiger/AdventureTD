using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScript : MonoBehaviour
{
    public void MoveToGame(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Xit(){
        Application.Quit();
        Debug.Log("AOk");
    }

    public void Gonext(){
        SceneManager.LoadScene("DemonScene");
    }

    public void GoRetreat(){
        SceneManager.LoadScene("start game");
    }

}
