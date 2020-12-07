using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFragmentPartition : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private int _cptFragment;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == 9 && collision.gameObject.name != "FirstPlatform")
        //{
        //    _playerData.FirstTouched = true;
        //}
        if (collision.gameObject.layer == 13)
        {
            _cptFragment = collision.gameObject.GetComponent<FragmentPartition>().CptFragment;
            _playerData.CptFragmentPartition = _cptFragment;
        }
    }
}
