using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partition : MonoBehaviour
{
    [SerializeField] private float _speed, _coefSpeed, _time, _time1d, _time2d;
    [SerializeField] private bool _use1D, _use2D;

    private void Update()
    {
        transform.position -= Vector3.up * Time.deltaTime * _speed;
    }

    private void ShitFonction()
    {
        _time += Time.deltaTime;

        _time1d = Mathf.Round(_time * 10.0f) / 10.0f;
        _time2d = Mathf.Round(_time * 100.0f) / 100.0f;
        if (_use1D && !_use2D)
        {
            if (_time1d % _coefSpeed == 0)
            {
                transform.position -= new Vector3(0, 0.01f);
            }
        }
        else if (!_use1D && _use2D)
        {
            if (_time2d % _coefSpeed == 0)
            {
                transform.position -= new Vector3(0, 0.01f);
            }
        }
        else if (!_use1D && !_use2D)
        {
            Debug.Log("Tu as coché aucun bool, faudrait bien que tu changes de main");
        }
        else
        {
            Debug.Log("Tu as coché les deux bool sale boloss c'est pas bien de tromper");
        }
    }
}
