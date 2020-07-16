using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicAttack : MonoBehaviour
{
    public float timeElapsed;

    public float hitboxBeginTime;
    public float hitboxEndTime;

    public bool canHit;

    public float lifespan;

    public ActionHandler handler;
    public UnityEvent onAttackFinish;

    public void Initialize(ActionHandler eh)
    {
        handler = eh;
        canHit = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!BattleManager.IsBattlePaused())
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= lifespan)
            {
                onAttackFinish.Invoke();
                Destroy(this.gameObject);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }

    public void HandleCollision(Collider other)
    {
        if (canHit && timeElapsed >= hitboxBeginTime && timeElapsed < hitboxEndTime)
        {
            Damageable d = other.gameObject.GetComponent<Damageable>();
            if(d != null)
            {
                canHit = false;
                d.ApplyDamage(10);
            }
        }
    }
    
}
