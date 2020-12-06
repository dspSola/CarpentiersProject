using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Image _firstImage;
    [SerializeField] Text _firstText;

    private void Awake()
    {
        _firstImage.DOFade(0, 50);
        _firstText.DOFade(255, 100);
    }
}
