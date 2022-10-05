using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private float jumpForce;

    [Header("Grounded")]
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float checkGroundRadius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Obstacle and Reward")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask rewardLayer;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip foodSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip winSound;

    [Header("UI")]
    [SerializeField] private TMPro.TMP_Text scoreText;

    private Rigidbody2D rb;
    private Animator anim;
    private int score;

    private bool IsGrounded => Physics2D.OverlapCircle(groundPoint.position, checkGroundRadius, groundLayer);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsGrounded)
        {
            // jump
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
        audioSource.PlayOneShot(jumpSound);
        anim.SetTrigger("Jump");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            ((1 << other.gameObject.layer) & obstacleLayer) != 0)
        {
            audioSource.PlayOneShot(winSound);
            Destroy(gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if (
            ((1 << other.gameObject.layer) & rewardLayer) != 0)
        {
            score++;
            scoreText.text = "" + score;
            audioSource.PlayOneShot(foodSound);
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, checkGroundRadius);
    }
}
