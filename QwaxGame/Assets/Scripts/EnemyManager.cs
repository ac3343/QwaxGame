using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Vector3 position;
    public GameObject enemy;
    public int numEnemiesRandomRange;
    public List<GameObject> enemies;
    public List<Rect> enemyCollisions;

    private int numEnemies;
    private float enemySpread;

    // Start is called before the first frame update
    void Start()
    {
        numEnemies = Random.Range(1, numEnemiesRandomRange);
        enemySpread = numEnemies * 1.5f;
        position = transform.position;
        SetEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        transform.position = position;
    }

    void SetEnemies()
    {
        for(int i = 0; i < numEnemies; i++)
        {
            enemies.Add(Instantiate(enemy, new Vector3(position.x + Random.Range(-enemySpread,enemySpread), position.y, 0), Quaternion.identity));
            enemies[enemies.Count - 1].transform.parent = transform;
            //enemyCollisions.Add(new Rect(enemies[i].transform.position, new Vector2(2, 2)));
        }
    }
}
