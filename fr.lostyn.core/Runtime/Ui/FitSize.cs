using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class FitSize : UIBehaviour
    {
        /// <summary>
        /// The size fit modes avaliable to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Resize to the minimum size of the content.
            /// </summary>
            MinSize,
            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }

         /// <summary>
        /// Target used to fit size
        /// </summary>
        [SerializeField] RectTransform m_target;
        [SerializeField] Vector2 m_padding;

        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        public FitMode horizontalFit { get { return m_HorizontalFit; } set { if (SetStruct(ref m_HorizontalFit, value)) SetDirty(); } }
        [SerializeField] protected FitMode m_HorizontalFit = FitMode.PreferredSize;


        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode verticalFit { get { return m_VerticalFit; } set { if (SetStruct(ref m_VerticalFit, value)) SetDirty(); } }
        [SerializeField] protected FitMode m_VerticalFit = FitMode.PreferredSize;

        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        private DrivenRectTransformTracker m_Tracker;
        private Vector2 cachedSize;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        private void HandleSelfFitting()
        {
            if (m_target == null)
                return;

            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
            m_Tracker.Add(this, m_target, DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY );

            Vector2 size = GetTargetSize();
            Vector2 tSize = size - m_padding;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            rectTransform.position = m_target.position;

            m_target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tSize.x);
            m_target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tSize.y);
        }

        Vector2 GetTargetSize() {
            return new Vector2(
                horizontalFit == FitMode.MinSize ? LayoutUtility.GetMinSize(m_target, 0) + m_padding.x * 2 : LayoutUtility.GetPreferredSize(m_target, 0) + m_padding.x * 2,
                verticalFit == FitMode.MinSize ? LayoutUtility.GetMinSize(m_target, 1) + m_padding.y * 2 : LayoutUtility.GetPreferredSize(m_target, 1) + m_padding.y * 2
            );
        }

        public void Update() {
            bool dirty = false;
            
            if(GetTargetSize() != cachedSize) {
                dirty = true;
                cachedSize = m_target.sizeDelta;
            }

            if(dirty) {
                SetDirty();
            }
        }
        
        protected void SetDirty()
        {
            if (!IsActive())
                return;

            HandleSelfFitting();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        /// <summary>
        /// Originaly SetPropertyUtility.SetStruct<T>() from UnityEngine.UI
        ///     But inacessible because of interface static class ???
        /// </summary>
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}
