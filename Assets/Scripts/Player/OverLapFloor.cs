using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLapFloor : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 _halfExtents;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _center;

    [Header("Debug")]
    [SerializeField] private bool _drawGizmos;
    [SerializeField] private Color _gizmosColor;

    [SerializeField] private Transform _transformHit;

    private Collider2D[] _colliderBuffer = new Collider2D[1];
    private Color _activeColor;
    private Color _inactiveColor;
    private Color _color;

    private void Awake()
    {
        _activeColor = _gizmosColor * 1.5f;
        _inactiveColor = _gizmosColor * 0.5f;
    }

    private void Update()
    {
        //if (TestCollision())
        //{
        //    _color = _activeColor;
        //}
        //else
        //{
        //    _color = _inactiveColor;
        //}
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireCube(_center.position, _halfExtents);
        }
    }
    public bool TestCollision(Vector3 center, Vector3 halfExtents, Quaternion orientation, LayerMask layerMask)
    {
        return Physics2D.OverlapBoxNonAlloc(center, halfExtents, 0, _colliderBuffer, layerMask) > 0;
    }

    public bool TestCollision()
    {
        return TestCollision(_center.position, _halfExtents, Quaternion.identity, _layerMask);
    }
}
