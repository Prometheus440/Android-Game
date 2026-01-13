using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float movementSpeed = 5f;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
    }
}