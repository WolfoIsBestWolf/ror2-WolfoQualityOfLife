using R2API;


namespace WolfoQualityOfLife
{
    public class Language_English
    {
        public static void Start()
        {
            DeathMessages();
            LanguageAPI.Add("SHRINE_CHANCE_SUCCESS_MESSAGE_UPGRADE_2P", "<style=cShrine>You offer to the shrine and are rewarded (Lucky)!</color>");
            LanguageAPI.Add("SHRINE_CHANCE_SUCCESS_MESSAGE_UPGRADE", "<style=cShrine>{0} offered to the shrine and was rewarded (Lucky)!</color>");

            LanguageAPI.Add("SHRINE_CHANCE_SUCCESS_MESSAGE_DOLL_2P", "<style=cShrine>Your chance doll felt lucky!</color>");
            LanguageAPI.Add("SHRINE_CHANCE_SUCCESS_MESSAGE_DOLL", "<style=cShrine>{0}'s chance doll felt lucky!</color>");

            LanguageAPI.Add("OBJECTIVE_SECRET_GEODE", "Crack the <color=#FFE880>Aurelionite Geodes</color> ({0}/{1})", "en");
            LanguageAPI.Add("OBJECTIVE_SECRET_GEODE_FAIL", "<color=#808080><s>Crack the <color=#FFE880>Aurelionite Geodes</color> ({0}/{1})</color></s>", "en");


            LanguageAPI.Add("REMINDER_KEY", "Unlock the <style=cisDamage>Rusty Lockbox</style>", "en");
            LanguageAPI.Add("REMINDER_KEY_MANY", "Unlock the <style=cisDamage>Rusty Lockbox</style> ({0}/{1})", "en");

            LanguageAPI.Add("REMINDER_KEYVOID", "Unlock the <color=#FF9EEC>Encrusted Lockbox</color>", "en");
            LanguageAPI.Add("REMINDER_KEYVOID_MANY", "Unlock the <color=#FF9EEC>Encrusted Lockbox</color> ({0}/{1})", "en");

            LanguageAPI.Add("REMINDER_FREECHEST", "Collect free <style=cIsHealing>delivery</style>", "en");
            LanguageAPI.Add("REMINDER_FREECHEST_MANY", "Collect free <style=cIsHealing>delivery</style> ({0}/{1})", "en");

            LanguageAPI.Add("REMINDER_FREECHESTVOID", "Find and activate the <style=cIsVoid>Lost Battery</style>", "en");

            LanguageAPI.Add("REMINDER_SALESTAR", "Make use of <style=cIsUtility>Sale Star</style>", "en");
            LanguageAPI.Add("REMINDER_COUNT", " ({0}/{1})", "en");

            //<style=cIsVoid>Lost Battery</style>
        }
        public static void DeathMessages()
        {
            LanguageAPI.Add("DEATH_DOT_FRACTURE_2P", "", "en");
            LanguageAPI.Add("DEATH_DOT_FRACTURE", "", "en");

            LanguageAPI.Add("DEATH_DOT_GENERIC_2P", "", "en");
            LanguageAPI.Add("DEATH_DOT_GENERIC", "", "en");

            LanguageAPI.Add("DEATH_VOID_FOG_2P", "", "en");
            LanguageAPI.Add("DEATH_VOID_FOG", "", "en");

            LanguageAPI.Add("DEATH_VOID_EXPLODE_2P", "", "en");
            LanguageAPI.Add("DEATH_VOID_EXPLODE", "", "en");

            LanguageAPI.Add("DEATH_TWISTED_2P", "", "en");
            LanguageAPI.Add("DEATH_TWISTED", "", "en");

            LanguageAPI.Add("DEATH_FALL_DAMAGE_2P", "", "en");
            LanguageAPI.Add("DEATH_FALL_DAMAGE", "", "en");

            LanguageAPI.Add("DEATH_FRIENDLY_SUICIDE_2P", "You were killed by yourself.", "en");
            LanguageAPI.Add("DEATH_FRIENDLY_SUICIDE", "", "en");

            LanguageAPI.Add("DEATH_FRIENDLY_2P", "", "en");
            LanguageAPI.Add("DEATH_FRIENDLY", "", "en");

            LanguageAPI.Add("DEATH_ECHO_2P", "", "en");
            LanguageAPI.Add("DEATH_ECHO", "", "en");

            LanguageAPI.Add("DEATH_GLASS_2P", "", "en");
            LanguageAPI.Add("DEATH_GLASS", "", "en");

            LanguageAPI.Add("DEATH_GENERIC_2P", "", "en");
            LanguageAPI.Add("DEATH_GENERIC", "", "en");

            LanguageAPI.Add("DEATH_DAMAGE", " ({{0}:F2} damage taken)</style>", "en");

        }

    }
}