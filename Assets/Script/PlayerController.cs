using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon;

    public AudioSource src;
    public AudioClip shootsfx;
    public AudioClip hitsfx;


    private Vector2 moveDirection;
    private Vector2 mousePosition;

    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            weapon.Fire();
            src.clip = shootsfx;
            src.Play();
            fireTimer = weapon.fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet")){
            AudioManager.instance.PlaySFX(hitsfx);
            LevelManager.manager.GameOver();
            Destroy(gameObject);
        }
    }
}
