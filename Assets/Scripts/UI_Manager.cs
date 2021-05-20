using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90 {

    public class UI_Manager : MonoBehaviour
    {
        private GameSettings settings;

        [SerializeField] private GameObject menu;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private TextMeshProUGUI cicleText;
        [SerializeField] private TMP_InputField circleLength;
        [SerializeField] private TMP_InputField numberOfCircles;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted.AddListener(HandleGameStart);
        }

        // Start is called before the first frame update
        void Start()
        {
            
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

    }
}