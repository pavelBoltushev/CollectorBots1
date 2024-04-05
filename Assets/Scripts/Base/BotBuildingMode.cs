using UnityEngine;

public class BotBuildingMode : BaseMode
{    
    [SerializeField] private Bot _botTemplate;    

    protected override void Execute()
    {            
        Bot builtBot = Instantiate(_botTemplate, Base.transform.position, Base.transform.rotation);
        Base.AddBot(builtBot);
        builtBot.transform.position = builtBot.Slot.position;
        builtBot.transform.rotation = builtBot.Slot.rotation;        
    }
}
