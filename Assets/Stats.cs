using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats
{
    public enum Stat {Health, PhysicalAttack, PhysicalDefense, MagicalAttack, MagicalDefense, Speed};

    public static float GetSpeedCommandGaugeMultiplier(float speed)
    {
        //The average speed value, this value will return a multiplier of 1
        const float AVERAGE_SPEED = 70f;
        //How many points of speed stat is between each speed 'step'. Steps are similar to pokemon stat buff (... -> 1/3 -> 1/2 -> 1/1 -> 2/1 -> 3/1 -> ...) 
        const float SPEED_STEP_NUMBER = 50f;
        
        if (speed>=AVERAGE_SPEED)
        {
            return 1 + (speed - AVERAGE_SPEED) / SPEED_STEP_NUMBER;
        }
        else
        {
            return 1 / (1 + (AVERAGE_SPEED - speed) / SPEED_STEP_NUMBER);
        }
    }
}
