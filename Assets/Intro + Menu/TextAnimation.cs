using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    private TMP_Text _text;
    [SerializeField] private string _line;
    bool done;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (this.isActiveAndEnabled && !done)
        {
            Debug.Log("Activated");
            _text.DOText(_line, 2, true);
            done = true;
        }
    }
}
