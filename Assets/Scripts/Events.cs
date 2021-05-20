using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class Events
    {
        [System.Serializable] public class GameStateChangedEvent : UnityEvent<GameManager.GameStates, GameManager.GameStates> { }
        [System.Serializable] public class LanguageChanged : UnityEvent { }
        [System.Serializable] public class Tick: UnityEvent { }
        [System.Serializable] public class GameStarted : UnityEvent { }
    }
}