using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Btntutorial : MonoBehaviour
{
    public Button yourButton;
    public GameObject PopupTut;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick(){
		PopupTut.SetActive(true);
	}
}
