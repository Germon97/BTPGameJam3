using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform shootingPosition;
    [SerializeField]
    private AudioClip shootingSound;

    [SerializeField]
    private float gunCooldown = 0.3f;

    private float currentCooldown = 0;

    private void Update()
    {
        if(currentCooldown < gunCooldown)
            currentCooldown += Time.deltaTime;
    }

    public void Shoot(Vector3 target)
    {
        Shoot(target, 5);
    }

    public void Shoot(Vector3 target, float force)
    {
        if (currentCooldown < gunCooldown)
            return;

        GlobalAudioSource.PlaySoundEffect(shootingSound);

        currentCooldown = 0;
        Vector3 direction = target - shootingPosition.position;
        GameObject newBullet = Instantiate(bulletPrefab, shootingPosition.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
}
