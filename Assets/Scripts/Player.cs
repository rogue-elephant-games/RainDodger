using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float turnSpeedFactor = 30f;
    [SerializeField] float padding = 0.3f;
    [SerializeField] int health = 1000;
    [SerializeField] int bulletTime = 500;
    [SerializeField] [Range(0.1f, 0.7f)] float slowDownFactor = 0.5f;
    [SerializeField] [Range(2f, 7f)] float playerBulletTimeSpeedFactor = 5f;

    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionDuration = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.3f;

    [Header("SFX")]
    [SerializeField] AudioClip destroyedSFX;
    [SerializeField] [Range(0, 1)] float destroyedSFXVolume = 0.7f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.2f;

    Coroutine firingCoroutine;
    Coroutine bulletTimeCoroutine;
    bool isInBulletTime = false;
    float inGameMoveSpeed;
    AudioSource audioSource;
    int maxBulletTime;

    float xMin, xMax, yMin, yMax;

    void Start()
    {
        SetUpMoveBoundaries();
        inGameMoveSpeed = moveSpeed;
        maxBulletTime = bulletTime;
    }

    void Update()
    {
        Move();
        Fire();
        if (!isInBulletTime && bulletTime < maxBulletTime)
            bulletTime += 1;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuosly());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
        if (Input.GetButtonDown("Fire3") && bulletTime > 0)
        {
            bulletTimeCoroutine = StartCoroutine(BulletTime());
        }
        if (Input.GetButtonUp("Fire3") || bulletTime <= 0)
        {
            StopCoroutine(bulletTimeCoroutine);
            ResetFromBulletTime();
            isInBulletTime = false;
        }
    }

    IEnumerator BulletTime()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        inGameMoveSpeed *= playerBulletTimeSpeedFactor;

        var music = GameObject.FindWithTag("Music");
        if (music != null)
        {
            audioSource = music.GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.pitch = 1f - slowDownFactor;
        }

        while (bulletTime >= 0)
        {
            isInBulletTime = true;
            bulletTime -= 10;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void ResetFromBulletTime()
    {
        Time.timeScale = 1f;
        inGameMoveSpeed = moveSpeed;
        if (audioSource != null)
            audioSource.pitch = 1f;
    }

    IEnumerator FireContinuosly()
    {
        if (transform?.up != null)
        {
            while (true)
            {
                GameObject projectile =
                    Instantiate(laserPrefab, transform.position, transform.rotation)
                    as GameObject;
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.up.x * projectileSpeed, transform.up.y * projectileSpeed);
                AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
                yield return new WaitForSeconds(projectileFiringPeriod);
            }
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        var min = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        var max = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        if(min == null)
            Debug.Log("Game Camera Min is NULL!!!");
        else
            (xMin, xMax, yMin, yMax) = (min.x + padding, max.x - padding, min.y + padding, max.y - padding);
    }

    private void Move()
    {
        float turnSpeed = (inGameMoveSpeed * Time.deltaTime) * turnSpeedFactor;
        Func<float, string, float, float, float> getDelta = (position, axis, min, max) =>
            Mathf.Clamp(position + (Input.GetAxis(axis) * Time.deltaTime * inGameMoveSpeed), min, max);
        transform.position =
            new Vector2(
                getDelta(transform.position.x, "Horizontal", xMin, xMax),
                getDelta(transform.position.y, "Vertical", yMin, yMax));

        var rotation = Input.GetAxis("Rotate");
        if (rotation != 0)
            transform.Rotate(0, 0, rotation * turnSpeed);
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
            DestroyPlayer();
    }
    private void DestroyPlayer()
    {
        ResetFromBulletTime();
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(destroyedSFX, Camera.main.transform.position, destroyedSFXVolume);
    }

    public int GetHealth() => health;
    public int GetBulletTimeRemaining() => bulletTime;
}
