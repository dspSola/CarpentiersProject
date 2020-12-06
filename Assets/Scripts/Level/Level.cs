using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private float _timeOfGame, _score;

    private void Awake()
    {
        _playerData.InitPlayerData();
        _timeOfGame = 0;
        _score = 0;
    }

    private void Start()
    {

    }

    private void Update()
    {
        _timeOfGame += Time.deltaTime;
        AddScore();
    }

    private void AddScore()
    {
        _score = _timeOfGame * _playerData.CptFragmentPartition * _playerData.GameFlow; 
    }
}
