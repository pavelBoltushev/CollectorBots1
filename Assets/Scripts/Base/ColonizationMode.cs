using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonizationMode : BaseMode
{   
    protected override void Execute()
    {
        Bot botColonizer = Base.GetBotForColonization();
        Base.RemoveBot(botColonizer);
        StartCoroutine(botColonizer.Colonize(Base.Flag));
    }
}
