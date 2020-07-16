using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Elements
{
    public enum Element {Typeless, Heat, Water, Cold, Electric, Earth, Air, Life, Death, Prime};

    public const float ADVANTAGE_MULTIPLIER = 1.5f;
    public const float DISADVANTAGE_MULTIPLIER = 2/3f;
    

    public static float GetEffectiveness(Element a, Element d)
    {
        return 1;
    }
    public static float GetEffectiveness(Element a, Element d1, Element d2)
    {
        return GetEffectiveness(a,d1) * GetEffectiveness(a,d2);
    }
}
