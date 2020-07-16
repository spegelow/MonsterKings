using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on ProjectileMover script from toon projectile pack
public class Projectile : MonoBehaviour
{
    //Base characteristics
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    //Gameplay characteristics
    ActionHandler eh;
    Vector3 direction;
    public float remainingHealth;

    public List<Projectile> collidingProjectiles;

    public void Initialize(ActionHandler eh)
    {
        rb = GetComponent<Rigidbody>();
        this.eh = eh;
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }

        //Calculate the move direction of this projectile
        direction = (eh.tile.transform.position - eh.user.currentTile.transform.position).normalized;

        //set the starting health
        remainingHealth = eh.action.health;

        collidingProjectiles = new List<Projectile>();

        //Destroy(this.gameObject, 5);
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            //rb.velocity = transform.forward * speed;
            transform.position += direction * (speed * Time.deltaTime);
        }
    }
    
    //SWITCHED TO TRIGGERS INSTEAD OF COLLIDERS
    //void OnCollisionEnter(Collision collision)
    //{
    //    //Ignore the user of the move
    //    if (collision.gameObject.GetComponent<BattleUnit>() != null && collision.gameObject.GetComponent<BattleUnit>().Equals(eh.user))
    //    {
    //        return;
    //    }
    //    //Lock all axes movement and rotation
    //    rb.constraints = RigidbodyConstraints.FreezeAll;
    //    speed = 0;

    //    ContactPoint contact = collision.contacts[0];
    //    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //    Vector3 pos = contact.point + contact.normal * hitOffset;

    //    if (hit != null)
    //    {
    //        var hitInstance = Instantiate(hit, pos, rot);
    //        if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
    //        else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
    //        else { hitInstance.transform.LookAt(contact.point + contact.normal); }

    //        var hitPs = hitInstance.GetComponent<ParticleSystem>();
    //        if (hitPs != null)
    //        {
    //            Destroy(hitInstance, hitPs.main.duration);
    //        }
    //        else
    //        {
    //            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
    //            Destroy(hitInstance, hitPsParts.main.duration);
    //        }
    //    }
    //    foreach (var detachedPrefab in Detached)
    //    {
    //        if (detachedPrefab != null)
    //        {
    //            detachedPrefab.transform.parent = null;
    //        }
    //    }
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        //Check what we collided with and handle it differently depending on what we hit
        if (other.gameObject.GetComponent<BattleUnit>() != null)
        {
            BattleUnit bu = other.gameObject.GetComponent<BattleUnit>();
            //Ignore the user of the move
            if (bu.Equals(eh.user))
            {
                return;
            }
            else
            {
                //We hit another creature! Apply damage and destroy the projectile
                speed = 0;
                bu.ApplyDamage(eh.action.damage);

                CreateOnHitEffect();
            }
        }
        else if (other.gameObject.GetComponent<Projectile>() != null)
        {
            
            
            //We hit another projectile so apply damage and destroy the weaker projectile
            Projectile bu = other.gameObject.GetComponent<Projectile>();

            //if the other projectile is already in our list, then we already handled collision with that object
            if (collidingProjectiles.Contains(bu))
            {
                collidingProjectiles.Remove(bu);
            }
            else
            {
                Debug.Log("Projectile Clash!");
                bu.collidingProjectiles.Add(this);//Add ourself to their list of projectiles so that we don't trigger these events twice

                bu.ApplyDamage(eh.action.damage);
                ApplyDamage(bu.eh.action.damage);

                bool destroySelf = false;
                bool destroyOther = false;

                if(this.remainingHealth <= 0 || this.remainingHealth <= bu.remainingHealth)
                {
                    destroySelf = true;
                }

                if (bu.remainingHealth <= 0 || bu.remainingHealth <= this.remainingHealth)
                {
                    destroyOther = true;
                }

                if (destroySelf)
                {
                    CreateOnHitEffect();
                }
                
                if(destroyOther)
                {
                    bu.CreateOnHitEffect();
                }
            }
        }
        else if (other.CompareTag("Beam"))
        {

        }
        else if (other.CompareTag("Boundary"))
        {
            //We hit a wall so destroy this projectile
            CreateOnHitEffect();
        }
    }

    public void ApplyDamage(float amount)
    {
        remainingHealth -= amount;
    }

    public void CreateOnHitEffect()
    {
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, this.transform.position, this.transform.rotation);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(this.transform.position); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        Destroy(gameObject);
    }
}