using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// internationalization class based on xml file located in StreamingAssets
/// Default localization is french
/// call <code>i18n.language = "english"</code> to change it
/// Sample french.xml for french localization
/// <root>
///	    <menu>
///		    <item key = "hello" > Hello world !</item>
///		    <item key = "hello1" > Hello world 1!</item>
///	    	<item key = "hello2" > Hello world 2!</item>
///	    	<item key = "hello3" > Hello world 3!</item>
///	    </menu>
///	    <test>
///	    	<step>
///	    		<restep>
///	    			<item key = "banzay" > Banzay !</item>
///	    			<item key = "banzay1" > Banzay 1!</item>
///	    		</restep>
///	    	</step>
///	    </test>
///</root>
/// </summary>
namespace Hyperfiction.Core.i18n
{  
    public class i18n : MonoBehaviour
    {

        static i18n mInstance;
        string mLanguage;
        XmlDocument _doc;
        public bool LocalizationHasBeenSet { get; private set; } = false;

        void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        static public i18n instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<i18n>();

                    if (mInstance == null)
                    {
                        GameObject go = new GameObject("_i18n");
                        DontDestroyOnLoad(go);
                        mInstance = go.AddComponent<i18n>();
                    }
                }
                return mInstance;
            }
        }

        void OnEnable() { if (mInstance == null) mInstance = this; }
        void OnDestroy() { if (mInstance == this) mInstance = null; }
        void OnDisable() { LocalizationHasBeenSet = false; }

        public string language
        {
            get
            {
                return mLanguage;
            }
            set
            {
                if (Application.isPlaying && language == value)
                    return;

                if (!string.IsNullOrEmpty(value))
                {
                    mLanguage = value;

    #if UNITY_EDITOR
                    string path = GetPathForPlatform(EditorUserBuildSettings.activeBuildTarget);
    #else
                    string path = GetPathForPlatform(Application.platform);
    #endif
                    string filePath = Path.Combine(path, value + ".xml");

                    _doc = new XmlDocument();
    #if UNITY_ANDROID
                    WWW www = new WWW(filePath);
                    while (!www.isDone) { }
                    _doc.LoadXml(www.text);
    #else
                    _doc.Load(filePath);
    #endif

                    LocalizationHasBeenSet = true;
                    TriggerLanguageUpdate();
                    PlayerPrefs.SetString("Language", mLanguage);
                    return;
                }

            }
        }

        void TriggerLanguageUpdate() {
            var array = FindObjectsOfType<MonoBehaviour>().OfType<ILocalized>();
            foreach(var localized in array) {
                localized.LanguageChanged(mLanguage);
            }
        }

        public static string GetDir()
        {

    #if UNITY_EDITOR
            return i18n.GetPathForPlatform(EditorUserBuildSettings.activeBuildTarget);
    #else
            return i18n.GetPathForPlatform(Application.platform);
    #endif

        }

    #if UNITY_EDITOR
        public static string GetPathForPlatform(BuildTarget target)
        {
            return "file://" + Application.dataPath + "/StreamingAssets/";
        }
    #endif

        public static string GetPathForPlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "jar:file://" + Application.dataPath + "!/assets/";
                case RuntimePlatform.IPhonePlayer:
                    return Application.dataPath + "/Raw";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                    return "file:///" + Application.dataPath + "/StreamingAssets/";
                // Add more build platform for your own.
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get localization value for given key base on defined language
        /// </summary>
        /// <param name="key"></param>
        /// <returns>localization value</returns>
        public static string Get(string key)
        {
            i18n _i18n = i18n.instance;
            if (!_i18n.LocalizationHasBeenSet) _i18n.language = PlayerPrefs.GetString("Language", "french");
            
            int idx = key.LastIndexOf('/');
            string path;
            if (idx < 0)
                path = string.Format("root/item[@key='{0}']", key);
            else
                path = string.Format("root/{0}/item[@key='{1}']", key.Substring(0, idx), key.Substring(idx + 1));
                
            XmlNodeList results = _i18n._doc.SelectNodes(path);
            if (results.Count > 1)
            {
                Debug.LogWarning("More than one localization key found[" + key + "]");
                return key;
            }
            else if (results.Count == 0)
            {
                Debug.LogWarning("No localization key found [" + key + "]");
                return key;
            }

            // Fix some editor which replace \n string by \\n
            return results[0].InnerText.Replace("\\n", "\n"); ;
        }
    }
}