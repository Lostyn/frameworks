
using UnityEngine;

public static class CavansGroupExtensions 
{
    public static void SetVisible(this CanvasGroup grp, bool value) {
        grp.alpha = value ? 1 : 0;
        grp.SetReactif(value);
    }  

    public static void SetReactif(this CanvasGroup grp, bool value) {
        grp.interactable = grp.blocksRaycasts = value;
    }
}