using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI curremcyUI;

    private void OnGUI(){
        curremcyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected(){
        
    }
}
