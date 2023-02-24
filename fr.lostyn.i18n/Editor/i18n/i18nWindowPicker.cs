using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.i18n
{  
    public delegate void i18nSelectKeyHandler(string result);
    public class i18nWindowPicker : EditorWindow
    {
        i18nSelectKeyHandler _handler;
        List<string> _candidates;
        string _filter;
        Vector2 scrollPos;

        public static i18nWindowPicker OpenWithKey(i18nSelectKeyHandler handler)
        {
            i18nWindowPicker windowInstance = ScriptableObject.CreateInstance<i18nWindowPicker>();
            windowInstance.Init(handler);
            windowInstance.titleContent = new GUIContent("i18n key picker");
            windowInstance.Show();

            return windowInstance;
        }

        private void OnGUI()
        {
            string result = "";

            string cacheFilter = _filter;
            _filter = GUILayout.TextField(_filter);

            if (_filter != cacheFilter)
                PlayerPrefs.SetString("i18nPicker_filter", _filter);

            List<string> filteredCandidates = _candidates.FindAll((obj) => obj.Contains(_filter));

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 40));

            for (int i = 0; i < filteredCandidates.Count; i++)
            {
                if (GUILayout.Button(filteredCandidates[i])) {
                    result = filteredCandidates[i];
                }
            }

            EditorGUILayout.EndScrollView();

            if (!string.IsNullOrEmpty(result) && _handler != null)
            {
                _handler(result);
                _handler = null;
                Close();
            }
        }

        void Init(i18nSelectKeyHandler handler)
        {
            _handler = handler;
            _filter = PlayerPrefs.GetString("i18nPicker_filter", ""); ;
            _candidates = i18nEditorUtils.EditorGetAll();
        }

        private void OnLostFocus()
        {
            if (_handler != null) {
                _handler = null;
                Close();
            }
        }
    }
}