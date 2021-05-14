using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class LanguageDictionary
    {
        public enum Languages
        {
            English,
            Russian,
            Buryat
        };

        private Dictionary<Languages, Dictionary<string, string>> dictionary;

        public LanguageDictionary()
        {

            dictionary = new Dictionary<Languages, Dictionary<string, string>>();
            InitEnglish();
            InitRussian();
            InitBuryat();
        }

        private void InitEnglish()
        {
            Dictionary<string, string> engDictionary = new Dictionary<string, string>();
            engDictionary.Add("OK", "OK");
            engDictionary.Add("cancel", "Cancel");
            engDictionary.Add("font", "Font");
            engDictionary.Add("font size", "Font size");
            engDictionary.Add("audio", "Audio");
            engDictionary.Add("language", "Language");
            engDictionary.Add("sample text", "Sample text");
            engDictionary.Add("volume", "Volume");
            engDictionary.Add("sample text for font", "Almost before we knew it, we had left the ground.");

            if (dictionary.ContainsKey(Languages.English))
            {
                dictionary.Remove(Languages.English);
            }

            dictionary.Add(Languages.English, engDictionary);
        }

        private void InitRussian()
        {
            Dictionary<string, string> rusDictionary = new Dictionary<string, string>();
            rusDictionary.Add("OK", "Принять");
            rusDictionary.Add("cancel", "Отмена");
            rusDictionary.Add("font", "Шрифт");
            rusDictionary.Add("font size", "Размер шрифта");
            rusDictionary.Add("audio", "Аудио");
            rusDictionary.Add("language", "Язык");
            rusDictionary.Add("sample text", "Пример текста");
            rusDictionary.Add("volume", "Громкость");
            rusDictionary.Add("sample text for font", "Шифровальщица попросту забыла ряд ключевых множителей и тэгов.");

            if (dictionary.ContainsKey(Languages.Russian))
            {
                dictionary.Remove(Languages.Russian);
            }

            dictionary.Add(Languages.Russian, rusDictionary);
        }

        private void InitBuryat()
        {
            Dictionary<string, string> burDictionary = new Dictionary<string, string>();
            burDictionary.Add("OK", "Заа");
            burDictionary.Add("cancel", "Боли");
            burDictionary.Add("font", "Үзэг");
            burDictionary.Add("font size", "Үзэгэй хэмжээ");
            burDictionary.Add("audio", "Абяан");
            burDictionary.Add("language", "Хэлэн");
            burDictionary.Add("sample text", "Бэшэгээ жэшээ");
            burDictionary.Add("volume", "Абяани шанга");
            string burPana = "Yндэр уулын сэсэг шэнги дэлгэржэ, " +
                "Yри хүүгэдѳѳ үргэжэ, " +
                "Yндэр наhатай боложо, " +
                "Сэдьхэн дуулажа жаргагты!";

            burDictionary.Add("sample text for font", burPana);

            if (dictionary.ContainsKey(Languages.Buryat))
            {
                dictionary.Remove(Languages.Buryat);
            }

            dictionary.Add(Languages.Buryat, burDictionary);
        }

        public string Words(Languages language, string keyWord)
        {
            if (dictionary.ContainsKey(language))
            {
                if (dictionary[language].ContainsKey(keyWord))
                {
                    return dictionary[language][keyWord];
                }
                else
                {
                    Debug.LogError("[LanguageDictionary] Cannot find word '" + keyWord + "' in language '" + language + "'!");
                }
            }
            else
            {
                Debug.LogError("[LanguageDictionary] Unsupported language '" + language + "'!");
            }

            return keyWord;
        }

    }
}