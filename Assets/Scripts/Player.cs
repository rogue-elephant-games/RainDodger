using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.3f;
    [SerializeField] int health = 200;

    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionDuration = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.3f;

    [Header("SFX")]
    [SerializeField] AudioClip destroyedSFX;
    [SerializeField][Range(0,1)] float destroyedSFXVolume = 0.7f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField][Range(0,1)] float laserSFXVolume = 0.2f;

    Coroutine firingCoroutine;

    float xMin, xMax, yMin, yMax;

    void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuosly());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuosly()
    {
        while(true)
        {
            GameObject projectile =
                Instantiate(laserPrefab, transform.position, Quaternion.identity)
                as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        var min = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0));
        var max = gameCamera.ViewportToWorldPoint(new Vector3(1,1,0));
        (xMin, xMax, yMin, yMax) = (min.x +padding, max.x -padding, min.y + padding, max.y - padding);
    }

    private void Move()
    {
        Func<float, string, float, float, float> getDelta = (position, axis, min, max) =>
            Mathf.Clamp(position + (Input.GetAxis(axis) * Time.deltaTime * moveSpeed), min, max);
        transform.position =
            new Vector2(
                getDelta(transform.position.x, "Horizontal", xMin, xMax),
                getDelta(transform.position.y, "Vertical", yMin, yMax));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
            TakeDamage(damageDealer);
    }

    private void TakeDamage(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
            DestroyPlayer();
    }
    private void DestroyPlayer()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(destroyedSFX, Camera.main.transform.position, destroyedSFXVolume);
    }

    public int GetHealth() => health;
}
