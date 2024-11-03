using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Btnexit : MonoBehaviour
{
    // Start is called before the first frame update
    public Button yourButton;
    public GameObject PopupTut;
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
		PopupTut.SetActive(false);
	}
}
