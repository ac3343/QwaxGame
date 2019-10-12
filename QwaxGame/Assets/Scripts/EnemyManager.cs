using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Vector3 position;
    public GameObject enemy;
    public int numEnemies;
    public int enemySpread;
    public List<GameObject> enemies;
    public List<Rect> enemyCollisions;

    // Start is called before the first frame update
    void Start()
    {
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
            enemies.Add(Instantiate(enemy, new Vector3(position.x + Random.Range(-enemySpread,enemySpread), 0, 0), Quaternion.identity));
            enemies[enemies.Count - 1].transform.parent = transform;
            //enemyCollisions.Add(new Rect(enemies[i].transform.position, new Vector2(2, 2)));
        }
    }
}
