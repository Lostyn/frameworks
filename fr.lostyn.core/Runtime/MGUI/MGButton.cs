using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Hyperfiction.Core {
    public class MGButton : BaseSelectable, IPointerClickHandler {
        [Serializable]
        /// <summary>
        /// Function definition for a button click event.
        /// </summary>
        public class ButtonClickedEvent : UnityEvent {}

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        public ButtonClickedEvent onClick { get { return m_OnClick; } set { m_OnClick = value; } }

        [FormerlySerializedAs("onHover")]
        [SerializeField]
        private ButtonClickedEvent m_OnHover = new ButtonClickedEvent();
        public ButtonClickedEvent onHover { get { return m_OnHover; } set { m_OnHover = value; } }

        [FormerlySerializedAs("onOut")]
        [SerializeField]
        private ButtonClickedEvent m_OnOut = new ButtonClickedEvent();
        public ButtonClickedEvent onOut { get { return m_OnOut; } set { m_OnOut = value; } }


        [SerializeField] bool m_PlayAudioClipOnSelectEnter;
        [SerializeField] AudioClip m_AudioClipForOnSelectEnter;
        
        [SerializeField] bool m_PlayAudioClipOnSelectExit;
        [SerializeField] AudioClip m_AudioClipForOnSelectExit;

        [SerializeField] bool m_PlayAudioClipOnHoverEnter;
        [SerializeField] AudioClip m_AudioClipForOnHoverEnter;

        [SerializeField] bool m_PlayAudioClipOnHoverExit;
        [SerializeField] AudioClip m_AudioClipForOnHoverExit;

        AudioSource m_EffectsAudioSource;

        private void Press()
        {
            if (!IsActive() && interactable)
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                    return;

            Press();
        }

        void CreateEffectsAudioSource()
        {
            m_EffectsAudioSource = gameObject.AddComponent<AudioSource>();
            m_EffectsAudioSource.loop = false;
            m_EffectsAudioSource.playOnAwake = false;
        }

        public override void OnPointerEnter(PointerEventData eventData) {
            base.OnPointerEnter(eventData);

            if (m_PlayAudioClipOnHoverEnter && m_AudioClipForOnHoverEnter != null) {
                if (m_EffectsAudioSource == null)
                    CreateEffectsAudioSource();

                m_EffectsAudioSource.PlayOneShot(m_AudioClipForOnHoverEnter);
            }

            m_OnHover?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData) {
            base.OnPointerExit(eventData);

            if (m_PlayAudioClipOnHoverExit && m_AudioClipForOnHoverExit != null) {
                if (m_EffectsAudioSource == null)
                    CreateEffectsAudioSource();

                m_EffectsAudioSource.PlayOneShot(m_AudioClipForOnHoverExit);
            }

            m_OnOut?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);

            if (m_PlayAudioClipOnSelectEnter && m_AudioClipForOnSelectEnter != null) {
                if (m_EffectsAudioSource == null)
                    CreateEffectsAudioSource();

                m_EffectsAudioSource.PlayOneShot(m_AudioClipForOnSelectEnter);
            }
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);

            if (m_PlayAudioClipOnSelectExit && m_AudioClipForOnSelectExit != null) {
                if (m_EffectsAudioSource == null)
                    CreateEffectsAudioSource();

                m_EffectsAudioSource.PlayOneShot(m_AudioClipForOnSelectExit);
            }
        }
    }
}