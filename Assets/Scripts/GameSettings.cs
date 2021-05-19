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
        private string soundOfTickName;
        private string soundOfCircleName;

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
            set { circleLength = value; }
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
            }
        }
    }
}