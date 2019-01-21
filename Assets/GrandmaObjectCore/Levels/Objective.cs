using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Grandma
{
    public abstract class Objective : GrandmaComponent
    {
        public Action OnComplete;
    }


    public interface IObjectiveDependent
    {
        void ObjectiveBegin();
    }

    /*create some sort of "middleware class" that is called "objective dependent"
    which means any gameobject with a variety of states has at least one state
    in which the objective is dependent on. this class then goes through every
    objective dependent item in the scene / level and checks for its state. */

    //example of a type of objective
    public class KillAllEnemies : Objective
    {
        private List<Damageable> enemies = new List<Damageable>();

        public void SetEnemyList(List<Damageable> damageables)
        {
            enemies.AddRange(damageables);
            enemies.ForEach(x =>
            {
                x.OnDestroyed += () =>
                {
                    enemies.Remove(x);
                    if (enemies.Count <= 0)
                    {
                        OnComplete?.Invoke();
                    }
                };
            });
        }
    }
}