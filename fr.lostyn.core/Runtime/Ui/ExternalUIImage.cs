using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hyperfiction.Core {
    public class ExternalUIImage : MaskableGraphic
    { 
        [Serializable]
        /// <summary>
        /// Function definition for a button click event.
        /// </summary>
        public class LoadedEvent : UnityEvent {}

        [SerializeField] string m_path;
        [SerializeField] Sprite m_defaultSprite;

        [SerializeField] Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);
        
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private LoadedEvent m_OnLoaded = new LoadedEvent();
        public UnityEvent OnError = new UnityEvent();
        public LoadedEvent OnLoaded
        {
            get { return m_OnLoaded; }
            set { m_OnLoaded = value; }
        }

        Texture2D _currentTex;
        public Texture2D texture {
            get => _currentTex;
            set {
                if (_currentTex == value)
                    return;
                
                _currentTex = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public string path {
            get => m_path;
            set {
                m_path = value;
                if (Application.isPlaying)
                    LoadSprite();
            }
        }

        public Sprite defaultSprite
        {
            get => m_defaultSprite;
            set
            {
                m_defaultSprite = value;
                LoadSprite();
            }
        }

        //protected override void OnValidate()
        //{
        //    base.OnValidate();
        //    LoadSprite();
        //}

        public override Texture mainTexture {
            get {
                if (_currentTex == null) {
                    if (material != null && material.mainTexture != null) 
                        return material.mainTexture;
                    return s_WhiteTexture;
                }

                return _currentTex;
            }
        }

        public Rect uvRect {
            get => m_UVRect;
            set {
                if (m_UVRect == value)
                    return;

                m_UVRect = value;
                SetVerticesDirty();
            }
        }

        public override void SetNativeSize() {
            Texture tex = mainTexture;
            if (tex != null) {
                int w = Mathf.RoundToInt(tex.width * uvRect.width);
                int h = Mathf.RoundToInt(tex.height * uvRect.height);
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            Dispose();
        }

        protected override void OnEnable() {
            base.OnEnable();

            if (Application.isPlaying && !string.IsNullOrEmpty(m_path)) {
                LoadSprite();
            }
        }

        protected override void OnDisable() {
            base.OnDisable();

            if (Application.isPlaying) {
                Dispose();
            }
        }

        private void LoadSprite() {
            if (m_path == "")
            {
                if (defaultSprite != null)
                {
                    texture = defaultSprite.texture;
                }
                return;
            }
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(m_path);
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();

            ao.completed += (a) => {
                if (ao.webRequest.isHttpError || ao.webRequest.isNetworkError) {
                    string error = ao.webRequest.error;
                    OnError?.Invoke();
                    Debug.LogWarning($"[ExternalImage path={m_path}]" + error);
                    //fallback to defaut
                    path = "";
                    LoadSprite();
                } else {
                    Dispose();
                    texture = DownloadHandlerTexture.GetContent(ao.webRequest);
                    OnLoaded?.Invoke();
                }

                ao.webRequest.Dispose();
            };
        }

        public void Dispose() {
            //if (_currentTex != null) DestroyImmediate(_currentTex);
            Resources.UnloadUnusedAssets();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (mainTexture != null)
            {
                if (mainTexture == Texture2D.whiteTexture && Application.isPlaying)
                    return;

                var r = GetPixelAdjustedRect();
                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
                var scaleX = mainTexture.width * mainTexture.texelSize.x;
                var scaleY = mainTexture.height * mainTexture.texelSize.y;
                {
                    var color32 = color;
                    vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMin * scaleY));
                    vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMax * scaleY));
                    vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMax * scaleY));
                    vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMin * scaleY));

                    vh.AddTriangle(0, 1, 2);
                    vh.AddTriangle(2, 3, 0);
                }
            }
        }

        
    }
}