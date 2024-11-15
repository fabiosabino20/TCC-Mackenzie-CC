using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AudioClip explosionSoundClip;
    [SerializeField] private Animator animator;
    [SerializeField] private int life = 2;
    [SerializeField] private const float moveSpeed = 3f;

    private const float boundary = -12.5f;

    // Update is called once per frame
    void Update()
    {
        CheckBoundary();

        if (animator.GetBool("isAlive"))
            MoveLeft();
    }

    private void MoveLeft()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * moveSpeed * 25 * Time.deltaTime, Space.Self);
    }

    private void CheckBoundary()
    {
        if (transform.position.x <= boundary)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            animator.SetBool("isAlive", false);
            SoundMixerManager.instance.PlaySoudFXClip(explosionSoundClip, transform, 0.8f);
            StartCoroutine(DestroyAsteroid());
        }
    }

    private IEnumerator DestroyAsteroid()
    {
        yield return new WaitForSeconds(0.417f);
        Destroy(gameObject);
    }
}
