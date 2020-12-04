using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLapBoxExemple : MonoBehaviour
{
    public LayerMask m_LayerMask;
    public Vector2 _halfExtents;

    private Collider2D[] _colliderBuffer = new Collider2D[1];

    void Start()
    {

    }

    void FixedUpdate()
    {
        MyCollisions();
    }

    public bool MyCollisions()
    {
        return Physics2D.OverlapBoxNonAlloc(new Vector2(transform.position.x, transform.position.y), _halfExtents, 0f, _colliderBuffer, m_LayerMask.value) > 0;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, _halfExtents);
    }
}
