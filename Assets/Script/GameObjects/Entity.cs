using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool isHit = false;
    private bool isAttack = false;

    private float health = 100;

    public bool IsAlive
    {
        get
        {
            return health > 0;
        }
    }

    public bool IsHit
    {
        get
        { 
            return isHit;
        }

        set
        { 
            isHit = value;
        }
    }

    public bool IsAttack
    {
        get
        {
            return isAttack;
        }

        set
        {
            isAttack = value;
        }
    }

    public float Health
    {
        get
        { 
            return health;
        }

        set
        { 
            health = value;

            if (health < 0.0f)
            {
                health = 0.0f;
            }
        }
    }
}
