using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{

    public int speed;

    public bool isTimerActive;
    public float maxTimerAmount;
    public float currentTimerAmount;

    public float maxHealth;
    public float currentHealth;


    public Slider healthBar;
    public Slider timerBar;

    public List<BattleAction> actions;

    public enum UnitState {Charging, UsingAction, Recovering }
    public UnitState unitState;

    public MapTile currentTile;
    public MapTile targetTile;

    public BattleAction currentAction;

    public ActionHandler actionHandler;

    public SpriteRenderer baseSprite;

    public MonsterData monsterData;

    // Start is called before the first frame update
    void Start()
    {
        unitState = UnitState.Recovering;
        ActivateTimer(maxTimerAmount);
        maxHealth = monsterData.GetStat(Stats.Stat.Health);
        currentHealth = maxHealth;
        healthBar.value = currentHealth / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BattleUpdate()
    {
        //Increment the timer if one is active and trigger the effects of the timer being filled
        if(isTimerActive && !BattleManager.IsBattlePaused())
        {
            float speedFactor = Stats.GetSpeedCommandGaugeMultiplier(monsterData.GetStat(Stats.Stat.Speed));


            currentTimerAmount += speedFactor * Time.deltaTime;
            UpdateTimer();

            if (currentTimerAmount >= maxTimerAmount)
            {
                //The timer is full so tell the battle manager something needs to happen
                if (unitState == UnitState.Recovering)
                {
                    //We were recovering so it is now this creatures turn
                    DeactivateTimer();
                    BattleManager.ActivateUnit(this);
                }
                else if (unitState == UnitState.Charging)
                {
                    //We were charging so perform whatever action we were charging
                    DeactivateTimer();
                    ActivateAction();
                }
            }
        }

        if(!BattleManager.IsBattlePaused() && unitState ==UnitState.UsingAction)
        {
            actionHandler.BattleUpdate();
        }
    }

    public void ActivateTimer(float timerMax)
    {
        isTimerActive = true;
        timerBar.gameObject.SetActive(true);

        this.maxTimerAmount = timerMax;
        currentTimerAmount = 0;
        UpdateTimer();
    }

    public void DeactivateTimer()
    {
        isTimerActive = false;
        timerBar.gameObject.SetActive(false);
    }

    private void UpdateTimer()
    {
        timerBar.value = currentTimerAmount / maxTimerAmount;
    }

    public void SetTile(MapTile tile)
    {
        this.currentTile = tile;

        this.transform.position = currentTile.transform.position;

        this.currentTile.occupant = this;
    }

    public void BeginAction(BattleAction selectedAction, MapTile targetTile)
    {
        currentAction = selectedAction;
        this.targetTile = targetTile;

        if(currentAction.chargeTime!=0)
        {
            //Begin charging action
            unitState = UnitState.Charging;
            ActivateTimer(currentAction.chargeTime);
        }
        else
        {
            ActivateAction();
        }
    }

    public void ActivateAction()
    {
        unitState = UnitState.UsingAction;
        actionHandler.ActivateAction(currentAction, targetTile);
    }

    public void FinishAction()
    {
        if (currentAction.recoveryTime != 0)
        {
            unitState = UnitState.Recovering;
            ActivateTimer(currentAction.recoveryTime);
        }
        else
        {
            unitState = UnitState.Recovering;
            BattleManager.ActivateUnit(this);
        }
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;

        healthBar.value = currentHealth / maxHealth;
    }
}
