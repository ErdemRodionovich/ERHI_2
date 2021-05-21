using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90 {

    public class UI_Manager : MonoBehaviour
    {
        private GameSettings settings;
        private Dictionary<LanguageDictionary.Languages, TMP_Dropdown.OptionData> languageOptions = new Dictionary<LanguageDictionary.Languages, TMP_Dropdown.OptionData>();

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
            GameManager.Instance.OnGameStarted.AddListener(HandleGameStart);
        }

        // Start is called before the first frame update
        void Start()
        {
            InitLanguageDropdown();
            GameManager.Instance.OnLanguageChanged.AddListener(UpdateTexts);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            ReadSettings();
        }

        private void HandleGameStart()
        {
            ReadSettings();
        }

        private void ReadSettings()
        {
            settings = GameManager.Instance.settings;
            UpdateCircleLengthText();
            UpdateNumberOfCirclesText();

            if (settings.vibrateOnClick)
            {
                vibrateSlider.SetValueWithoutNotify(1.0f);
            }
            else
            {
                vibrateSlider.SetValueWithoutNotify(0.0f);
            }
        }

        private void UpdateCircleLengthText()
        {
            if(circleLength == null)
            {
                Debug.LogError("circleOfLength is NULL!");
            }

            if(settings == null)
            {
                Debug.LogError("settings is NULL!");
            }
            
            circleLength.SetTextWithoutNotify(settings.lengthOfCircle.ToString());
        }
        private void UpdateNumberOfCirclesText()
        {
            numberOfCircles.SetTextWithoutNotify(settings.countOfCircles.ToString());
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
            menu.SetActive(false);
        }

        public void OnCircleLengthMinusButtonClicked()
        {
            if (GameManager.Instance.settings.lengthOfCircle > 1)
            {
                GameManager.Instance.settings.lengthOfCircle--;
                UpdateCircleLengthText();
            }
        }
        public void OnCircleLengthPlusButtonClicked()
        {
            settings.lengthOfCircle++;
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
                settings.lengthOfCircle = editedCircleLength;
            }
        }
        
        public void OnNumberOfCirclesMinusButtonClicked()
        {
            if(settings.countOfCircles > 1)
            {
                settings.countOfCircles--;
            }
            UpdateNumberOfCirclesText();
        }

        public void OnNumberOfCirclesPlusButtonClicked()
        {
            settings.countOfCircles++;
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
                settings.countOfCircles = editedNumberOfCircles;
            }            
        }

        public void OnVibrateChanged()
        {
            if(vibrateSlider.value == 1.0f)
            {
                settings.vibrateOnClick = true;
            }
            else
            {
                settings.vibrateOnClick = false;
            }
        }

        private void ReadLanguage()
        {
            languageDropdown.SetValueWithoutNotify(
                languageDropdown.options.IndexOf(
                    languageOptions[GameManager.Instance.Language]
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

        }

        public void OnLanguageValueChanged()
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            circlelengthCaption.text = GameManager.Instance.Words("circle length");
            numberOfCriclesCaption.text = GameManager.Instance.Words("number of circles");
            vibrateOnClickCaption.text = GameManager.Instance.Words("vibrate on click");
            languageCaption.text = GameManager.Instance.Words("language");
        }

    }
}