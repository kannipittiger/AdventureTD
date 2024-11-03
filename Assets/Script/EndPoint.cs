using UnityEngine;
using UnityEngine.SceneManagement;

public class Endpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy reached endpoint! Loading Defeat scene...");
            SceneManager.LoadScene("Defeat");
        }
    }
}
