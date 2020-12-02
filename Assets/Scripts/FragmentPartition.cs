using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentPartition : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> _partitionSprites;

    [SerializeField] bool _ifIsRandomSprite;
    private void Start()
    {
        if (_ifIsRandomSprite)
        {
            //Debug.Log(_partitionSprites.Count);
            int random = Random.Range(0, _partitionSprites.Count);
            //Debug.Log(random);
            _spriteRenderer.sprite = _partitionSprites[random];
        }
    }
}
