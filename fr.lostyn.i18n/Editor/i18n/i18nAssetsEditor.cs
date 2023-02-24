using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using Hyperfiction.Core.i18n;
using System.Linq;
using UnityEngine.Networking;
using System;
using System.Xml;

namespace Hyperfiction.Editor.Core.i18n
{
    [CustomEditor(typeof(i18nAssets), true)]
    public class i18nAssetsEditor : UnityEditor.Editor
    {
        SerializedProperty googleLink;

        protected void OnEnable() {
            
            googleLink = serializedObject.FindProperty("googleLink");
        }

        public void OnInspector()
        {
            using(new EditorGUILayout.VerticalScope()) {
                GUILayout.Label("Google link:");

                using(new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.PropertyField(googleLink, GUIContent.none);
                    if (GUILayout.Button(">")) {
                        Application.OpenURL(UnfixURL(googleLink.stringValue));
                    }
                }
            }
            GUILayout.Space(5);

            if( GUILayout.Button("Import") ) {
                string url = FixURL(googleLink.stringValue);
                Import(url);
            }
        }

        void Import(string url) {
            UnityWebRequest request = UnityWebRequest.Get(url);
            AsyncOperation ao = request.SendWebRequest();

            ao.completed += (AsyncOperation) => {
                if (request.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(request.responseCode);
                } else {
                    Parse(request.downloadHandler);
                }
            };
        }

        void Parse(DownloadHandler handler) {
            string[] lines = handler.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.None)
                            .Where( o => o.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                            .ToArray();
        
            if (lines.Length == 0) return;

            List<string> langues = lines[0].Split(new[] {'\t'}).Skip(1).ToList();
            List<XmlDocument> docs = langues.Select( o => {
                var doc = new XmlDocument();
                doc.AppendChild( doc.CreateElement("root"));
                return doc;
            }).ToList();
            
            
            string[] values;
            XmlNode node;
            for(int i = 1; i < lines.Length; i++) {
                values = lines[i].Split(new[] {'\t'});

                for(int d=0;d<docs.Count;d++){
//                foreach(var doc in docs) {
                    node = CreateOrGet(values[0], docs[d].ChildNodes[0], docs[d]);
                    var cdata = docs[d].CreateCDataSection(Convert.ToString(values[d+1]).Trim());
                    node.AppendChild(cdata);
                    //Debug.Log(values[1]);
                    //node.InnerXml = "<![CDATA[" + values[1] + "]]>";
                }
            }

            for(int i = 0; i < langues.Count; i++) {
                docs[i].Save( Application.streamingAssetsPath + "/" + langues[i] + ".xml");
            }

            AssetDatabase.Refresh();
        }

        string GetLine(string key, string value) {
            return "";
        }

        XmlNode CreateOrGet(string path, XmlNode baseNode, XmlDocument doc) {
            string[] childs = path.Split(new[] { '/' });
            XmlNode node = baseNode;

            for(int i = 0; i < childs.Length; i++) {
                var nnode = node.SelectSingleNode(childs[i]);

                if (nnode == null) {
                    if (i == childs.Length - 1) {
                        nnode = node.AppendChild(doc.CreateElement("item")); 
                        XmlAttribute attr = doc.CreateAttribute("key");
                        attr.Value = childs[i];
                        nnode.Attributes.SetNamedItem(attr);

                        return nnode;
                    }
                    
                    nnode = node.AppendChild(doc.CreateElement(childs[i]));
                }

                node = nnode;
            }

            return node;
        }


        /// <summary>
        /// Reformat URL to match document or spreadsheets export url
        ///     document as txt
        ///     spreadsheets as csv
        /// </summary>
        /// <param name="url">public url</param>
        /// <returns>Fixed URL</returns>
        public static string FixURL(string url)
        {
            // if it's a Google Docs URL, then grab the document ID and reformat the URL
            if (url.StartsWith("https://docs.google.com/document/d/"))
            {
                var docID = url.Substring( "https://docs.google.com/document/d/".Length, 44 );
                return string.Format("https://docs.google.com/document/export?format=txt&id={0}&includes_info_params=true", docID);
            }
            if (url.StartsWith("https://docs.google.com/spreadsheets/d/"))
            {
                var docID = url.Substring( "https://docs.google.com/spreadsheets/d/".Length, 44 );
                return string.Format("https://docs.google.com/spreadsheets/export?format=tsv&id={0}", docID);
            }
            return url;
        }

        /// <summary>
        /// Oposition as FixURL
        ///     See
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UnfixURL(string url)
        {
           // if it's a Google Docs URL, then grab the document ID and reformat the URL
            if (url.StartsWith("https://docs.google.com/document/export?format=txt"))
            {
                var docID = url.Substring( "https://docs.google.com/document/export?format=txt&id=".Length, 44 );
                return string.Format("https://docs.google.com/document/d/{0}/edit", docID);
            }
            if (url.StartsWith("https://docs.google.com/spreadsheets/export?format=csv"))
            {
                var docID = url.Substring( "https://docs.google.com/spreadsheets/export?format=csv&id=".Length, 44 );
                return string.Format("https://docs.google.com/spreadsheets/d/{0}", docID);
            }
            return url;
        }
    }
}