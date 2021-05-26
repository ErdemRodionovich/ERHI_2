﻿using System.Collections;
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
                    if(OnCircleLengthChanged != null)
                    {
                        OnCircleLengthChanged.Invoke();
                    }
                }
                circleLength = value;
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

        public void SaveSettings()
        {
            string settingString = JsonUtility.ToJson(this);
            PlayerPrefs.SetString("Game settings", settingString);
            PlayerPrefs.Save();
        }

        public void ReadSettings()
        {
            if (PlayerPrefs.HasKey("Game settings"))
            {
                string settingsString = PlayerPrefs.GetString("Game settings");
                JsonUtility.FromJsonOverwrite(settingsString, this);

                if(OnSettingsChanged != null)
                {
                    OnSettingsChanged.Invoke();
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