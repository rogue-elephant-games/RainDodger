using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.3f;

    float xMin, xMax, yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        var min = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0));
        var max = gameCamera.ViewportToWorldPoint(new Vector3(1,1,0));
        (xMin, xMax, yMin, yMax) = (min.x +padding, max.x -padding, min.y + padding, max.y - padding);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
