using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90 {

    public class UI_Manager : MonoBehaviour
    {
        private Dictionary<LanguageDictionary.Languages, TMP_Dropdown.OptionData> languageOptions = new Dictionary<LanguageDictionary.Languages, TMP_Dropdown.OptionData>();
        private int circleLengthValue;
        private int circlesCount;
        private bool vibrateOnTick;
        private bool soundOnTick;
        private bool soundOnCircle;
        private string soundOfTickName;
        private string soundOfCircleName;
        private LanguageDictionary.Languages language;
        private bool initialized = false;
        private Dictionary<string, TMP_Dropdown.OptionData> onClickAudioList = new Dictionary<string, TMP_Dropdown.OptionData>();
        private Dictionary<string, TMP_Dropdown.OptionData> onCircleAudioList = new Dictionary<string, TMP_Dropdown.OptionData>();

        [SerializeField] private GameObject menu;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private TextMeshProUGUI cicleText;
        [SerializeField] private TextMeshProUGUI circlelengthCaption;
        [SerializeField] private TMP_InputField circleLength;
        [SerializeField] private TextMeshProUGUI numberOfCriclesCaption;
        [SerializeField] private TMP_InputField numberOfCircles;
        [SerializeField] private TextMeshProUGUI vibrateOnClickCaption;
        [SerializeField] private Slider vibrateSlider;
        [SerializeField] private TextMeshProUGUI languageCaption;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private Slider playSoundOnTickSlider;
        [SerializeField] private TextMeshProUGUI playSoundOnClickCaption;
        [SerializeField] private TMP_Dropdown soundOnClickDropdown;
        [SerializeField] private TextMeshProUGUI soundOnClickCaption;
        [SerializeField] private Slider playSoundOnCircleSlider;
        [SerializeField] private TextMeshProUGUI playSoundOnCircleCaption;
        [SerializeField] private TMP_Dropdown soundOnCircleDropdown;
        [SerializeField] private TextMeshProUGUI soundOnCircleCaption;
        [SerializeField] private TextMeshProUGUI soundOnCircleLabel;
        [SerializeField] private TextMeshProUGUI soundOnCircleItemLabel;
        [SerializeField] private TextMeshProUGUI aboutProgramCaption;
        [SerializeField] private GameObject aboutGroup;

        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnLanguageChanged.AddListener(UpdateTexts);
            GameManager.Instance.OnGameStarted.AddListener(HandleGameStart);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            if (!initialized)
            {
                InitMenu();
            }
            ReadSettings();
        }

        private void InitMenu()
        {
            InitLanguageDropdown();
            InitSoundOnClickDropdown();
            InitSoundOnCircleDropdown();
        }

        private void ReadSettings()
        {
            circleLengthValue = GameManager.Instance.settings.lengthOfCircle;
            circlesCount = GameManager.Instance.settings.countOfCircles;
            vibrateOnTick = GameManager.Instance.settings.vibrateOnClick;
            soundOnTick = GameManager.Instance.settings.playSoundOnTick;
            soundOnCircle = GameManager.Instance.settings.playSoundOnCircle;
            soundOfTickName = GameManager.Instance.settings.audioOfTickName;
            soundOfCircleName = GameManager.Instance.settings.audioOfCircleName;

            ReadLanguage();

            UpdateCircleLengthText();
            UpdateNumberOfCirclesText();

            if (vibrateOnTick)
            {
                vibrateSlider.SetValueWithoutNotify(1.0f);
            }
            else
            {
                vibrateSlider.SetValueWithoutNotify(0.0f);
            }

            if (soundOnTick)
            {
                playSoundOnTickSlider.SetValueWithoutNotify(1.0f);
            }
            else
            {
                playSoundOnTickSlider.SetValueWithoutNotify(0.0f);
            }

            ReadOnClickAudio();

            if (soundOnCircle)
            {
                playSoundOnCircleSlider.SetValueWithoutNotify(1.0f);
            }
            else
            {
                playSoundOnCircleSlider.SetValueWithoutNotify(0.0f);
            }

            ReadSoundCircleAudio();

        }

        private void SaveSettings()
        {
            if(circleLengthValue != GameManager.Instance.settings.lengthOfCircle)
            {
                GameManager.Instance.settings.lengthOfCircle = circleLengthValue;
            }
            if (circlesCount != GameManager.Instance.settings.countOfCircles)
            {
                GameManager.Instance.settings.countOfCircles = circlesCount;
            }
            if(vibrateOnTick != GameManager.Instance.settings.vibrateOnClick)
            {
                GameManager.Instance.settings.vibrateOnClick = vibrateOnTick;
            }
            if(soundOnTick != GameManager.Instance.settings.playSoundOnTick)
            {
                GameManager.Instance.settings.playSoundOnTick = soundOnTick;
            }
            if(soundOnCircle != GameManager.Instance.settings.playSoundOnCircle)
            {
                GameManager.Instance.settings.playSoundOnCircle = soundOnCircle;
            }
            if(soundOfTickName != GameManager.Instance.settings.audioOfTickName)
            {
                GameManager.Instance.settings.audioOfTickName = soundOfTickName;
            }
            if(soundOfCircleName != GameManager.Instance.settings.audioOfCircleName)
            {
                GameManager.Instance.settings.audioOfCircleName = soundOfCircleName;
            }

            GameManager.Instance.settings.SaveSettings();

        }

        private void UpdateCircleLengthText()
        {
            circleLength.SetTextWithoutNotify(circleLengthValue.ToString());
        }
        private void UpdateNumberOfCirclesText()
        {
            numberOfCircles.SetTextWithoutNotify(circlesCount.ToString());
        }

        public void OnMenuButtonClicked()
        {
            menu.SetActive(true);
        }

        public void OnCancelButtonClicked()
        {

        }

        public void OnBackButtonClicked()
        {
            SaveSettings();
            menu.SetActive(false);
        }

        public void OnCircleLengthMinusButtonClicked()
        {
            if (circleLengthValue > 1)
            {
                circleLengthValue--;
                UpdateCircleLengthText();
            }
        }
        public void OnCircleLengthPlusButtonClicked()
        {
            circleLengthValue++;
            UpdateCircleLengthText();
        }

        public void OnCircleLengthValueChanged()
        {
            int editedCircleLength = int.Parse(circleLength.text);
            if(editedCircleLength < 1)
            {
                UpdateCircleLengthText();
            }
            else
            {
                circleLengthValue = editedCircleLength;
            }
        }
        
        public void OnNumberOfCirclesMinusButtonClicked()
        {
            if(circlesCount > 1)
            {
                circlesCount--;
            }
            UpdateNumberOfCirclesText();
        }

        public void OnNumberOfCirclesPlusButtonClicked()
        {
            circlesCount++;
            UpdateNumberOfCirclesText();
        }
        
        public void OnNumberOfCirclesValueChanged()
        {
            int editedNumberOfCircles = int.Parse(numberOfCircles.text);
            if(editedNumberOfCircles < 1)
            {
                UpdateNumberOfCirclesText();
            }
            else
            {
                circlesCount = editedNumberOfCircles;
            }            
        }

        public void OnVibrateChanged()
        {
            if(vibrateSlider.value == 1.0f)
            {
                vibrateOnTick = true;
            }
            else
            {
                vibrateOnTick = false;
            }
        }

        private void ReadLanguage()
        {
            language = GameManager.Instance.Language;

            if (!languageOptions.ContainsKey(language))
            {
                Debug.LogError("[UI_Manager] Language not found! " + language+" count of options: "+languageOptions.Count);

                foreach(KeyValuePair<LanguageDictionary.Languages, TMP_Dropdown.OptionData>keyValue in languageOptions)
                {
                    Debug.Log("[UI_Manager] Lang options: " + keyValue.Key + " --- " + keyValue.Value);
                }

            }

            languageDropdown.SetValueWithoutNotify(
                languageDropdown.options.IndexOf(
                    languageOptions[language]
                )
            );
        }

        private void InitLanguageDropdown()
        {
            languageOptions.Clear();
            languageDropdown.options.Clear();
            
            TMP_Dropdown.OptionData engLanguageOption = new TMP_Dropdown.OptionData();
            engLanguageOption.text = "English";
            languageDropdown.options.Add(engLanguageOption);
            languageOptions.Add(LanguageDictionary.Languages.English, engLanguageOption);

            TMP_Dropdown.OptionData burLanguageOption = new TMP_Dropdown.OptionData();
            burLanguageOption.text = "Буряад";
            languageDropdown.options.Add(burLanguageOption);
            languageOptions.Add(LanguageDictionary.Languages.Buryat, burLanguageOption);

            TMP_Dropdown.OptionData rusLanguageOption = new TMP_Dropdown.OptionData();
            rusLanguageOption.text = "Русский";
            languageDropdown.options.Add(rusLanguageOption);
            languageOptions.Add(LanguageDictionary.Languages.Russian, rusLanguageOption);

        }

        public void OnLanguageValueChanged()
        {
            foreach(KeyValuePair<LanguageDictionary.Languages, TMP_Dropdown.OptionData>keyValue in languageOptions)
            {
                if(languageDropdown.options[languageDropdown.value] == keyValue.Value)
                {
                    language = keyValue.Key;
                    break;
                }
            }
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            circlelengthCaption.text = GameManager.Instance.WordOnLanguage(language, "circle length");
            numberOfCriclesCaption.text = GameManager.Instance.WordOnLanguage(language, "number of circles");
            vibrateOnClickCaption.text = GameManager.Instance.WordOnLanguage(language, "vibrate on click");
            languageCaption.text = GameManager.Instance.WordOnLanguage(language, "language");
            playSoundOnClickCaption.text = GameManager.Instance.WordOnLanguage(language, "play sound on click");
            soundOnClickCaption.text = GameManager.Instance.WordOnLanguage(language, "sound on click");
            soundOnCircleCaption.text = GameManager.Instance.WordOnLanguage(language, "sound on circle");
            playSoundOnCircleCaption.text = GameManager.Instance.WordOnLanguage(language, "play sound on circle");

            foreach(KeyValuePair<string, TMP_Dropdown.OptionData>keyValue in onClickAudioList)
            {
                keyValue.Value.text = GameManager.Instance.WordOnLanguage(language, keyValue.Key);
            }
            soundOnClickDropdown.RefreshShownValue();
            
            foreach(KeyValuePair<string, TMP_Dropdown.OptionData>keyValue in onCircleAudioList)
            {
                keyValue.Value.text = GameManager.Instance.WordOnLanguage(language, keyValue.Key);
            }
            soundOnCircleDropdown.RefreshShownValue();

            switch (language)
            {
                case LanguageDictionary.Languages.English:
                    soundOnCircleItemLabel.fontSize = 22.0f;
                    soundOnCircleLabel.fontSize = 22.0f;
                    break;

                case LanguageDictionary.Languages.Buryat:
                    soundOnCircleItemLabel.fontSize = 18.0f;
                    soundOnCircleLabel.fontSize = 18.0f;
                    break;

                case LanguageDictionary.Languages.Russian:
                    soundOnCircleItemLabel.fontSize = 16.0f;
                    soundOnCircleLabel.fontSize = 16.0f;
                    break;

                default:
                    break;
            }

            aboutProgramCaption.text = GameManager.Instance.WordOnLanguage(language, "about profram");

        }

        public void OnPlaySoundOnClickChanged()
        {
            if(playSoundOnTickSlider.value == 1.0f)
            {
                soundOnTick = true;
            }
            else
            {
                soundOnTick = false;
            }
        }

        private void HandleGameStart()
        {
            InitMenu();
        }

        private void InitSoundOnClickDropdown()
        {
            onClickAudioList.Clear();
            soundOnClickDropdown.options.Clear();

            if (GameManager.Instance.OnClickPlayList != null)
            {
                for (int i = 0; i < GameManager.Instance.OnClickPlayList.Count; i++)
                {
                    TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
                    string clipName = GameManager.Instance.OnClickPlayList.GetNameOfAudio(i);
                    opt.text = GameManager.Instance.WordOnLanguage(language, clipName);
                    soundOnClickDropdown.options.Add(opt);
                    onClickAudioList.Add(clipName, opt);
                }
            }

        }

        private void ReadOnClickAudio()
        {
            soundOfTickName = GameManager.Instance.settings.audioOfTickName;

            if(soundOfTickName.Length != 0 && onClickAudioList.ContainsKey(soundOfTickName))
            {
                soundOnClickDropdown.SetValueWithoutNotify(
                    soundOnClickDropdown.options.IndexOf(
                        onClickAudioList[soundOfTickName]
                    )
                );
            }
            else
            {
                foreach(KeyValuePair<string, TMP_Dropdown.OptionData> keyValue in onClickAudioList)
                {
                    soundOfTickName = keyValue.Key;
                    soundOnClickDropdown.SetValueWithoutNotify(soundOnClickDropdown.options.IndexOf(keyValue.Value));
                    break;
                }
            }

        }

        public void OnSoundOnClickValueChanged()
        {
            TMP_Dropdown.OptionData curOpt = soundOnClickDropdown.options[soundOnClickDropdown.value];
            
            bool findClip = false;
            foreach(KeyValuePair<string, TMP_Dropdown.OptionData>keyValue in onClickAudioList)
            {
                if(keyValue.Value == curOpt)
                {
                    findClip = true;
                    AudioClip curAudio = GameManager.Instance.OnClickPlayList.GetAudioByName(keyValue.Key);
                    GameManager.Instance.PlayAudio(curAudio);
                    soundOfTickName = keyValue.Key;
                    break;
                }
            }

            if (!findClip)
            {
                Debug.LogError("[UI_Manager] Cannot find audio in play on click play list! Option text '"+curOpt.text+"'");
            }
        }

        public void OnPlaySoundOnCircleValueChanged()
        {
            if(playSoundOnCircleSlider.value == 1.0f)
            {
                soundOnCircle = true;
            }
            else
            {
                soundOnCircle = false;
            }
        }

        private void InitSoundOnCircleDropdown()
        {
            onCircleAudioList.Clear();
            soundOnCircleDropdown.options.Clear();

            if(GameManager.Instance.OnCirclePlayList != null)
            {
                for(int i=0; i < GameManager.Instance.OnCirclePlayList.Count; i++)
                {
                    TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
                    string clipName = GameManager.Instance.OnCirclePlayList.GetNameOfAudio(i);
                    opt.text = GameManager.Instance.WordOnLanguage(language, clipName);
                    soundOnCircleDropdown.options.Add(opt);
                    onCircleAudioList.Add(clipName, opt);
                }
            }
        }

        private void ReadSoundCircleAudio()
        {
            soundOfCircleName = GameManager.Instance.settings.audioOfCircleName;

            if(soundOfCircleName.Length > 0 && onCircleAudioList.ContainsKey(soundOfCircleName))
            {
                soundOnCircleDropdown.SetValueWithoutNotify(
                    soundOnCircleDropdown.options.IndexOf(
                        onCircleAudioList[soundOfCircleName]
                    )
                );
            }
            else
            {
                foreach(KeyValuePair<string, TMP_Dropdown.OptionData>keyValue in onCircleAudioList)
                {
                    soundOfCircleName = keyValue.Key;
                    soundOnCircleDropdown.SetValueWithoutNotify(soundOnCircleDropdown.options.IndexOf(keyValue.Value));
                    break;
                }
            }

        }

        public void OnSoundOnCircleValueChanged()
        {
            TMP_Dropdown.OptionData curOpt = soundOnCircleDropdown.options[soundOnCircleDropdown.value];

            bool findClip = false;
            foreach(KeyValuePair<string, TMP_Dropdown.OptionData>keyValue in onCircleAudioList)
            {
                if(keyValue.Value == curOpt)
                {
                    findClip = true;
                    AudioClip curClip = GameManager.Instance.OnCirclePlayList.GetAudioByName(keyValue.Key);
                    GameManager.Instance.PlayAudio(curClip);
                    soundOfCircleName = keyValue.Key;
                    break;
                }
            }

            if (!findClip)
            {
                Debug.LogError("[UI_Manager] Cannot find audio clip in play on circle play list! Option text '"+curOpt.text+"'");
            }

        }

        public void OnAboutProgramButtonClicked()
        {
            aboutGroup.SetActive(true);
        }

        public void OnBackFromAboutButtonClicked()
        {
            aboutGroup.SetActive(false);
        }

    }
}