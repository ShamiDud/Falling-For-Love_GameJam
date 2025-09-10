using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject liquidLovePrefabs;
    [SerializeField] private GameOverManager gameOverManager;

    [Header("Movement Variables")]
    private float moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxFallSpeed = -10f;

    [Header("Score Variables")]
    [SerializeField] private int totalScore = 0;
    [SerializeField] private int liquidLoveScoreValue = 10;

    [Header("Sprite Flip")]
    [SerializeField] private Transform sprite;
    private bool facingRight = true;

    void Update() {
        // Get Movement Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip the sprite based on movement direction
        if (moveInput > 0 && !facingRight) {
            Flip();
        }
        else if (moveInput < 0 && facingRight) {
            Flip();
        }
    }

    void FixedUpdate() {
        // Move the player
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Clamp fall speed
        if (rb.linearVelocity.y < maxFallSpeed) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("LiquidLove")) {
            // Add score and update UI
            totalScore += liquidLoveScoreValue;
            scoreText.text = "Score: " + totalScore;

            // Get the position of the collected item
            Vector2 position = collider.transform.position;

            // Remove the collected item
            Destroy(collider.gameObject);

            // Respawn collected item after delay
            StartCoroutine(OnCollectedLiquidLove(position));
        }
        else if (collider.CompareTag("Portal")) {
            // Teleport player back to the top
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            // Reset vertical velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        else if (collider.CompareTag("Obstacle")) {
            // Activate Game Over Screen
            gameOverManager.Setup(totalScore);
            // Disable player movement
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    IEnumerator OnCollectedLiquidLove(Vector2 position) {
        // wait for 1 second before respawning
        yield return new WaitForSeconds(1f);
        Instantiate(liquidLovePrefabs, position, Quaternion.identity);
    }

    // Flip the player sprite
    private void Flip() {
        facingRight = !facingRight;

        Vector3 localScale = sprite.localScale;
        localScale.x *= -1f;
        sprite.localScale = localScale;
    }
}
