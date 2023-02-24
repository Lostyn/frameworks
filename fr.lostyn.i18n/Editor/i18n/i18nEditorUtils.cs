using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.i18n {
    [InitializeOnLoad]
    public static class i18nEditorUtils 
    {
        static FileSystemWatcher watcher;
        static XmlDocument doc;

        static i18nEditorUtils() {
            if (watcher == null) {
                string filePath = FilePath;
                if (File.Exists(filePath)) {
                    watcher = new FileSystemWatcher();
                    watcher.Path = Path.GetDirectoryName(filePath);
                    watcher.Filter = Path.GetFileName(filePath);
                    watcher.IncludeSubdirectories = false;
                    watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;
                    watcher.Changed += new FileSystemEventHandler(OnChanged);
                    watcher.EnableRaisingEvents = true;  
                }
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            //Debug.Log($"{e.Name} with path {e.FullPath} has been {e.ChangeType}");
            LoadDocument(e.FullPath);
        }

        static void LoadDocument(string path) {
            Debug.Log("<color=#ff1155>[i18n]: </color> file reloaded");
            doc = new XmlDocument();
            doc.Load(path);
        }

        static string FilePath {
            get {
                string lang = PlayerPrefs.GetString("Language", "french");
                return Path.Combine(Application.streamingAssetsPath, lang + ".xml");
            }
        }

        #if UNITY_EDITOR
        public static string EditorGet(string key)
        {
            if (doc == null) LoadDocument(FilePath);
            /*
            XmlDocument _doc;
            string lang = PlayerPrefs.GetString("Language", "french");
            string filePath = Path.Combine("file://" + Application.dataPath + "/StreamingAssets/", lang + ".xml");
            _doc = new XmlDocument();
            _doc.Load(filePath);
            */
            // update text
            int idx = key.LastIndexOf('/');
            string path;
            if (idx < 0)
                path = string.Format("root/item[@key='{0}']", key);
            else
                path = string.Format("root/{0}/item[@key='{1}']", key.Substring(0, idx), key.Substring(idx + 1));

            XmlNodeList results = doc.SelectNodes(path);

            if (results.Count == 1)
                return results[0].InnerText.Replace("\\n", "\n"); ;

            return "";
        }

        public static List<string> EditorGetAll()
        {
            if (doc == null) LoadDocument(FilePath);
            
            XmlNodeList xmlResults = doc.SelectNodes("//item");
            List<string> result = new List<string>();

            for(int i = 0; i <xmlResults.Count; i++)
            {
                result.Add(FindPath(xmlResults[i]));
            }

            return result;
        }

        public static string FindPath(XmlNode node)
        {
            XmlNode parent = node.ParentNode;
            string result = node.Attributes["key"].Value;

            while(parent.Name != "root")
            {
                result = parent.Name + "/" + result;
                parent = parent.ParentNode;
            }
            return result;
        }
        #endif
    }
}