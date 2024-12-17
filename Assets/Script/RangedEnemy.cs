using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject enemyBulletPrefab;
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    private float timeToFire;
    public float fireRate;
    public Transform firePoint;

    public AudioClip shootSFX;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (target == null)
        {
            GetTarget();
            return;
        }

        if (Vector2.Distance(target.position, transform.position) <= distanceToShoot)
        {
            Shoot();
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
    protected override void FixedUpdate()
    {
        if (target == null)
            return;

        base.FixedUpdate();

        if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        
        
    }

    protected override void GetTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure an object with tag 'Player' exists in the scene.");
        }
    }


    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.gameObject.CompareTag("Bullet"))
        {
            LevelManager.manager.IncreaseScore(3);
            Destroy(other.gameObject);
            AudioManager.instance.PlaySFX(hitsfx);
            Destroy(gameObject);
        }

    }
}
