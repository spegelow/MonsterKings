using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public float speed = 15;
    ActionHandler eh;
    Vector3 direction;

    BeamEffect beam;
    public GameObject beamPrefab;
    public enum BeamState { NoHits, HitTarget, BeamClash};
    public BeamState beamState;

    public GameObject hitObject;
    public Vector3 hitOffset;

    public float damage;
    public float duration;
    public float timeElapsed;

    public bool inChargeOfClash; //Determines which of the two beams involved in the clash should manage the clash
    public float currentPoint;
    public float clashPoint; //0-1, 0 at this 1 at other
    public float clashDuration;
    public GameObject clashEffect;

    public GameObject clashPrefab;
    public GameObject clashExplosion;

    public void Initialize(ActionHandler eh)
    {
        this.eh = eh;

        //Spawn the beam effect object
        GameObject go = GameObject.Instantiate(beamPrefab, this.transform.position, this.transform.rotation);
        beam = go.GetComponent<BeamEffect>();
        beam.Initialize(this);

        //Calculate the move direction of this beam
        direction = (eh.tile.transform.position - eh.user.currentTile.transform.position).normalized;

        beamState = BeamState.NoHits;

        timeElapsed = 0;
        //Destroy(this.gameObject, 5);
    }

    void FixedUpdate()
    {
        //Check for any other objects before the end of beam
        //TODO
        //RaycastHit hit;
        //if (Physics.Linecast(transform.position, target.transform.position, out hit))
        //{
        //    if (hit.transform.tag == "player")
        //    {
        //        //do something
        //    }
        //}

        if (beamState == BeamState.BeamClash)
        {
            if (inChargeOfClash)
            {
                clashDuration += Time.deltaTime;

                clashEffect.transform.localScale = 2 * Vector3.one * clashDuration / 7;

                if (clashDuration >= 7)
                {
                    beam.DisablePrepare();
                    eh.FinishAction();
                    Destroy(this.gameObject);

                    BeamAttack other = hitObject.gameObject.GetComponent<BeamAttack>();
                    other.beam.DisablePrepare();
                    other.eh.FinishAction();
                    Destroy(other.gameObject);

                    GameObject go = GameObject.Instantiate(clashExplosion, this.transform.position, Quaternion.identity);
                    go.transform.localScale = Vector3.one * 3;
                }
            }
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= duration)
            {
                //We need to stop this beam
                beam.DisablePrepare();
                eh.FinishAction();
                Destroy(this.gameObject);
            }
        }



        if (beamState == BeamState.NoHits)
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
        else if (beamState == BeamState.HitTarget)
        {
            if (hitObject != null)
            {
                transform.position = hitObject.transform.position + hitOffset;

                //Apply damage to the object
                hitObject.gameObject.GetComponent<BattleUnit>()?.ApplyDamage(damage / duration * Time.deltaTime);
                Projectile p = hitObject.gameObject.GetComponent<Projectile>();
                p?.ApplyDamage(damage / duration * Time.deltaTime);
                if (p != null && p.remainingHealth <= 0)
                {
                    p.CreateOnHitEffect();
                    hitObject = null;
                    beamState = BeamState.NoHits;
                }
            }
        }
        else if (beamState == BeamState.BeamClash)
        {
            if(inChargeOfClash)
            {
                BeamAttack other = hitObject.gameObject.GetComponent<BeamAttack>();

                //Determine effective power of each beam
                float thisPower = this.damage;// * Random.Range(.5f,1.5f);
                float otherPower = other.damage;// * Random.Range(.5f, 1.5f);


                //Get the current point
                currentPoint = Vector3.Distance(beam.transform.position, this.transform.position) / Vector3.Distance(beam.transform.position, other.beam.transform.position);

                //Move the clash point toward the current point, if it gets there, send it further the other way
                if(clashPoint <= currentPoint)
                {
                    clashPoint += .1f * Time.deltaTime * thisPower / otherPower;
                    
                    if(clashPoint > currentPoint)
                    {
                        clashPoint += .5f;
                    }
                }
                else if (clashPoint > currentPoint)
                {
                    clashPoint -= .1f * Time.deltaTime * otherPower / thisPower;

                    if (clashPoint < currentPoint)
                    {
                        clashPoint -= .5f;
                    }
                }


                //Get the target point
                Vector3 target = Vector3.Lerp(this.beam.transform.position,other.beam.transform.position,clashPoint);

                

                float moveRate;
                //Determine the max beam move speed
                if (thisPower >= otherPower)
                {
                    moveRate = otherPower / thisPower / 2;

                    //Are we pushing forward
                    if(clashPoint > currentPoint)
                    {
                        moveRate = 1 - moveRate;
                    }
                    moveRate *= speed;
                }
                else
                {
                    moveRate = thisPower / otherPower / 2;

                    //Are we pushing forward
                    if (currentPoint > clashPoint)
                    {
                        moveRate = 1 - moveRate;
                    }
                    moveRate *= other.speed;
                }
                moveRate /= 2;

                //Move the 
                transform.position = Vector3.MoveTowards(transform.position, target, moveRate * Time.deltaTime);
                other.transform.position = other.hitObject.transform.position + other.hitOffset;

                
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BattleUnit>() != null)
        {
            BattleUnit bu = other.gameObject.GetComponent<BattleUnit>();
            //Ignore the user of the move
            if (bu.Equals(eh.user))
            {

            }
            else
            {
                if (beamState == BeamState.BeamClash)
                {
                    //Disable the other beam
                    BeamAttack ba = hitObject.gameObject.GetComponent<BeamAttack>();
                    ba.beam.DisablePrepare();
                    ba.eh.FinishAction();
                    Destroy(ba.gameObject);
                }

                hitObject = bu.gameObject;
                hitOffset = this.transform.position - other.transform.position;

                beamState = BeamState.HitTarget;
            }

        }
        else if (other.gameObject.GetComponent<Projectile>() != null)
        {
            Projectile bu = other.gameObject.GetComponent<Projectile>();

            hitObject = bu.gameObject;
            hitOffset = this.transform.position - other.transform.position;
            beamState = BeamState.HitTarget;
        }
        else if (other.gameObject.GetComponent<BeamAttack>() != null && beamState != BeamState.BeamClash)
        {
            BeamAttack bu = other.gameObject.GetComponent<BeamAttack>();

            inChargeOfClash = true;
            clashPoint = .5f;
            clashDuration = 0;

            hitObject = bu.gameObject;
            hitOffset = Vector3.zero;
            beamState = BeamState.BeamClash;

            bu.inChargeOfClash = false;
            bu.hitObject = this.gameObject;
            bu.hitOffset = Vector3.zero;
            bu.beamState = BeamState.BeamClash;

            clashEffect = GameObject.Instantiate(clashPrefab, this.transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //If we are no longer hitting our 'hit object' go back to the No Hits state
        if(beamState!=BeamState.BeamClash && other.gameObject.Equals(hitObject))
        {
            hitObject = null;
            beamState = BeamState.NoHits;
        }
    }
}