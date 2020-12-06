using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [SerializeField] Sprite[] foodSprite;
    [SerializeField] Sprite weaponSprite;
    [SerializeField] AudioClip[] pickupSFX;
    [SerializeField] bool isWeapon;

    SpriteRenderer currentSprite;
    GameSession gameSession;
    private void Awake()
    {
        FindObjectOfType<GameSession>().CountCollectibles();
    }
    void Start () {
        currentSprite = GetComponent<SpriteRenderer>();
        if (isWeapon) { currentSprite.sprite = weaponSprite; }
        else { currentSprite.sprite = foodSprite[Random.Range(0, foodSprite.Length)]; }
        gameSession = FindObjectOfType<GameSession>();

	}

    public void CountCollectibles()
    {
        gameSession.CountCollectibles();
    }

    public bool GetWeapon()
    {
        if (isWeapon)
        {
            return true;
        }
        else { return false; }
    }

    public void PlaySFX()
    {
        AudioSource.PlayClipAtPoint(pickupSFX[Random.Range(0, pickupSFX.Length)], Camera.main.transform.position);
    }

}
