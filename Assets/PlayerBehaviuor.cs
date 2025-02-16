using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviuor : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed;
    float speedX, speedY;

    [Header("Key Settings")]
    [SerializeField] KeyCode dashKey;

    private Rigidbody2D rigd;

    private void Start()
    {
        rigd = GetComponent<Rigidbody2D>();
    }
    private void MoveBehaviour(float speed)
    {
        speedX = Input.GetAxisRaw("Horizontal") * speed;
        speedY = Input.GetAxisRaw("Vertical") * speed;

        rigd.velocity = new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime);
    }
    private void Update()
    {
        MoveBehaviour(moveSpeed);
        //dash
        if (Input.GetKeyDown(dashKey))
            MoveBehaviour(moveSpeed * 100);
    }
}
