using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //Fields
    public GameObject playerPrefab;
    GameObject player;
    public GameObject enemyGroupPrefab;
    List<GameObject> enemyGroups = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Sets the player variable to the created player instance
        player = Instantiate(playerPrefab, new Vector3(0, -3, 0), Quaternion.identity);

        //Adds created enemy group instance to the enemy groups list
        enemyGroups.Add(Instantiate(enemyGroupPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for collision
        CheckCollisions();
    }
    
    void CheckCollisions()
    {
        //Gets refrence to player object's script
        Player playerInfo = player.GetComponent<Player>();

        //Checks to see if the current attack collision is not zero
        if (playerInfo.CurrentAttackCollision != Rect.zero)
        {
            //Runs through the list of enemygroups
            foreach(GameObject g in enemyGroups)
            {
                //Gets a refrence to the current enemy group's script
                EnemyManager enemyGroup = g.GetComponent<EnemyManager>();

                //Runs through list of enemies in current enemy group
                for(int i = enemyGroup.enemies.Count - 1; i > 0; i--)
                {
                    //Gets a refrence to the current enemy's script
                    Enemy currentEnemy = enemyGroup.enemies[i].GetComponent<Enemy>();

                    //Checks to see if the enemy and the attack box are colliding
                    if (areColliding(playerInfo.CurrentAttackCollision, currentEnemy.collision))
                    {
                        //Gets a refrence to the enemy to be destroyed
                        GameObject destroyedEnemy = enemyGroup.enemies[i];

                        //Removes enemy from the current list of enemies
                        enemyGroup.enemies.RemoveAt(i);

                        //Destroys current enemy
                        GameObject.Destroy(destroyedEnemy);
                        
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
