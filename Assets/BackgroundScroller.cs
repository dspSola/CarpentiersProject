using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float _scrollSpeed;

    void Update()
    {
        transform.position -= new Vector3(transform.position.x, _scrollSpeed, transform.position.z);
    }
}
