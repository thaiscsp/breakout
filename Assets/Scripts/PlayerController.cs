using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    BallController ballController;
    GameManager gameManager;
    Rigidbody2D rigidBody;
    Vector2 startPosition;
    Vector2 direction;

    public int Lives { get; set; } = 3;
    public int Score { get; set; } = 0;

    public float speed = 7;

    void Start()
    {
        startPosition = transform.position;

        ballController = FindFirstObjectByType<BallController>();
        gameManager = FindFirstObjectByType<GameManager>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveHorizontally();   
    }

    private void MoveHorizontally()
    {
        if (gameManager.RoundStarted) rigidBody.linearVelocity = direction * speed;
    }

    public void ResetStats()
    {
        Lives = 3;
        Score = 0;
    }

    public void ResetProperties()
    {
        direction = Vector2.zero;
        rigidBody.linearVelocity = Vector2.zero;
        transform.position = startPosition;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (gameManager.RoundStarted) direction = context.ReadValue<Vector2>();
    }

    public void Push(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.RoundStarted)
        {
            ballController.Move();
            gameManager.RoundStarted = true;
        }
    }

}
