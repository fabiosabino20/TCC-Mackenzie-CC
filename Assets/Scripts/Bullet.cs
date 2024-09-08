using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    [SerializeField] private Animator animator;

    private const float boundary = 11.5f;
    private const float moveSpeed = 15f;

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isAlive"))
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        if (transform.position.x >= boundary)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(DestroyBullet());
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.TakeDamage(damage);
            StartCoroutine(DestroyBullet());
        }
    }

    IEnumerator DestroyBullet()
    {
        animator.SetBool("isAlive", false);
        yield return new WaitForSeconds(0.333f);
        Destroy(gameObject);
    }
}
