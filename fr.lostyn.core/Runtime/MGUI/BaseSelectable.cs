using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class BaseSelectable : UIBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
{
    public enum SelectionState { Disable, Normal, Highlighted, Pressed }

    [Serializable]
    public class SelectableTarget {
        public Graphic target;
        public TriColor colors = new TriColor();
        public Color disableColor = Color.white;

        public void SetState(SelectionState state, float duration) {
            if (target == null) return;

            Color color;
            switch(state) {
                case SelectionState.Disable:        color = disableColor;           break;
                case SelectionState.Normal:         color = colors.Normal;          break;
                case SelectionState.Highlighted:    color = colors.Light;           break;
                case SelectionState.Pressed:        color = colors.Dark;            break;
                default:                            color = Color.black;            break;
            }

            target.CrossFadeColor(color, duration, true, true);
        }
    }

    [SerializeField] SelectableTarget m_primaryTarget = new SelectableTarget();
    public SelectableTarget PrimaryTarget => m_primaryTarget;

    [SerializeField] List<SelectableTarget> m_secondTargets = new List<SelectableTarget>();
    public List<SelectableTarget> SecondsTargets => m_secondTargets;
    
    [SerializeField] bool m_interactable = true;
    [SerializeField, Range(0, 1)] float m_fadeDuration = 0.1f;

    private bool isPointerDown {get; set;}
    private bool isPointerInside {get; set;}

    /// <summary>
    /// Is this object interactable.
    /// </summary>
    /// <value></value>
    public bool interactable {
        get => m_interactable;
        set {
            m_interactable = value;
            if (!m_interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
            OnSetProperty();
        }
    }

    private void OnSetProperty()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            DoStateTransition(currentSelectionState, true);
        else
#endif
        DoStateTransition(currentSelectionState, false);
    }

    protected SelectionState currentSelectionState {
        get {
            if (!m_interactable)
                return SelectionState.Disable;
            if (isPointerDown)
                return SelectionState.Pressed;
            if (isPointerInside)
                return SelectionState.Highlighted;
            return SelectionState.Normal;
        }
    }

    protected override void OnEnable() {
        base.OnEnable();

        isPointerDown = isPointerInside = false;
        DoStateTransition(currentSelectionState, true);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        isPointerDown = true;
        DoStateTransition(currentSelectionState, false);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        isPointerDown = false;
        DoStateTransition(currentSelectionState, false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        DoStateTransition(currentSelectionState, false);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        DoStateTransition(currentSelectionState, false);
    }

    void DoStateTransition(SelectionState state, bool instant) {
        if (!gameObject.activeInHierarchy || m_primaryTarget == null)
                return;
        
        m_primaryTarget.SetState(state, instant ? 0f : m_fadeDuration);
        for(int i = 0; i < m_secondTargets.Count; i++) {
            m_secondTargets[i].SetState(state, instant ? 0f : m_fadeDuration);
        }
    }

    public void RemoveSecondaryTarget(int index) {
        if (m_secondTargets.Count >= index)
            m_secondTargets.RemoveAt(index);
    }
    public SelectableTarget AddSecondaryTarget()
    {
        var target = new SelectableTarget();
        m_secondTargets.Add(target);
        return target;
    }

#if UNITY_EDITOR
    protected override void OnValidate() {
        base.OnValidate();
        
        // And now go to the right state.
        DoStateTransition(currentSelectionState, true);
    }
#endif
}