using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partition : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private List<FragmentPartition> _fragmentPartitions;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void InitializePartition(Transform _posStart)
    {
        transform.position = _posStart.position;
        for(int i = 0; i < _fragmentPartitions.Count; i++)
        {
            _fragmentPartitions[i].InitializePartition(i + 1);
        }
    }

    public void InitializePartition(Transform _posStart, float rotation)
    {
        transform.position = _posStart.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

    public void SetRandomSpriteAllFragment()
    {
        foreach (FragmentPartition fp in _fragmentPartitions)
        {
            fp.SetRandomSprite();
        }
    }

    public void SetRandomRotationAllFragment()
    {
        foreach (FragmentPartition fp in _fragmentPartitions)
        {
            fp.SetRandomRotation();
        }
    }

    public void MovePartition(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * _speed;
    }
}