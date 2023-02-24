namespace fr.lostyn.i18n
{  
    [System.Serializable]
    public class i18nString {

        public string key;
        public string value { get {
                if (!string.IsNullOrEmpty(key))
                    return i18n.Get(key);
                return key;
        } }

        public i18nString() { }
        public i18nString(string key) { this.key = key; }
    }
}