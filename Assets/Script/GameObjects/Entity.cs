using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool isAlive = true;
    private bool isHit = false;

    private int health = 100;

    public bool IsAlive
    {
        get
        { 
            return isAlive;
        }

        set
        { 
            isAlive = value;
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

    public int Health
    {
        get
        { 
            return health;
        }

        set
        { 
            health = value;

            if (health <= 0)
            {
                health = 0;
                isAlive = false;
            }
        }
    }
}
