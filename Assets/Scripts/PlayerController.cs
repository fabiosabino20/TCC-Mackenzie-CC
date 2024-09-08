using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public float engineHeat = 0;

    [SerializeField] private AudioClip flashSoundClip;
    [SerializeField] private AudioClip explosionSoundClip;
    [SerializeField] private const float moveSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Slider heatGauge;
    [SerializeField] private TextMeshProUGUI flowText;
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

    private void UpdateFlowText()
    {
        flowText.text = "Fluxo: " + engineHeat.ToString("F2") + " L/min";
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
            if (engineHeat >= 0 && engineHeat < 3)
            {
                yield return new WaitForSeconds(0.85f);
            }
            else if (engineHeat >= 3 && engineHeat < 7)
            {
                yield return new WaitForSeconds(0.425f);
            }
            else if (engineHeat >= 7 && engineHeat < 10)
            {
                yield return new WaitForSeconds(1f);
            }
            else if (engineHeat >= 10)
            {
                StartCoroutine(DestroyShip());
            }

            if (isAlive)
            {
                Vector2 spawnPos = new(transform.position.x + 1.4f, transform.position.y);
                Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
                GameManager.instance.PlaySoudFXClip(flashSoundClip, transform, 0.8f);
                StartCoroutine(FlashAnimation());
                heatGauge.value = engineHeat / 10;
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
