using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public bool isDesktop;
    
    public Animator anim;

    public Transform target;

    public float damping;
    public float speed;

    public float maxHitTimeout;
    public int hitCount_D;
    bool canActivateTimer_D;
    public float hitTimer_D = 0f;

    public int hitCount_A;
    bool canActivateTimer_A;
    public float hitTimer_A = 0f;

    Joystick joystick;
    public float hor;
    public float ver;


    public bool canGetHit = true;
    public float getHitTimeout;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    
    void Update()
    {
        Movement();
        
        if (canActivateTimer_D)
        {
            hitTimer_D += Time.deltaTime;
            if(hitTimer_D >= maxHitTimeout)
            {
                Attack_D(hitCount_D);
                hitTimer_D = 0f;
                canActivateTimer_D = false;
                hitCount_D = 0;
                
            }
        }

        if (canActivateTimer_A)
        {
            hitTimer_A += Time.deltaTime;
            if (hitTimer_A >= maxHitTimeout)
            {
                Attack_A(hitCount_A);
                hitTimer_A = 0f;
                canActivateTimer_A = false;
                hitCount_A = 0;

            }
        }
    }
    float y = 0;
    float x = 0;
    void Movement()
    {
        if (!anim.applyRootMotion)
        {
            if (isDesktop)
            {
                hor = joystick.Horizontal;
                ver = joystick.Vertical;
                if (joystick.Vertical < 0f)
                {
                    float v = joystick.Vertical * 10f;
                    anim.SetFloat("Y", v);
                }
                else
                {
                    anim.SetFloat("Y", ver);
                }
            }
            else
            {
                hor = Input.GetAxis("Horizontal");
                ver = Input.GetAxis("Vertical");
                if (ver < 0f)
                {
                    float v = ver * 10f;
                    anim.SetFloat("Y", v);
                }
                else
                {
                    anim.SetFloat("Y", ver);
                }
            }


            anim.SetFloat("X", hor);
           


            gameObject.transform.position += transform.forward * ver * speed * Time.deltaTime;
            gameObject.transform.position += transform.right * hor * speed * Time.deltaTime;


            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }
    public void AttackTouchDefault()
    {
    //    speed = 0;
    //    anim.applyRootMotion = true;
        canActivateTimer_D = true;
        hitCount_D++;
        hitTimer_D = 0f;
    }

    public void AttackTouchAdvanced()
    {

        //speed = 0;
        //anim.applyRootMotion = true;
        canActivateTimer_A = true;
        hitCount_A++;
        hitTimer_A = 0f;
    }

    public void Attack_D(int type)
    {
        if (anim.applyRootMotion == false)
        {
            anim.SetTrigger("Attack_" + type);
            StartCoroutine(animatorPhysics(1f));
        }
    }
    public void Attack_A(int type)
    {
        if (anim.applyRootMotion == false)
        {
            int rand = Random.Range(1, 3);
            anim.SetTrigger("Combo_" + type + "_" + rand);
            StartCoroutine(animatorPhysics(1.8f));
        }
    }

    IEnumerator animatorPhysics(float time)
    {
        anim.applyRootMotion = true;
        yield return new WaitForSeconds(time);
        anim.applyRootMotion = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && other.gameObject.name != "MySword")
        {
            if (canGetHit)
            {
                canGetHit = false;
                // anim.SetTrigger("GetHit_" + Random.Range(1, 5));
                anim.Play("GetHit_" + Random.Range(1, 5));
                anim.applyRootMotion = true;

                Invoke("SetCanGetHit", getHitTimeout);
                Debug.Log("Hit");
            }
        }
    }

    void SetCanGetHit()
    {
        canGetHit = true;
        anim.applyRootMotion = false;
    }
}
