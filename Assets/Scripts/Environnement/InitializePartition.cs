using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePartition : MonoBehaviour
{
    [SerializeField] private Partition _partition;
    [SerializeField] private InitializeDetectZone _initializeDetectZone;
    [SerializeField] private bool _toUp, _toDown, _toRight, _toLeft, _setRandomSpriteAllFragment, _setRandomRotationAllFragment;
    [SerializeField] private Transform _startTopPos, _startRightPos, _startDownPos, _startLeftPos;

    private void Start()
    {
        int cptCoche = 0;
        if (_toUp)
        {
            cptCoche++;
        }
        if (_toDown)
        {
            cptCoche++;
        }
        if (_toRight)
        {
            cptCoche++;
        }
        if (_toLeft)
        {
            cptCoche++;
        }

        if (cptCoche == 0)
        {
            Debug.Log("Attention vous n'avez pas coché de direction à la partition");
        }
        else if (cptCoche > 1)
        {
            Debug.Log("Attention vous avez coché plusieurs direction à la partition");
        }
        else
        {
            Debug.Log("Partition bien initalizé");

            if (_toUp)
            {
                _partition.InitializePartition(_startDownPos);
                _initializeDetectZone.Initialize("Down");
            }
            if (_toDown)
            {
                _partition.InitializePartition(_startTopPos);
                _initializeDetectZone.Initialize("Top");
            }
            if (_toRight)
            {
                _partition.InitializePartition(_startLeftPos, 90f);
                _initializeDetectZone.Initialize("Left");
            }
            if (_toLeft)
            {
                _partition.InitializePartition(_startRightPos, -90f);
                _initializeDetectZone.Initialize("Right");
            }

            if (_setRandomSpriteAllFragment)
            {
                _partition.SetRandomSpriteAllFragment();
            }
            if (_setRandomRotationAllFragment)
            {
                _partition.SetRandomRotationAllFragment();
            }
        }
    }

    private void Update()
    {
        if (_toDown)
        {
            _partition.MovePartition(Vector3.down);
        }
        if (_toUp)
        {
            _partition.MovePartition(Vector3.up);
        }
        if (_toRight)
        {
            _partition.MovePartition(Vector3.right);
        }
        if (_toLeft)
        {
            _partition.MovePartition(Vector3.left);
        }
    }
}
