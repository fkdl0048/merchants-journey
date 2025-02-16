using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviuor : MonoBehaviour
{
    [Header("Move Setting")]
    [SerializeField] float moveSpeed;
    float speedX, speedY;

    private Rigidbody2D rigd;

    private void Start()
    {
        rigd = GetComponent<Rigidbody2D>();
    }
    private void MoveBehaviour()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;

        rigd.velocity = new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime);
    }
    private void Update()
    {
        MoveBehaviour();
    }
}
