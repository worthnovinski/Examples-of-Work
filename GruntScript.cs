using UnityEngine;
using UnityEngine.InputSystem;

public class GruntScript : Enemy
{
    public GameObject modlG;
    public Animator animator;

    public GameObject manualPlayerInputG = null;
    public int movementSpeedG = 5;

    public int attackG = 85;
    public GruntScript() {
        maxHealth = 10;

        manualPlayerInput = manualPlayerInputG;
        personalhealth = maxHealth;
        attacking = false;
        moving = false;
        model = modlG;
        anima = animator;
        movementSpeed = movementSpeedG;
        attack = attackG;
    }
    void setThingsUp() {
        maxHealth = 900;
        manualPlayerInput = manualPlayerInputG;
        personalhealth = maxHealth;
        attacking = false;
        moving = false;
        model = modlG;
        anima = animator;
        movementSpeed = movementSpeedG;
        alive = true;

    }
    void Start()
    {
        setThingsUp();
        if (manualPlayerInput != null)
        {
            players = new GameObject[] { manualPlayerInput };

        }

    }

    void Update()
    {
        if (personalhealth > 0 && alive)
        {
            approach();
        }

        else if(alive)die();
        



    }

    

    

}
