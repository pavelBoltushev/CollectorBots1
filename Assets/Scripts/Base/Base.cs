using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(BaseView), typeof(BotBuildingMode), typeof(ColonizationMode))]
public class Base : MonoBehaviour
{    
    [SerializeField] private Level _level;
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private float _minDistanceForFlag;
    [SerializeField] private List<Transform> _botSlots;    
    [SerializeField] private BaseInfoPanel _infoPanel;

    private List<Bot> _bots;
    private List<Mineral> _minerals;
    private bool[] _isSlotHasBot;
    private BotBuildingMode _botBuildingMode;
    private ColonizationMode _colonizationMode;
    private BaseView _baseView;

    public Flag Flag { get; private set; }   
    public Level Level => _level;
    public BaseInfoPanel InfoPanel => _infoPanel;

    public event Action<int> MineralCountChanged;
    public event Action<string> ModeChanged;

    private void Awake()
    {
        _bots = new List<Bot>();
        _minerals = new List<Mineral>();
        _isSlotHasBot = new bool[_botSlots.Count];
        _botBuildingMode = GetComponent<BotBuildingMode>();
        _colonizationMode = GetComponent<ColonizationMode>();
        _baseView = GetComponent<BaseView>();
    }

    private void Start()
    {        
        _botBuildingMode.enabled = true;
        _colonizationMode.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _minerals.Add(mineral);
            MineralCountChanged(_minerals.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _minerals.Remove(mineral);
            MineralCountChanged(_minerals.Count);
        }
    }
    
    public void Init(Level level, Bot builderBot, BaseInfoPanel infoPanel)
    {
        SetLevel(level);        
        AddBot(builderBot);
        _infoPanel = infoPanel;        
        _botBuildingMode.enabled = true;
        _colonizationMode.enabled = false;
    }

    public void SetLevel(Level level)
    {
        _level = level;
    }    

    public void TransferTarget(Mineral mineral)
    {
        if (_minerals.Contains(mineral) == false && IsAnyBotContains(mineral) == false)
        {
            _bots.OrderBy(bot => Vector3.Distance(bot.Slot.position, mineral.transform.position))
                    .First()
                    .AddTarget(mineral);
        }
    }   
    
    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
        bot.Init(this, GetFreeSlot());
        ClearAllBotsTargets();

        if(IsThereFreeSlot() == false)
        {
            _botBuildingMode.enabled = false;
            ModeChanged(GetModeTypeText());
        }
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
        _isSlotHasBot[_botSlots.IndexOf(bot.Slot)] = false;
    }

    public void SpendMinerals(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Mineral mineral = _minerals[0];
            _minerals.Remove(mineral);
            _level.RemoveMineral(mineral);
            Destroy(mineral.gameObject);
        }              
    }

    public Bot GetBotForColonization()
    {               
        foreach (var bot in _bots)
        {
            if (bot.IsFree)
            {
                _botBuildingMode.enabled = true;
                _colonizationMode.enabled = false;
                ModeChanged(GetModeTypeText());
                return bot;
            }
        }

        return null;
    }

    public void OnClicked()
    {
        _baseView.Mark();
        _infoPanel.On();
        _infoPanel.SetMineralCount(_minerals.Count);
        _infoPanel.SetModeTypeText(GetModeTypeText());
        MineralCountChanged += _infoPanel.SetMineralCount;
        ModeChanged += _infoPanel.SetModeTypeText;
    }

    public void OnUnclicked()
    {
        _baseView.Unmark();
        _infoPanel.Off();
        MineralCountChanged -= _infoPanel.SetMineralCount;
        ModeChanged -= _infoPanel.SetModeTypeText;
    }

    public void StandFlag(Vector3 position)
    {
        if(Vector3.Distance(transform.position, position) > _minDistanceForFlag)
        {
            _botBuildingMode.enabled = false;
            _colonizationMode.enabled = true;
            ModeChanged(GetModeTypeText());

            if (Flag != null)
                Destroy(Flag.gameObject);

            Flag = Instantiate(_flagTemplate, position, Quaternion.identity);             
        }
    }      

    public void ClearAllBotsTargets()
    {
        foreach (var bot in _bots)
        {
            bot.ClearTargets();
        }
    }
    
    private bool IsThereFreeSlot()
    {       
        for (int i = 0; i < _isSlotHasBot.Length; i++)
        {
            if (_isSlotHasBot[i] == false)
            {
                return true;                
            }
        }

        return false;
    }

    private Transform GetFreeSlot()
    {               
        for (int i = 0; i < _isSlotHasBot.Length; i++)
        {
            if (_isSlotHasBot[i] == false)
            {                
                _isSlotHasBot[i] = true;
                return _botSlots[i];                
            }
        }

        return null;
    }    

    private bool IsAnyBotContains(Mineral target)
    {
        foreach (var bot in _bots)
        {
            if (bot.ContainsTarget(target))
                return true;
        }

        return false;
    }

    private string GetModeTypeText()
    {
        if (_botBuildingMode.enabled == true)
            return "Строительство";
        else if (_colonizationMode.enabled == true)
            return "Колонизация";
        else
            return "Накопление";
    }
}
