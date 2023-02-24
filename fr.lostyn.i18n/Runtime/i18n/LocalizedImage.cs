using UnityEngine;
using UnityEngine.UI;

namespace fr.lostyn.i18n
{
    public class LocalizedImage : MonoBehaviour, ILocalized
    {
        public i18nSprite key;
        public Image image { get; protected set; }

        void Awake()
        {
            image = GetComponent<Image>();
        }
        
        void Start()
        {
            SetSprite(key.sprite);
        }

        public void Set(string locKey)
        {
            key.key = locKey;
            SetSprite(key.sprite);
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void LanguageChanged(string language)
        {
            SetSprite(key.sprite);
        }
    }
}