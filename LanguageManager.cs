namespace Essence
{
    public class LanguageManager
    {
        public int lang = 1;
        public string Translate(string key)
        {
            //if (key != "Olá" && key != "Segundos." && key != "Dias" && key != "Horas" && key != "Minutos e" && key != "expira em")
            //{
            //    Console.ForegroundColor = ConsoleColor.Gray;
            //    Console.WriteLine("[Translating]  " + key);
            //    Console.ForegroundColor = ConsoleColor.White;
            //}


            if (lang == 1 && !string.IsNullOrEmpty(Translations.EN_US.ResourceManager.GetString(key)))
                return Translations.EN_US.ResourceManager.GetString(key);

            else if (lang == 2 && !string.IsNullOrEmpty(Translations.PT_BR.ResourceManager.GetString(key)))
                return Translations.PT_BR.ResourceManager.GetString(key);

            else if (lang == 3 && !string.IsNullOrEmpty(Translations.EN_US.ResourceManager.GetString(key)))
                return Translations.ES_ES.ResourceManager.GetString(key);

            else
                return key;
        }
    }
}