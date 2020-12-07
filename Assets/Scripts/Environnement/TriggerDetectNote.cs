using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectNote : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _firstPlatform;
    [SerializeField] private bool _firstNotePassed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("RGEZGZEGER");
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("RGEZGZEGER");
            if (!_firstNotePassed)
            {
                Debug.Log("RGEZGZEGER");
                _spriteRenderer.enabled = false;
                _firstPlatform.SetActive(false);
                _firstNotePassed = true;
            }
        }
    }
}
