using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Databse of localized image
 *   Must be placed in Resources directory
 */
namespace fr.lostyn.i18n
{
    [CreateAssetMenu(menuName = "i18n/Images Database", fileName = "i18nImagesDatabase")] 
    public class i18nImagesDatabase : ScriptableObject
    {
        [Serializable]
        public class PairLangSprite
        {
            public string lang;
            public Sprite sprite;
        }

        [Serializable]
        public class ListKeyPair
        {
            public string key;
            public List<PairLangSprite> sprites;
        }

        [SerializeField] List<ListKeyPair> m_sprites;

        public Sprite GetSprite(string name)
        {
            ListKeyPair keyPair = m_sprites.FirstOrDefault(o => o.key == name);
            if (keyPair == null || keyPair.sprites.Count == 0) return null;

            PairLangSprite langSprite = keyPair.sprites.FirstOrDefault(o => o.lang == i18n.instance.language);
            if (langSprite == null)
                return keyPair.sprites[0].sprite;

            return langSprite.sprite;
        }
    }
}