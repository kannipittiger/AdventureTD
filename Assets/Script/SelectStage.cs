using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button firstStageButton;
    [SerializeField] private Button secondStageButton;
    [SerializeField] private TextMeshProUGUI firstStageText;
    [SerializeField] private TextMeshProUGUI secondStageText;
    [SerializeField] private TextMeshProUGUI invalidText;
    [SerializeField] private Button mainMenu;
    [SerializeField] public GameObject selectStagePanel;

    // private EnemySpawner enemySpawner;
    private bool isCheck;
    private void Start()
    {
        isCheck = ChangeScript.isVictory;
        invalidText.gameObject.SetActive(false);
        firstStageText.text = $"Play";
        if (isCheck)
        {
            secondStageText.text = $"Play";
            secondStageButton.onClick.AddListener(TaskOnClick);
        }
        else
        {
            secondStageText.text = $"Locked";
            secondStageButton.onClick.AddListener(LockOnClick);
            ;
        }
    }
    private void LockOnClick(){
        StartCoroutine(ShowInvalidText("Complete stage 1 first!"));
    }

    // public void CheckVictory()
    // {
    //     if (isCheck)
    //     {
    //         secondStageText.text = $"Play";
    //         secondStageButton.onClick.AddListener(TaskOnClick);
    //     }
    //     else
    //     {
    //         secondStageText.text = $"Locked";
    //         StartCoroutine(ShowInvalidText("Complete stage 1 first!"));
    //     }
    // }
    private void TaskOnClick(){
        SceneManager.LoadScene("DemonScene");
    }


    public void ToStageOne()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMenu()
    {
        selectStagePanel.SetActive(false);
    }

    private IEnumerator ShowInvalidText(string message)
    {
        invalidText.gameObject.SetActive(true);
        invalidText.text = message;
        yield return new WaitForSeconds(1f);  // Wait for 3 seconds before hiding the message
        invalidText.gameObject.SetActive(false);
    }


}
