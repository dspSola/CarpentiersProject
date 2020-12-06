using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetection : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _longueurMax, _cptRayCastTouch, _minDistanceHit, _moyenneHit, _maxDistanceHit;

    [SerializeField] private List<Transform> _originsRayCast;

    [SerializeField] private bool _ifActive, _ifOnOfRayCastTouch, _ifOnOfRayCastTouchAt0;
    [SerializeField] private Transform _transformHit;


    private void FixedUpdate()
    {
        if (_ifActive)
        {
            AllRayCast();
        }
    }

    private void AllRayCast()
    {
        _cptRayCastTouch = 0f;
        _minDistanceHit = 999f;
        _moyenneHit = 0f;
        _maxDistanceHit = -999f;
        _ifOnOfRayCastTouch = false;
        _ifOnOfRayCastTouchAt0 = false;
        _transformHit = null;

        float _moyenneAdition = 0f;

        for (int i = 0; i < _originsRayCast.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_originsRayCast[i].position, _direction, _longueurMax, _layerMask);

            if (hit.collider != null)
            {
                _transformHit = hit.transform;

                _cptRayCastTouch++;
                if (!_ifOnOfRayCastTouch)
                {
                    _ifOnOfRayCastTouch = true;
                }

                //float distanceHit = Mathf.Abs(hit.point.y - transform.position.y);
                //float distanceHit = Mathf.Abs(hit.point.y - _originsRayCast[i].position.y);
                //float distanceHit = hit.point.y - _originsRayCast[i].position.y;
                float distanceHit = hit.distance;
                _moyenneAdition += distanceHit;

                if (distanceHit < _minDistanceHit)
                {
                    _minDistanceHit = distanceHit;
                }
                if (distanceHit > _maxDistanceHit)
                {
                    _maxDistanceHit = distanceHit;
                }
                Debug.DrawRay(_originsRayCast[i].position, _direction * distanceHit, Color.red);
            }
            else
            {
                Debug.DrawRay(_originsRayCast[i].position, _direction * _longueurMax, Color.white);
            }
        }

        if(_cptRayCastTouch > 0)
        {
            _moyenneHit = _moyenneAdition / _cptRayCastTouch;
        }

        if (_minDistanceHit == 0f)
        {
            if (!_ifOnOfRayCastTouchAt0)
            {
                _ifOnOfRayCastTouchAt0 = true;
            }
        }
    }

    public LayerMask LayerMask { get => _layerMask; set => _layerMask = value; }
    public Vector2 Direction { get => _direction; set => _direction = value; }
    public float LongueurMax { get => _longueurMax; set => _longueurMax = value; }
    public float MinDistanceHit { get => _minDistanceHit; set => _minDistanceHit = value; }
    public float MoyenneHit { get => _moyenneHit; set => _moyenneHit = value; }
    public float MaxDistanceHit { get => _maxDistanceHit; set => _maxDistanceHit = value; }
    public List<Transform> OriginsRayCast { get => _originsRayCast; set => _originsRayCast = value; }
    public bool IfActive { get => _ifActive; set => _ifActive = value; }
    public bool IfOnOfRayCastTouch { get => _ifOnOfRayCastTouch; set => _ifOnOfRayCastTouch = value; }
    public bool IfOnOfRayCastTouchAt0 { get => _ifOnOfRayCastTouchAt0; set => _ifOnOfRayCastTouchAt0 = value; }
    public float CptRayCastTouch { get => _cptRayCastTouch; set => _cptRayCastTouch = value; }
    public Transform TransformHit { get => _transformHit; set => _transformHit = value; }
}
