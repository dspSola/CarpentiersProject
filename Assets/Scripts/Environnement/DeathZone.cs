using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            SceneManager.LoadScene("VerticalTestScroll");
            SceneManager.LoadScene("Camera", LoadSceneMode.Additive);
            SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        }
    }
}
