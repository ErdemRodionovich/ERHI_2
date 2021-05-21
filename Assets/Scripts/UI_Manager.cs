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
        private bool vibrateOnTick = false;
        private bool soundOnTick = true;
        private bool soundOnCircle = true;
        private string soundOfTickName;
        private string soundOfCircleName;
        private LanguageDictionary.Languages language;

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



        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnLanguageChanged.AddListener(UpdateTexts);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            InitLanguageDropdown();
            ReadSettings();
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
                Debug.LogError("language not found! " + language+" count of options: "+languageOptions.Count);

                foreach(KeyValuePair<LanguageDictionary.Languages, TMP_Dropdown.OptionData>keyValue in languageOptions)
                {
                    Debug.Log("lang options: " + keyValue.Key + " --- " + keyValue.Value);
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

            Debug.Log("Language dropdown initialized!");

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
        }

        public void OnPlaySoundOnClickChanged()
        {
            //TODO
        }

    }
}