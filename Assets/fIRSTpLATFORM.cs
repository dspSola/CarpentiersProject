using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fIRSTpLATFORM : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        Destroy(gameObject, 18f);
    }
}
