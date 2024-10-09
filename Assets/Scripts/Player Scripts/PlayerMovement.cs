using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;

    private Rigidbody2D myBody;
    private Animator anim;

    public Transform groundCheckPosition;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool jumped;

    private float jumpPower = 12f; // Declare jumpPower variable

    void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        CheckIfGrounded(); // Check if the player is grounded

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) { // Jump when spacebar is pressed and player is grounded
            PlayerJump();
        }
    }

    void FixedUpdate() {
        PlayerWalk();
    }

    void PlayerWalk() {
        float h = Input.GetAxis("Horizontal"); // Get axis of movement.

        if (h > 0) {
            myBody.velocity = new Vector2(speed, myBody.velocity.y);
            ChangeDirection(1);
        } else if (h < 0) {
            myBody.velocity = new Vector2(-speed, myBody.velocity.y);
            ChangeDirection(-1);
        } else {
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
        }

        anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));
    }

    void ChangeDirection(int direction) {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    // Checking if the player is on the ground
    void CheckIfGrounded() {
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.1f, groundLayer);

        if (isGrounded) {
            if (jumped) {
                jumped = false;
                anim.SetBool("Jump", false);
            }
        }
    }

    // Make the player jump
    void PlayerJump() {
        if (isGrounded) {
            jumped = true; // Set jumped to true on each jump
            myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
            anim.SetBool("Jump", true);
        }
    }

    // Detect if the player enters the water
    void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Water")) {
        Vector2 position = transform.position; // Get the player's current position
        GameManager.Instance.PlayerEnteredWater(position); // Call the GameManager method
    }
}

}
