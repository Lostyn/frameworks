using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hyperfiction.Core.i18n
{        
    public class i18nSwitcher : MonoBehaviour
    {
        [Serializable]
        public class i18nToggle
        {
            public string langue;
            public Toggle toggle;
        }

        [SerializeField] List<i18nToggle> m_toggles;
                    
        public void Start()
        {
            foreach(var toggle in m_toggles)
            {
                toggle.toggle.onValueChanged.AddListener(
                    value => {
                        if (value) ChangeLangue(toggle.langue);
                    }
                );
            }

            StopAllCoroutines();
            StartCoroutine(UpdateToggles());
        }

        public void ChangeLangue(string langue)
        {
            if (i18n.instance.language != langue)
            {
                i18n.instance.language = langue;
                StopAllCoroutines();
                StartCoroutine(UpdateToggles());
            }
        }

        IEnumerator UpdateToggles()
        {
            while (!i18n.instance.LocalizationHasBeenSet) {
                yield return null;
            }

            bool itsMe;
            Vector3 pos = Vector3.zero;

            foreach(var toggle in m_toggles)
            {
                itsMe = toggle.langue == i18n.instance.language;
                toggle.toggle.isOn = itsMe;
            }
        }
    }   
}