using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Btnbacktomenu : MonoBehaviour
{
    
    public void backToMenu()
    {
        SceneManager.LoadScene("start game");
    }

}
