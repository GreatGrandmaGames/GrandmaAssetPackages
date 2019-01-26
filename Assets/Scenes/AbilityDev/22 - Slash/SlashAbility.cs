using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;
public class SlashAbility : Ability
{
    //use "cooldown" system for castTime
    public float castTimeSeconds = 2f;
    private bool casting = false;

    /*
     * overall premise here is this
     * we have a cast time, say 3 seconds.
     * during this cast time, the player will
     * a. be in stun lock
     * b. not be able to use any other abilities
     * c. can take damage, which interrupts the cast time, 
     * and is out of stun lock
     * d. not take any damage from character they're attacking 
     * e. make "noise" (others will notice)   
     * the enemy will
     * a. be in stun lock
     * b. not be able to use their abilities
     * c. if the cast time completes, enemy will die
     * d. if the cast time is interrupted, they can attack the player
    */

    public override void Activate()
    {
        Debug.Log("IM SLASHING!");
        //KillEnemy();
        casting = true;
    }
    public void KillEnemy(Damageable d)
    {

    }

    public override bool WillActivate()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

}
