using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _textScore;

    private void Update()
    {
        _textScore.text = _playerData.Score.ToString();
    }
}
