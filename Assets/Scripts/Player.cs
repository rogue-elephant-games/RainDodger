using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.3f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.3f;

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
            GameObject laser =
                Instantiate(laserPrefab, transform.position, Quaternion.identity)
                as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
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
}
