using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public float engineHeat = 0;

    [SerializeField] private AudioClip flashSoundClip;
    [SerializeField] private AudioClip explosionSoundClip;
    [SerializeField] private const float moveSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Slider heatGauge;
    [SerializeField] private Image heatGaugeColor;
    [SerializeField] private GameObject flashSprite;

    private const float boundary = 3.5f;
    private InputActions inputActions;
    private Animator animator;

    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Enable();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        heatGaugeColor.color = Color.grey;
        heatGauge.value = 0;
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBoundary();
    }

    private void Move()
    {
        if (isAlive)
        {
            Vector2 moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveValue.x = 0;

            if (moveValue.y > 0 && !animator.GetBool("MovingUp"))
            {
                animator.SetTrigger("TrUp");
                animator.SetBool("MovingUp", true);
                animator.SetBool("MovingDown", false);
            }
            else if (moveValue.y < 0 && !animator.GetBool("MovingDown"))
            {
                animator.SetTrigger("TrDown");
                animator.SetBool("MovingDown", true);
                animator.SetBool("MovingUp", false);
            }
            else if (moveValue.y == 0)
            {
                animator.SetBool("MovingDown", false);
                animator.SetBool("MovingUp", false);
            }

            transform.Translate(moveValue * moveSpeed * Time.deltaTime);

        }
    }

    private void CheckBoundary()
    {
        if (transform.position.y >= boundary)
        {
            transform.position = new Vector2(transform.position.x, boundary);
        }
        else if (transform.position.y <= -boundary)
        {
            transform.position = new Vector2(transform.position.x, -boundary);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.TakeDamage(99);

            StartCoroutine(DestroyShip());           
        }
    }

    private IEnumerator DestroyShip()
    {
        StopCoroutine(Shoot());

        animator.SetBool("MovingDown", false);
        animator.SetBool("MovingUp", false);
        animator.SetBool("IsAlive", false);
        isAlive = false;

        yield return new WaitForSeconds(0.417f);

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        animator.enabled = false;

        StartCoroutine(GameManager.instance.ReturnToMainMenu());
        //Destroy(gameObject);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1);
        while (isAlive)
        {
            yield return new WaitForSeconds(0.1f);
            if (engineHeat<0.2f)
            {
                heatGaugeColor.color = Color.grey;
                heatGauge.value = 0;
            }
            else if (engineHeat >= 0.2f && engineHeat < 1.0f)
            {
                yield return new WaitForSeconds(0.85f);
                heatGaugeColor.color = Color.yellow;
            }
            else if (engineHeat >= 1.0f && engineHeat < 2.0f)
            {
                yield return new WaitForSeconds(0.425f);
                heatGaugeColor.color = Color.green;
            }
            else if (engineHeat >= 2.0f && engineHeat < 3.0f)
            {
                yield return new WaitForSeconds(1f);
                heatGaugeColor.color = Color.red;
            }
            else if (engineHeat >= 3.0f)
            {
                StartCoroutine(DestroyShip());
            }

            if (isAlive && engineHeat>=0.2f)
            {
                Vector2 spawnPos = new(transform.position.x + 1.4f, transform.position.y);
                Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
                SoundMixerManager.instance.PlaySoudFXClip(flashSoundClip, transform, 0.8f);
                StartCoroutine(FlashAnimation());
                heatGauge.value = engineHeat / 3;
            }
        }
    }

    private IEnumerator FlashAnimation()
    {
        flashSprite.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        flashSprite.SetActive(false);
    }
}
