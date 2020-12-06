using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private Vector2 _positionPlayer;
    [SerializeField] private int _cptFragmentPartition;
    [SerializeField] private float _gameFlow;
    [SerializeField] private float _score;

    public void InitPlayerData()
    {
        _positionPlayer = new Vector2();
        _cptFragmentPartition = 0;
        _gameFlow = 1;
    }

    public Vector2 PositionPlayer { get => _positionPlayer; set => _positionPlayer = value; }
    public int CptFragmentPartition { get => _cptFragmentPartition; set => _cptFragmentPartition = value; }
    public float GameFlow { get => _gameFlow; set => _gameFlow = value; }
    public float Score { get => _score; set => _score = value; }
}
