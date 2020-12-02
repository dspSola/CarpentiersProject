using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeDetectZone : MonoBehaviour
{
    [SerializeField] private Transform _detectZone, _startTopPos, _startRightPos, _startDownPos, _startLeftPos;

    public void Initialize(string startPos)
    {
        if(startPos == "Top")
        {
            _detectZone.position = _startTopPos.position;
        }
        if (startPos == "Right")
        {
            _detectZone.position = _startRightPos.position;
            _detectZone.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
        if (startPos == "Down")
        {
            _detectZone.position = _startDownPos.position;
        }
        if (startPos == "Left")
        {
            _detectZone.position = _startLeftPos.position;
            _detectZone.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
    }
}
