using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int maxHealth;
    public bool singleplayer;
    public int playersInGame;
    public GameObject[] players;
    public GameObject manualPlayerInput;
    public int personalhealth;
    public bool attacking;
    public bool moving;
    public GameObject model;
    public Animator anima;
    public int movementSpeed;
    public bool alive;
    public int attack;
    public int attackLength = 1000000;
    public void TakeDamage(float damage) { 
        personalhealth = personalhealth - (int)damage;
        Debug.Log("enemy is being damaged");
        anima.Play("Hit");
    
    }
    public bool isAlive() {
        return alive;
    
    }

    public Enemy() {
        maxHealth = 10;

        manualPlayerInput = null;
        personalhealth = maxHealth;
        attacking = false;
        moving = false;
        model = null;
        anima = null;
        movementSpeed = 5;
        alive = true;
    }

    public Enemy(int speed, GameObject modl, Animator animator, int hPMax) { 
        movementSpeed = speed;
        model = modl;
        anima = animator;
        maxHealth = hPMax;
    
    }

    void Update()
    {
        
    }
    public void setSinglePlayerFromSceneManager(bool islone)
    {
        singleplayer = islone;
    }

    public void setPlayers(GameObject playerOnly)
    {
        singleplayer = true;
        players = new GameObject[] { playerOnly };
    }

    public void setPlayers(GameObject[] fellas)
    {
        players = fellas;
        playersInGame = players.Length;
    }
    public GameObject nearestPlayer()
    {
        GameObject near = players[0];

        for (int i = 1; i < players.Length; i++)
        {
            if (Vector3.Distance(near.transform.position, gameObject.transform.position) > Vector3.Distance(players[i].transform.position, gameObject.transform.position))
            {
                near = players[i];


            }

        }


        return near;
    }
    public void approach() {
        Vector3 directionR = (transform.position - nearestPlayer().transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionR);
        transform.rotation = targetRotation;

        if (Vector3.Distance(nearestPlayer().transform.position, transform.position) < 4)
        {
            moving = false;
            attacking = true;
            anima.Play("Attack_1");
            
            if (attackLength > 0)
            {
                attackLength--;
            }
            else if (attackLength == 0)
            {
                nearestPlayer().GetComponent<PlayerInteractor>().dealDamage(attack);

                attackLength = 175;
            }


        }
        else
        {
            Vector3 direction = (nearestPlayer().transform.position - transform.position).normalized;
            attacking = false;
            moving = true;
            transform.position += direction * movementSpeed * Time.deltaTime;

        }
        anima = model.GetComponent<Animator>();
        anima.SetBool("Moving", moving);
        anima.SetBool("Attacking", attacking);
    }

    public void die() { 
        alive = false;
        anima.Play("Death");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;


    }
}
