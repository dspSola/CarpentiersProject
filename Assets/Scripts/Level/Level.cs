using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private float _timeOfGame;

    private void Awake()
    {
        _playerData.InitPlayerData();
        _timeOfGame = 0;
        _playerData.Score = 0;
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
        //_playerData.Score = _timeOfGame * _playerData.CptFragmentPartition * _playerData.GameFlow; 
        _playerData.Score = _timeOfGame * _playerData.GameFlow; 
    }
}
