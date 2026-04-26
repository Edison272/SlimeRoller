using System;
using Unity.VisualScripting;

public class EventBus
{
    #region Singleton
    // Lazy, thread safe singleton
    private static readonly Lazy<EventBus> singleton = new Lazy<EventBus>(() => new EventBus());

    // Private constructor prevents external instantiation
    private EventBus() { }

    // Public static property to access the instance
    public static EventBus Singleton => singleton.Value;

    #endregion
    
    public Action PlayerDeath;
    public Action LevelComplete;
    public Action CoinCollect;
}