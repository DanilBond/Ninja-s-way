using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy : MonoBehaviour
{
    public string State;
    public float damping;

    public LayerMask a;
    public bool isPlayerVisible;

    public float attackInterval;
    public float attackIntervalMin;
    public float attackIntervalMax;

    public float findDistance;

    public float attackDistance;
    public float viewAngle;
    public float viewDistance;

    private NavMeshAgent Nma;

    private Transform target;

    private float distance;

    private Animator anim;

    public GameObject Sword;
    private void Start()
    {
        Nma = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(checkDistanceAndPlayerView());
    }
    private void Update()
    {
        if(attackInterval > 0f)
        {
            attackInterval -= Time.deltaTime;
        }
        if (distance > findDistance)
        {
            Nma.SetDestination(target.position);
        }
        if (distance <= attackDistance)
        {
            if(attackInterval <= 0f)
            {
                Sword.GetComponent<BoxCollider>().enabled = true;
                int rand = Random.Range(0, 1000);
                if (rand >= 400f)
                {
                    anim.SetTrigger("Attack_" + Random.Range(1, 3));
                }
                else
                {
                    int Rand = Random.Range(1, 3);
                    anim.SetTrigger("Combo_" + 1 + "_" + Rand);
                }
                attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
            }
        }
        else
        {
            Sword.GetComponent<BoxCollider>().enabled = false;
        }
        if (isPlayerVisible)
        {
            LookAtPlayer();
        }
        anim.SetFloat("X", Nma.velocity.x);
        anim.SetFloat("Y", Mathf.Abs(Nma.velocity.z));
    }
    void LookAtPlayer()
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    bool findThePlayer()
    {
        if (Vector3.Distance(transform.position, target.position) < viewDistance)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2)
            {

                if (!Physics.Linecast(transform.position, target.position, a))
                {
                    return true;
                }

            }
        }
        return false;
    }

    IEnumerator checkDistanceAndPlayerView()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            distance = Vector3.Distance(transform.position, target.position);
            isPlayerVisible = findThePlayer();
        }
    }
}
