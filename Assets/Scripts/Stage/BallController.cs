using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;
    Rigidbody2D rigidBody;
    SFXManager sfxManager;
    Vector3 startPosition;
    Vector2 direction = new(-1, 1);
    bool increasingVelocity, increasingScore, hasTouchedCeiling;
    float maxSpeed = 8.144f;
    public float speed = 5;

    void Start()
    {
        startPosition = transform.position;

        RetrieveComponents();
    }

    private void RetrieveComponents()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerController = FindFirstObjectByType<PlayerController>();
        rigidBody = GetComponent<Rigidbody2D>();
        sfxManager = FindFirstObjectByType<SFXManager>();
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
        float direction = Mathf.Sign(distance);
        float xVelocity = direction * Mathf.Abs(Mathf.Clamp(rigidBody.linearVelocityX, speed, maxSpeed));
        float yVelocity = Mathf.Clamp(rigidBody.linearVelocityY, speed, maxSpeed);

        rigidBody.linearVelocity = new(xVelocity, yVelocity);
    }

    public void ResetProperties()
    {
        rigidBody.linearVelocity = Vector2.zero;
        transform.position = startPosition;
        speed = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        sfxManager.PlayClip(sfxManager.hit);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ChangeVelocity(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            sfxManager.PlayClip(sfxManager.reset);

            ResetProperties();
            playerController.ResetProperties();
            gameManager.ResetRound();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Brick"))
        {
            GameObject brick = collision.gameObject;

            int points = brick.GetComponent<BrickController>().points;
            brick.SetActive(false);

            if (!increasingVelocity) StartCoroutine(IncreaseVelocity());
            StartCoroutine(IncreaseScore(points));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ceiling") && !hasTouchedCeiling)
        {
            ReducePaddleSize();
        }
    }

    private IEnumerator IncreaseScore(int points)
    {
        if (!increasingScore)
        {
            increasingScore = true;

            playerController.Score += points;
            yield return null;

            increasingScore = false;
        }
    }

    private IEnumerator IncreaseVelocity()
    {
        if (!increasingVelocity && speed <= maxSpeed)
        {
            increasingVelocity = true;

            speed *= 1.05f; // 5% increase
            yield return null;

            increasingVelocity = false;
        }
    }

    private void ReducePaddleSize()
    {
        hasTouchedCeiling = true;
        playerController.transform.localScale = new(playerController.transform.localScale.x * 0.7f, playerController.transform.localScale.y, 1);
    }

}
