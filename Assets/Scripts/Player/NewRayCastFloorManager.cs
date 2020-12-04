using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRayCastFloorManager : MonoBehaviour
{
    [SerializeField] private float _lengtMaxOfRayCast;
    [SerializeField] private bool _oneTouchFloor;
    [SerializeField] private List<NewRayCastFloor> _newRayCastFloors;
    [SerializeField] private Vector2 _floorPosition;
    [SerializeField] private int _hitCount;

    private void Start()
    {
        for(int i = 0; i < _newRayCastFloors.Count; i++)
        {
            _newRayCastFloors[i].Initialize(_lengtMaxOfRayCast);
        }
    }

    public bool AverageCollision(out Vector2 averageCollisionPosition)
    {
        int hitCount = 0;
        Vector2 combinedPosition = Vector2.zero;

        for (int i = 0; i < _newRayCastFloors.Count; i++)
        {
            if (_newRayCastFloors[i].RayCastTouchFloor(out Vector2 hit))
            {
                combinedPosition += hit;
                hitCount++;
            }
        }

        if (hitCount > 0)
        {
            averageCollisionPosition = combinedPosition / hitCount;
        }
        else
        {
            averageCollisionPosition = Vector2.zero;
        }

        _floorPosition = averageCollisionPosition;
        _hitCount = hitCount;

        return hitCount > 0;
    }
}
