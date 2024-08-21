using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private float boundary = -12.5f;
    private float moveSpeed = 3f;
    private int life = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= boundary)
        {
            Destroy(gameObject);
        }
        //MoveLeft();
    }

    private void MoveLeft()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * moveSpeed * 25 * Time.deltaTime, Space.Self);
    }

    public void TakeDamage()
    {
        life -= 1;
        if (life == 0)
        {
            animator.SetInteger("life", life);
            StartCoroutine(Demolish());
        }
    }

    private IEnumerator Demolish()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
