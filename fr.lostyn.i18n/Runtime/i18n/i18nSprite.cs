using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperfiction.Core.i18n
{
    [System.Serializable]
    public class i18nSprite
    {
        public string key;
        public Sprite sprite
        {
            get
            {
                i18nImagesDatabase[] dbs = Resources.LoadAll<i18nImagesDatabase>("");
                if (dbs.Length == 0) return null;

                i18nImagesDatabase db = dbs[0];
                return db.GetSprite(key);
            }
        }
        public bool HasValue => !string.IsNullOrEmpty(key);
    }

}