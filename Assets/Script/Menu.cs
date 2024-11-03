using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI curremcyUI;

    public GameObject pauseMenuScreen;

    private void OnGUI(){
        curremcyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected(){
        
    }

    public void PauseGame(){
        Time.timeScale = 0;
        pauseMenuScreen.SetActive(true);
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        pauseMenuScreen.SetActive(false);
    }

    public void GotoMenu(){
        SceneManager.LoadScene("start game");
    }
}
