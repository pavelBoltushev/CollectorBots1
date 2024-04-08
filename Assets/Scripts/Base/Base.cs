using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(BaseController), typeof(BaseView))]
[RequireComponent(typeof(BotBuildingMode), typeof(ColonizationMode))]
public class Base : MonoBehaviour
{    
    [SerializeField] private Level _level;    
    [SerializeField] private List<Transform> _botSlots;    

    private List<Bot> _bots;
    private List<Mineral> _minerals;
    private bool[] _isSlotHasBot;
    private List<BaseMode> _availableModes;
    private BaseView _view;
    private BaseController _controller;

    public Flag Flag { get; set; }   
    public Level Level => _level;
    public int MineralsCount => _minerals.Count;
    public BaseInfoPanel InfoPanel => _view.InfoPanel;
    public Mode CurrentMode { get; private set; }
    
    public event Action<int> MineralCountChanged;
    public event Action<string> ModeChanged;

    private void Awake()
    {
        _bots = new List<Bot>();
        _minerals = new List<Mineral>();
        _isSlotHasBot = new bool[_botSlots.Count];
        _availableModes = new List<BaseMode>();
        _availableModes.Add(GetComponent<BotBuildingMode>());
        _availableModes.Add(GetComponent<ColonizationMode>());
        _view = GetComponent<BaseView>();
        _controller = GetComponent<BaseController>();
    }

    private void Start()
    {
        Set(Mode.BotBuilding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _minerals.Add(mineral);
            MineralCountChanged?.Invoke(_minerals.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _minerals.Remove(mineral);
            MineralCountChanged?.Invoke(_minerals.Count);
        }
    }
    
    public void Init(Level level, Bot builderBot, BaseInfoPanel infoPanel)
    {
        SetLevel(level);        
        AddBot(builderBot);
        _view.SetInfoPanel(infoPanel);
        Set(Mode.BotBuilding);
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
            Set(Mode.Standby);
            ModeChanged?.Invoke(GetModeTypeText());
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
                Set(Mode.BotBuilding);
                ModeChanged(GetModeTypeText());
                return bot;
            }
        }

        return null;
    }    

    public void ClearAllBotsTargets()
    {
        foreach (var bot in _bots)
        {
            bot.ClearTargets();
        }
    }

    public void Set(Mode mode)
    {
        CurrentMode = mode;
        _view.InfoPanel.SetModeTypeText(GetModeTypeText());

        switch (mode)
        {
            case Mode.BotBuilding:

                foreach (var item in _availableModes)
                {
                    if (item is BotBuildingMode)
                        item.enabled = true;
                    else
                        item.enabled = false;
                }
                
                break;

            case Mode.Colonization:

                foreach (var item in _availableModes)
                {
                    if (item is ColonizationMode)
                        item.enabled = true;
                    else
                        item.enabled = false;
                }
                
                break;

            case Mode.Standby:

                foreach (var item in _availableModes)
                {
                    item.enabled = false;
                }
                
                break;
        }   
    }

    public string GetModeTypeText()
    {
        switch (CurrentMode)
        {
            case Mode.BotBuilding:
                return "Строительство";
                
            case Mode.Colonization:
                return "Колонизация";
                
            case Mode.Standby:
                return "Накопление";                
        }

        return "";
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
}

public enum Mode
{
    BotBuilding,
    Colonization,
    Standby
}
