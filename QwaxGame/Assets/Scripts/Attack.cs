using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    public enum AttackStates
    {
        WindUp,
        Attack,
        CoolDown
    }
    //Fields
    int windUpFrames;
    int attackFrames;
    int coolDownFrames;
    public AttackStates currentAttackState;
    int totalFrames;

    // Start is called before the first frame update
    void Start()
    {
        totalFrames = windUpFrames + attackFrames + coolDownFrames;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Attack()
    {
        
    }

    public bool AttackFinished(int currentFrame)
    {
        if(currentFrame > totalFrames)
        {
            return true;
        }
        else
        {
            if(currentFrame < windUpFrames)
            {
                currentAttackState = AttackStates.WindUp;
            }
            else if(currentFrame < windUpFrames + attackFrames)
            {
                currentAttackState = AttackStates.Attack;
            }
            else if(currentFrame < totalFrames)
            {
                currentAttackState = AttackStates.CoolDown;
            }

            return false;
        }
    }

    public void GiveFrames(int windUp, int attack, int coolDown)
    {
        windUpFrames = windUp;
        attackFrames = attack;
        coolDownFrames = coolDown;
        totalFrames = windUpFrames + attackFrames + coolDownFrames;
    }
}
