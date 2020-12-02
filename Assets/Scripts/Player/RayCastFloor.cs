using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastFloor : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _lengt, _distance, _distanceToStopPlayer;

    public bool RayCastTouchFloor()
    {
        bool valueReturn = false;

        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _lengt, _layerMask);

        // If it hits something...
        if (hit.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            _distance = Mathf.Abs(hit.point.y - transform.position.y);

            if (_distance < _distanceToStopPlayer)
            {
                valueReturn = true;
                Debug.DrawRay(transform.position, Vector2.down * _distance, Color.cyan);
            }
            else
            {
                valueReturn = false;
                Debug.DrawRay(transform.position, Vector2.down * _distance, Color.red);
            }
        }
        else
        {
            valueReturn = false;
            Debug.DrawRay(transform.position, Vector2.down * _lengt, Color.white);
        }

        return valueReturn;
    }
}
