using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LayoutControl : UIBehaviour
{
    [SerializeField] bool m_IgnoreLayout = false;
    [SerializeField] float m_maxWidth = -1;
    [SerializeField] float m_maxHeight = -1;

    public virtual bool ignoreLayout { get { return m_IgnoreLayout; } set { m_IgnoreLayout = value; SetDirty(); } }
    public virtual float maxWidth { get { return m_maxWidth; } set { m_maxWidth = value; SetDirty(); } }
    public virtual float maxHeight { get { return m_maxHeight; } set { m_maxHeight = value; SetDirty(); } }
    

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
    }

    protected override void OnTransformParentChanged()
    {
        SetDirty();
    }

    protected override void OnDisable()
    {
        SetDirty();
        base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
        SetDirty();
    }

    protected override void OnBeforeTransformParentChanged()
    {
        SetDirty();
    }

    /// <summary>
    /// Mark the LayoutElement as dirty.
    /// </summary>
    /// <remarks>
    /// This will make the auto layout system process this element on the next layout pass. This method should be called by the LayoutElement whenever a change is made that potentially affects the layout.
    /// </remarks>
    protected void SetDirty()
    {
        if (!IsActive())
            return;
        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        SetDirty();
    }

#endif
}