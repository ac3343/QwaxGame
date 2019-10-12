using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;
    public GameObject enemyGroupPrefab;
    List<GameObject> enemyGroups = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, -3, 0), Quaternion.identity);
        enemyGroups.Add(Instantiate(enemyGroupPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisions();
    }

    void CreateCollisionBox(Rect boxRect)
    {

    }
    
    void CheckCollisions()
    {
        Player playerInfo = player.GetComponent<Player>();
        if (playerInfo.CurrentAttackCollision != Rect.zero)
        {
            foreach(GameObject g in enemyGroups)
            {
                EnemyManager enemyGroup = g.GetComponent<EnemyManager>();

                for(int i = 0; i < enemyGroup.enemyCollisions.Count; i++)
                {
                    if (areColliding(playerInfo.CurrentAttackCollision, enemyGroup.enemyCollisions[i]))
                    {
                        GameObject.Destroy(enemyGroup.enemies[i]);
                    }
                }
            }
        }
    }

    bool areColliding(Rect box1, Rect box2)
    {
        return box1.min.x < box2.max.x &&
            box1.min.y < box2.max.y &&
            box2.min.x < box1.max.x &&
            box2.min.y < box1.max.y;
    }

}
