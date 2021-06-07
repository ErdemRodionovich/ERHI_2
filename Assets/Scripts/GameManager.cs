using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{

    public class GameManager : Singleton<GameManager>
    {

        private AudioClip soundOfTick;
        private AudioClip soundOfCircle;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> playListOnClick;
        public ProtectedPlayList OnClickPlayList;
        [SerializeField] private List<AudioClip> playListOnCircle;
        public ProtectedPlayList OnCirclePlayList;

        public Events.Tick OnTick = new Events.Tick();
        public Events.GameStarted OnGameStarted = new Events.GameStarted();
        public Events.ClickForTick OnClickForTick = new Events.ClickForTick();
        public Events.GameRestarted OnGameRestarted = new Events.GameRestarted();

        public GameSettings settings {
            get { return GameSettings.Instance; }
        }

        public AudioClip audioOfTick
        {
            get { return soundOfTick; }
            set { soundOfTick = value; }
        }
        public AudioClip audioOfCircle
        {
            get { return soundOfCircle; }
            set { soundOfCircle = value; }
        }


        #region States
        public enum GameStates
        {
            PREGAME,
            RUNNING,
            PAUSED
        }

        private GameStates currentState = GameStates.PREGAME;
        public GameStates State
        {
            get { return currentState; }
        }
        public Events.GameStateChangedEvent OnStateChanged = new Events.GameStateChangedEvent();

        private void UpdateState(GameStates newState)
        {
            GameStates prevState = currentState;
            currentState = newState;
            switch (currentState)
            {
                case GameStates.PREGAME:

                    break;

                case GameStates.RUNNING:

                    break;

                case GameStates.PAUSED:

                    break;

                default:
                    break;
            }

            OnStateChanged.Invoke(currentState, prevState);

        }
        #endregion

        #region SceneLoading utils

        private Dictionary<AsyncOperation, string> loadOperations = new Dictionary<AsyncOperation, string>();
        public void LoadScene(string name)
        {
            AsyncOperation asyncOper = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            if (asyncOper == null)
            {
                Debug.LogError("[GameManager] Cannot load scene: " + name);
                return;
            }
            asyncOper.completed += OnLoadOperationComplete;
            loadOperations.Add(asyncOper, name);
        }

        private void OnLoadOperationComplete(AsyncOperation obj)
        {
            if (loadOperations.ContainsKey(obj))
            {
                OnSceneLoaded(loadOperations[obj]);
                loadOperations.Remove(obj);
            }
        }

        private void OnSceneLoaded(string name)
        {
            if (name == "TreeScene")
            {
                UpdateState(GameStates.RUNNING);
            }

        }

        public void UnLoadScene(string name)
        {
            AsyncOperation asyncOper = SceneManager.UnloadSceneAsync(name);
            if (asyncOper == null)
            {
                Debug.LogError("[GameManager] Cannot unload scene: " + name);
                return;
            }
            asyncOper.completed += OnUnLoadOperationComplete;
            loadOperations.Add(asyncOper, name);

        }

        private void OnUnLoadOperationComplete(AsyncOperation obj)
        {
            if (loadOperations.ContainsKey(obj))
            {
                OnSceneUnloaded(loadOperations[obj]);
                loadOperations.Remove(obj);
            }
        }

        private void OnSceneUnloaded(string name)
        {

        }

        #endregion

        #region Language

        private LanguageDictionary languageTranslater = new LanguageDictionary();
        private LanguageDictionary.Languages currentLanguage = LanguageDictionary.Languages.English;
        public LanguageDictionary.Languages Language
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value;
                if (OnLanguageChanged != null)
                {
                    OnLanguageChanged.Invoke();
                }
            }
        }

        public Events.LanguageChanged OnLanguageChanged = new Events.LanguageChanged();

        public string Words(string keyWord)
        {
            return languageTranslater.Words(currentLanguage, keyWord);
        }

        public string WordOnLanguage(LanguageDictionary.Languages lang, string keyWord)
        {
            return languageTranslater.Words(lang, keyWord);
        }

        #endregion

        #region UnityCore funstions

        // Start is called before the first frame update
        void Start()
        {
            InitOnClickPlayList();
            InitOnCirclePlayList();
            settings.OnSettingsChanged.AddListener(HandleSettingsChange);
            settings.ReadSettings();

            if(OnGameStarted != null)
            {
                OnGameStarted.Invoke();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        public void StartGame()
        {
            
        }

        public void OnApplicationQuit()
        {
            
        }

        public void TogglePause()
        {
            UpdateState(currentState == GameStates.PAUSED ? GameStates.RUNNING : GameStates.PAUSED);
        }

        public void ResetGame()
        {
            settings.currentTick = 0;
            settings.currentCircle = 0;
            if(OnGameRestarted != null)
            {
                OnGameRestarted.Invoke();
            }
            if(OnTick != null)
            {
                OnTick.Invoke();
            }
        }

        public void Tick()
        {
            settings.currentTick++;
            if(settings.currentTick >= settings.lengthOfCircle)
            {
                settings.currentCircle++;
                settings.currentTick = 0;

                if (settings.playSoundOnCircle)
                {
                    audioSource.PlayOneShot(soundOfCircle);
                }

            }

            if (settings.vibrateOnClick)
            {
                Handheld.Vibrate();
            }

            if (settings.playSoundOnTick)
            {
                audioSource.PlayOneShot(soundOfTick);
            }

            if(OnTick != null)
            {
                OnTick.Invoke();
            }

        }

        public void CancelTick()
        {
            if(settings.currentTick > 0)
            {
                settings.currentTick--;
                if (settings.playSoundOnTick)
                {
                    audioSource.PlayOneShot(soundOfTick);
                }
            }
            else
            {
                if(settings.currentCircle > 0 && settings.lengthOfCircle > 0)
                {
                    settings.currentCircle--;
                    settings.currentTick = settings.lengthOfCircle - 1;
                    if (settings.playSoundOnCircle)
                    {
                        audioSource.PlayOneShot(soundOfCircle);
                    }
                }
            }

            if(OnTick != null)
            {
                OnTick.Invoke();
            }

        }

        private void InitOnClickPlayList()
        {
            OnClickPlayList = new ProtectedPlayList(playListOnClick);
        }

        private void InitOnCirclePlayList()
        {
            OnCirclePlayList = new ProtectedPlayList(playListOnCircle);
        }

        public void PlayAudio(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        private void HandleSettingsChange()
        {
            soundOfTick = OnClickPlayList.GetAudioByName(settings.audioOfTickName);
            soundOfCircle = OnCirclePlayList.GetAudioByName(settings.audioOfCircleName);
        }

    }
}