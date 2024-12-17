using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.02f;
    public Rigidbody2D rb;

    public GameObject enemyBulletPrefab;
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    private float timeToFire;
    public float fireRate;
    public Transform firePoint;

    public AudioClip hitsfx;
    public AudioClip playerHit;
    public AudioClip shootSFX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();

            if (Vector2.Distance(target.position, transform.position) <= distanceToShoot)
            {
                Shoot();
            }
        }

        
    }

    private void Shoot()
    {
        if (timeToFire <= 0f)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
            AudioManager.instance.PlaySFX(shootSFX);
            timeToFire = fireRate;
        }
        else
        {
            timeToFire -= Time.deltaTime;
        }

    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
            {
                rb.velocity = transform.up * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }
    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.manager.GameOver();
            AudioManager.instance.PlaySFX(playerHit);
            Destroy(other.gameObject);
            target = null;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            LevelManager.manager.IncreaseScore(3);
            Destroy(other.gameObject);
            AudioManager.instance.PlaySFX(hitsfx);
            Destroy(gameObject);
        }

    }
}
