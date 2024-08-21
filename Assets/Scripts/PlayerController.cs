using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float engineHeat = 0;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Slider heatGauge;
    [SerializeField] private TextMeshProUGUI flowText;

    private bool isAlive = true;
    private float boundary = 3f;
    private float moveSpeed = 5f;
    private InputActions inputActions;
    private Vector2 boundaryPosition;

    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Enable();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = inputActions.Player.Move.ReadValue<Vector2>();
        moveValue.x = 0;
        transform.Translate(moveValue * moveSpeed * Time.deltaTime);

        if (transform.position.y >= boundary)
        {
            transform.position = new Vector2(transform.position.x, boundary);
        }
        else if (transform.position.y <= -boundary)
        {
            transform.position = new Vector2(transform.position.x, -boundary);
        }

        flowText.text = "Fluxo: " + engineHeat.ToString("F2") + " L/min";
    }

    IEnumerator Shoot()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(0.1f);
            if (engineHeat >= 1 && engineHeat < 3)
            {
                yield return new WaitForSeconds(0.7f);
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            else if (engineHeat >= 3 && engineHeat < 7)
            {
                yield return new WaitForSeconds(0.4f);
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            else if (engineHeat >= 7 && engineHeat < 10)
            {
                yield return new WaitForSeconds(0.8f);
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            else if (engineHeat >= 10)
            {
                isAlive = false;
                
                Destroy(gameObject);
                StopCoroutine(Shoot());
            }

            heatGauge.value = engineHeat / 10;
        }
    }
}
