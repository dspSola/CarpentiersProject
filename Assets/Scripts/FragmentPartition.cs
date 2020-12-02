using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentPartition : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> _partitionSprites;

    public void SetRandomSprite()
    {
        int randomSprite = Random.Range(0, _partitionSprites.Count);
        Debug.Log("Random Sprite = " + randomSprite);

        _spriteRenderer.sprite = _partitionSprites[randomSprite];
    }

    public void SetRandomRotation()
    {
        int randomRotation = Random.Range(0, 4);
        Debug.Log("Random Rotation = " + randomRotation);

        if (randomRotation == 0)
        {
            _spriteRenderer.flipX = false;
        }
        if (randomRotation == 1)
        {
            _spriteRenderer.flipX = true;
        }
        if (randomRotation == 2)
        {
            _spriteRenderer.flipY = false;
        }
        if (randomRotation == 3)
        {
            _spriteRenderer.flipY = true;
        }
    }
}
