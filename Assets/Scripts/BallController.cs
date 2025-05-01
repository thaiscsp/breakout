using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;
    Rigidbody2D rigidBody;
    Vector3 startPosition;
    Vector2 direction = new(-1, 1);

    public float speed;

    bool hasTouchedCeiling;

    void Start()
    {
        startPosition = transform.position;

        gameManager = FindFirstObjectByType<GameManager>();
        playerController = FindFirstObjectByType<PlayerController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Move()
    {
        rigidBody.linearVelocity = direction * speed;
    }

    private void ChangeVelocity(Collision2D collision)
    {
        Vector2 ballCenter = GetComponent<Collider2D>().bounds.center;
        Vector2 playerCenter = collision.collider.bounds.center;
        float distance = ballCenter.x - playerCenter.x;

        rigidBody.linearVelocity = new(Mathf.Sign(distance) * Mathf.Abs(rigidBody.linearVelocityX * distance * 1.1f), rigidBody.linearVelocityY);
    }

    public void ResetProperties()
    {
        rigidBody.linearVelocity = Vector2.zero;
        transform.position = startPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ChangeVelocity(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            ResetProperties();
            playerController.ResetProperties();
            gameManager.ResetRound();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Brick"))
        {
            if (collision.gameObject.activeSelf)
            {
                collision.gameObject.SetActive(false);
                rigidBody.linearVelocity *= 1.05f; // 5% increase in velocity
                playerController.Score += 100;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ceiling") && !hasTouchedCeiling)
        {
            hasTouchedCeiling = true;
            playerController.transform.localScale = new(playerController.transform.localScale.x * 0.5f, playerController.transform.localScale.y, 1);
        }
    }

}
