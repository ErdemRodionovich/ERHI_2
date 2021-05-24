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
            engDictionary.Add("circle length", "Circle length");
            engDictionary.Add("number of circles", "Number of circles");
            engDictionary.Add("vibrate on click", "Vibrate on click");
            engDictionary.Add("play sound on click", "Play sound on click");
            engDictionary.Add("sound on click", "Sound on click");
            engDictionary.Add("sound on circle", "Sound on circle");
            engDictionary.Add("play sound on circle", "Play sound on circle");
            engDictionary.Add("Bell ding", "Bell ding");
            engDictionary.Add("Ding", "Ding");
            engDictionary.Add("Short tick", "Short tick");
            engDictionary.Add("Bells down", "Bells down");
            engDictionary.Add("Bells up", "Bells up");
            engDictionary.Add("about profram", "About program");

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
            rusDictionary.Add("circle length", "Длина круга");
            rusDictionary.Add("number of circles", "Количество кругов");
            rusDictionary.Add("vibrate on click", "Вибрировать на подсчете");
            rusDictionary.Add("play sound on click", "Озвучивать подсчет");
            rusDictionary.Add("sound on click", "Звук подсчета");
            rusDictionary.Add("sound on circle", "Звук при завершении круга");
            rusDictionary.Add("play sound on circle", "Озвучивать завершение круга");
            rusDictionary.Add("Bell ding", "Колокол");
            rusDictionary.Add("Ding", "Колокольчик");
            rusDictionary.Add("Short tick", "Клик");
            rusDictionary.Add("Bells down", "Перезвон отдаляющийся");
            rusDictionary.Add("Bells up", "Перезвон приближающийся");
            rusDictionary.Add("about profram", "О программе");

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
                "Сэдьхэн дуулажа жаргагты!";    //hhhhh   үүүүү    ѳѳѳѳѳ

            burDictionary.Add("sample text for font", burPana);
            burDictionary.Add("circle length", "Нэгэ таталгын тоо");
            burDictionary.Add("number of circles", "Хэды дахин татаха");
            burDictionary.Add("vibrate on click", "Дорьбодхо татахадаа");
            burDictionary.Add("play sound on click", "Татахадаа абяа гаргаха");
            burDictionary.Add("sound on click", "Татаhани абяан");
            burDictionary.Add("sound on circle", "Нэгэ таталгын дүүрhэни абяан");
            burDictionary.Add("play sound on circle", "Нэгэ таталгын дүүрхэдэ абяа гаргаха");
            burDictionary.Add("Bell ding", "Хонхо");
            burDictionary.Add("Ding", "Заахан хонхо");
            burDictionary.Add("Short tick", "Тук");
            burDictionary.Add("Bells down", "Ханхинаан холодоhон");
            burDictionary.Add("Bells up", "Ханхинаан");

            if (dictionary.ContainsKey(Languages.Buryat))
            {
                dictionary.Remove(Languages.Buryat);
            }

            dictionary.Add(Languages.Buryat, burDictionary);
            burDictionary.Add("about profram", "Энэ программа тухай");
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