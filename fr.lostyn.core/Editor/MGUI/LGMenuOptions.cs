using System;
using Hyperfiction.Core;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Hyperfiction.Editor.Core {
    static internal class LGMenuOptions {

        static Vector2 k_TextSize = new Vector2(150, 50);
        static Vector2 k_ElementSize = new Vector2(200, 60);
        static Vector2 k_PictoSize = new Vector2(25, 25);

        static Sprite DefaultSprite => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        static Sprite DefaultPicto => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

        [MenuItem("GameObject/UI/MG/Button icon", false, 1)]
        static public void AddMGButton(MenuCommand menuCommand) {
            MGButton root = CreateMGButton();

            GameObject childPicto = CreateUIElement("Picto", root.gameObject, typeof(Image));
            Image picto = childPicto.GetComponent<Image>();
            picto.sprite = DefaultPicto;
            picto.rectTransform.sizeDelta = k_PictoSize;

            var st = root.AddSecondaryTarget();
            st.target = picto;
            st.colors.Normal = new Color().FromHex("333333");
            st.colors.Light = Color.white;
            st.colors.Dark = new Color().FromHex("333333");

            root.enabled = false;
            root.enabled = true;

            PlaceUIElementRoot(root.gameObject, menuCommand);
        }

        [MenuItem("GameObject/UI/MG/Button text", false, 10)]
        static public void AddMGButtonText(MenuCommand menuCommand) {
            MGButton root = CreateMGButton();
            GameObject childText = CreateUIElement("Text", root.gameObject, typeof(TextMeshProUGUI));
            TextMeshProUGUI text = childText.GetComponent<TextMeshProUGUI>();
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = Vector2.zero;
            text.rectTransform.offsetMax = Vector2.zero;
            text.alignment = TextAlignmentOptions.Center;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;

            text.text = "Text";
            text.color = new Color().FromHex("293040");
            
            PlaceUIElementRoot(root.gameObject, menuCommand);
        }

        
        static MGButton CreateMGButton() {
            GameObject root = CreateUIElement("MGButton", k_ElementSize, typeof(Image), typeof(MGButton));
            Image background = root.GetComponent<Image>();
            background.sprite = DefaultSprite;
            background.type = Image.Type.Sliced;

            MGButton bt = root.GetComponent<MGButton>();
            bt.PrimaryTarget.target = background;
            bt.PrimaryTarget.colors.Normal = Color.white;
            bt.PrimaryTarget.colors.Light = new Color().FromHex("333333");
            bt.PrimaryTarget.colors.Dark = Color.white;

            return bt;
        }

        static GameObject CreateText() {
            GameObject root = CreateUIElement("Text", k_TextSize, typeof(TextMeshProUGUI));
            TextMeshProUGUI text = root.GetComponent<TextMeshProUGUI>();
            text.text = "Text";
            text.color = Color.black;
            
            return root;
        }

        static GameObject CreateUIElement(string name, GameObject parent, params Type[] components) {
            GameObject child = ObjectFactory.CreateGameObject(name, components);
            SetParentAndAlign(child, parent);
            return child;
        }
        static GameObject CreateUIElement(string name, Vector2 size, params Type[] components) {
            GameObject child = ObjectFactory.CreateGameObject(name, components);
            RectTransform rTrs = (RectTransform) child.transform;
            rTrs.sizeDelta = size;
            return child;
        }

        static void SetParentAndAlign(GameObject child, GameObject parent) {
            Undo.SetTransformParent(child.transform, parent.transform, "");
            child.transform.localScale = Vector3.one;
            SetLayerRecursively(child, parent.layer);
        }

        static void SetLayerRecursively(GameObject go, int layer) {
            go.layer = layer;
            foreach(Transform child in go.transform)
                SetLayerRecursively(child.gameObject, layer);
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            
            if (parent == null) {
                parent = GetOrCreateCanvasGameObject();
            } 

            if (parent.GetComponentsInParent<Canvas>().Length == 0)
            {
                GameObject canvas = CreateNewUI();
                Undo.SetTransformParent(canvas.transform, parent.transform, "");
                parent = canvas;
            }

            GameObjectUtility.EnsureUniqueNameForSibling(element);
            SetParentAndAlign(element, parent);
            element.transform.localPosition = Vector3.zero;
            
            Undo.RegisterFullObjectHierarchyUndo(parent == null ? element : parent, "" );
            Undo.SetCurrentGroupName("Create " + element.name);
            Selection.activeGameObject = element;
        }

        static GameObject GetOrCreateCanvasGameObject() {
            GameObject selectedGo = Selection.activeGameObject;

            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (IsValidCanvas(canvas))
                return canvas.gameObject;

            Canvas[] canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();
            for (int i = 0; i < canvasArray.Length; i++)
                if (IsValidCanvas(canvasArray[i]))
                    return canvasArray[i].gameObject;

            return CreateNewUI();
        }

        static GameObject CreateNewUI() {
            var root = ObjectFactory.CreateGameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            root.layer = LayerMask.NameToLayer("UI");
            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            StageUtility.PlaceGameObjectInCurrentStage(root);
            Undo.SetCurrentGroupName("Create " + root.name);
            return root;
        }

        static bool IsValidCanvas(Canvas canvas)
        {
            if (canvas == null || !canvas.gameObject.activeInHierarchy)
                return false;

            // It's important that the non-editable canvas from a prefab scene won't be rejected,
            // but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
            if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
                return false;

            if (StageUtility.GetStageHandle(canvas.gameObject) != StageUtility.GetCurrentStageHandle())
                return false;

            return true;
        }
    }
}