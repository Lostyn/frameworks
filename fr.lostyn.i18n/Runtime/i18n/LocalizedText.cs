using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace fr.lostyn.i18n
{        
    public class LocalizedText : MonoBehaviour, ILocalized
    {

        public i18nString key;
        public Dictionary<string, string> replaces;

        #region getter

        public float preferredHeight{
            get
            {
                Text text = GetComponent<Text>();
                if (text != null) return text.preferredHeight;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) return tm.preferredHeight;

                return 0;
            }
        }

        public float preferredWidth
        {
            get
            {
                Text text = GetComponent<Text>();
                if (text != null) return text.preferredWidth;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) return tm.preferredWidth;

                return 0;
            }
        }

        public float fontSize
        {
            get
            {
                Text text = GetComponent<Text>();
                if (text != null) return text.fontSize;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) return tm.fontSize;

                return 0;
            }

            set
            {
                Text text = GetComponent<Text>();
                if (text != null) text.fontSize = (int)value;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) tm.fontSize = value;
            }
        }

        public bool autoSize
        {
            get
            {
                Text text = GetComponent<Text>();
                if (text != null) return text.resizeTextForBestFit;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) return tm.enableAutoSizing;

                return false;
            }

            set
            {
                Text text = GetComponent<Text>();
                if (text != null) text.resizeTextForBestFit = value;

                TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
                if (tm != null) tm.enableAutoSizing = value;
            }
        }

        #endregion

        void Start () {
            SetText(key.value);   
        }

        public void Set(string locKey)
        {
            key.key = locKey;
            SetText(key.value);
        }

        public void Set(string locKey, Dictionary<string, string> replaces)
        {
            this.replaces = replaces;
            key.key = locKey;
            SetText(key.value);
        }

        public void SetText(string msg)
        {
            if (replaces != null)
            {
                foreach (var pair in replaces)
                {
                    msg = msg.Replace(pair.Key, pair.Value);
                }
            }

            Text text = GetComponent<Text>();
            if( text != null ) text.text = msg;

            TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();
            if( tm != null ) tm.text = msg;
        }

        public void LanguageChanged(string language)
        {
            SetText(key.value);
        }
    }

}