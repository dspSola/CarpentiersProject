using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Show In Inspector

    [Header("Draw Gizmos")]
    [SerializeField] Transform _topLeftTransform;
    [SerializeField] Transform _bottomRightTransform;
    [SerializeField] Color _color;

    [Header("OverLapArea")]
    [SerializeField] private LayerMask _whatIsGround;

    #endregion

    #region Variables Globales

    private Collider2D[] _hitBuffer = new Collider2D[1];

    #endregion

    public bool ICantBelieveItsGround()
    {
        int groundHitCount = Physics2D.OverlapAreaNonAlloc(_topLeftTransform.position, _bottomRightTransform.position, _hitBuffer, _whatIsGround);

        return groundHitCount > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;

        Vector2 topLeft = _topLeftTransform.position;
        Vector2 topRight = new Vector2(_topLeftTransform.position.x, _bottomRightTransform.position.y);
        Vector2 bottomLeft = new Vector2(_bottomRightTransform.position.x, _topLeftTransform.position.y);
        Vector2 bottomRight = _bottomRightTransform.position;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
