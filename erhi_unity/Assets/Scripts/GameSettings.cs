using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class GameSettings : Singleton<GameSettings>
    {

        private int curTick = 0;
        private int curCircle = 0;
        private int circleLength = 12;
        private int circlesCount = 12;
        private bool vibrateOnTick = false;
        private bool soundOnTick = true;
        private bool soundOnCircle = true;
        private string soundOfTickName = "Bell ding";
        private string soundOfCircleName = "Bells up";
        
        public int currentTick
        {
            get { return curTick; }
            set { curTick = value; }
        }
        public int currentCircle
        {
            get { return curCircle; }
            set { curCircle = value; }
        }
        public int lengthOfCircle
        {
            get { return circleLength; }
            set { 
                if(circleLength != value)
                {
                    circleLength = value;
                    if (OnCircleLengthChanged != null)
                    {
                        OnCircleLengthChanged.Invoke();
                    }
                }                
            }
        }
        public int countOfCircles
        {
            get { return circlesCount; }
            set { circlesCount = value; }
        }
        public bool vibrateOnClick
        {
            get { return vibrateOnTick; }
            set { vibrateOnTick = value; }
        }
        public bool playSoundOnTick
        {
            get { return soundOnTick; }
            set { soundOnTick = value; }
        }
        public bool playSoundOnCircle
        {
            get { return soundOnCircle; }
            set { soundOnCircle = value; }
        }
        public string audioOfTickName
        {
            get { return soundOfTickName; }
            set { soundOfTickName = value; }
        }
        public string audioOfCircleName
        {
            get { return soundOfCircleName; }
            set { soundOfCircleName = value; }
        }
        public LanguageDictionary.Languages language
        {
            get { return GameManager.Instance.Language; }
            set { GameManager.Instance.Language = value; }
        }

        public Events.GameSettingChanged OnSettingsChanged = new Events.GameSettingChanged();
        public Events.CircleLengthChanged OnCircleLengthChanged = new Events.CircleLengthChanged();

        private class Settings
        {
            public int lengthOfCircle;
            public int countOfCircles;
            public bool vibrateOnClick;
            public bool playSoundOnTick;
            public bool playSoundOnCircle;
            public string audioOfTickName;
            public string audioOfCircleName;
            public string language;
        }

        public void SaveSettings()
        {
            Settings sets = new Settings();

            sets.lengthOfCircle = lengthOfCircle;
            sets.countOfCircles = countOfCircles;
            sets.vibrateOnClick = vibrateOnClick;
            sets.playSoundOnTick = playSoundOnTick;
            sets.playSoundOnCircle = playSoundOnCircle;
            sets.audioOfTickName = audioOfTickName;
            sets.audioOfCircleName = audioOfCircleName;
            sets.language = language.ToString();

            string settingString = JsonUtility.ToJson(sets);
            PlayerPrefs.SetString("GameSettings", settingString);
            PlayerPrefs.Save();
        }

        public void ReadSettings()
        {
            if (PlayerPrefs.HasKey("GameSettings"))
            {
                string settingsString = PlayerPrefs.GetString("GameSettings");
                Settings sets = new Settings();
                JsonUtility.FromJsonOverwrite(settingsString, sets);

                if (sets.lengthOfCircle > 0
                    && sets.countOfCircles > 0
                    && sets.audioOfTickName.Length > 0
                    && sets.audioOfCircleName.Length > 0
                    && (sets.language == LanguageDictionary.Languages.English.ToString()
                        || sets.language == LanguageDictionary.Languages.Buryat.ToString()
                        || sets.language == LanguageDictionary.Languages.Russian.ToString()))
                {
                    circleLength = sets.lengthOfCircle;
                    circlesCount = sets.countOfCircles;
                    vibrateOnTick = sets.vibrateOnClick;
                    soundOnTick = sets.playSoundOnTick;
                    soundOnCircle = sets.playSoundOnCircle;
                    soundOfTickName = sets.audioOfTickName;
                    soundOfCircleName = sets.audioOfCircleName;
                    if (sets.language.Equals(LanguageDictionary.Languages.English.ToString()))
                        language = LanguageDictionary.Languages.English;
                    else if(sets.language.Equals(LanguageDictionary.Languages.Buryat.ToString()))
                        language = LanguageDictionary.Languages.Buryat;
                    else if (sets.language.Equals(LanguageDictionary.Languages.Russian.ToString()))
                        language = LanguageDictionary.Languages.Russian;

                    if (OnSettingsChanged != null)
                    {
                        OnSettingsChanged.Invoke();
                    }
                    if(GameManager.Instance.OnLanguageChanged != null)
                    {
                        GameManager.Instance.OnLanguageChanged.Invoke();
                    }
                }
            }
        }

        private void Start()
        {
            
        }

        private void OnApplicationQuit()
        {
            SaveSettings();
        }

    }
}