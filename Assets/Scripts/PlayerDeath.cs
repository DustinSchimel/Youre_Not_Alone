using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    private void Die()
    {
        //Stop the player from moving and end the game
        rb.bodyType = RigidbodyType2D.Static;
        FindObjectOfType<GameManager>().GameOver();

    }
}
