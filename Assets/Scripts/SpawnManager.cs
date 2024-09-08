using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    private const float boundary = 3f;
    private const float timer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemy()
    {
        while (GameManager.instance.isRunning)
        {
            yield return new WaitForSeconds(timer);
            int enemyIndex = Random.Range(0, enemies.Length);
            Vector2 spawnPos = new(transform.position.x, Random.Range(-boundary, boundary));
            Instantiate(enemies[enemyIndex], spawnPos, Quaternion.identity);
        }
    }
}
