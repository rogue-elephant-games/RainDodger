using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] bool canShoot = true;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] bool projectileSeeksPlayer = false;

    [Header("Dying")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionDuration = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip destroyedSFX;
    [SerializeField] [Range(0, 1)] float destroyedSFXVolume = 0.7f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        if (canShoot)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0f)
            {
                Shoot();
                shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            }
        }
    }

    private void Shoot()
    {
        var player = FindObjectOfType<Player>();
        Vector2 firingVector;
        if(projectileSeeksPlayer && player != null)
        {
            transform.up = Vector3.Lerp(transform.up, (player.transform.position - transform.position), 10);
            firingVector =  new Vector2(player.transform.position.x, player.transform.position.y);
        } else
            firingVector = new Vector2(0, -projectileSpeed);

        GameObject projectile = Instantiate(
            this.projectile,
            transform.position,
            Quaternion.identity
        ) as GameObject;
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
        projectile.GetComponent<Rigidbody2D>().velocity = firingVector;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
            TakeDamage(damageDealer);
    }

    private void TakeDamage(DamageDealer damageDealer)
    {
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration * 0.75f);
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
            DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(destroyedSFX, Camera.main.transform.position, destroyedSFXVolume);
    }
}
