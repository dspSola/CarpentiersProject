using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRayCastFloor : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _lengtMax, _distanceToStopPlayer, _distanceHit;
    [SerializeField] private bool _touchGround;
    [SerializeField] Vector2 _pointHit;

    public void Initialize(float lengtMax)
    {
        _lengtMax = lengtMax;
    }
    public bool RayCastTouchFloor(out Vector2 hitOut)
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _lengtMax, _layerMask);
        hitOut = Vector2.zero;

        // If it hits something...
        if (hit.collider != null)
        {
            _distanceHit = Mathf.Abs(hit.point.y - transform.position.y);
            _pointHit = hit.point;
            hitOut = hit.point;
            _touchGround = true;
            Debug.DrawRay(transform.position, Vector2.down * _distanceHit, Color.red);
        }
        else
        {
            _pointHit = Vector2.zero;
            _touchGround = false;
            Debug.DrawRay(transform.position, Vector2.down * _lengtMax, Color.white);
        }

        return _touchGround;
    }

    public bool TouchGround { get => _touchGround; set => _touchGround = value; }
    public Vector2 PointHit { get => _pointHit; set => _pointHit = value; }
}
