using UnityEngine;
using UnityEngine.UI;

public class HorizontalLayout : LayoutGroup
{
    [SerializeField] float m_spacing = 0f;
    [SerializeField] bool m_usePreferedHeight;

    public override void SetLayoutHorizontal() { }
    public override void SetLayoutVertical() { }

    public override void CalculateLayoutInputVertical() { Layout(); }
    public override void CalculateLayoutInputHorizontal() { Layout(); }

    protected override void OnEnable() {
        base.OnEnable();
        Layout();
    }

    protected virtual void Layout() {
        m_Tracker.Clear();
        
        RectTransform child;
        LayoutControl ctr;

        float width, height, maxHeight = 0;
        float position = m_Padding.left;
        Vector3 pivot = PivotFromAlignement(childAlignment);
        Vector2 anchor = Vector2.zero;
        
        int i = 0;
        for(i = 0; i < transform.childCount; i++) {
            child = (RectTransform) transform.GetChild(i);
            ctr = child.GetComponent<LayoutControl>();
            if ((ctr && ctr.ignoreLayout) || !child.gameObject.activeSelf)
                continue;

            child.pivot = pivot;
            child.anchorMin = child.anchorMax = anchor;
            width = child.rect.width;

            height = FindHeight(child, ctr);
            maxHeight = Mathf.Max(height, maxHeight);
            Debug.Log(child.name + ":" + position);
            Vector2 pos = GetStartPosition();
            pos.x += position;
            pos.y -= height * 0.5f;
            
            child.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, width);
            child.localPosition = pos;
            

            position += width;
            
            if (HasItemAfter(i)) 
                position += m_spacing;
        }

        
        position += m_Padding.right;
        maxHeight += m_Padding.top + m_Padding.bottom;
        Debug.Log(position);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, position);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
    }

    bool HasItemAfter(int index) {
        LayoutControl ctr;
        for(int i = index + 1; i < transform.childCount; i++) {
            GameObject go = transform.GetChild(i).gameObject;
            if (!go.activeSelf)
                continue;
                
            ctr = transform.GetChild(i).GetComponent<LayoutControl>();
            
            if (!ctr)
                return true;

            if (ctr && !ctr.ignoreLayout)
                return true;
        }

        return false;
    }

    Vector2 GetStartPosition() {
        Vector2 pos = new Vector2(
            -rectTransform.rect.width * rectTransform.pivot.x,
            -rectTransform.rect.height * rectTransform.pivot.y
        );
        pos.y += rectTransform.rect.height - m_Padding.top;

        return pos;
    }

    Vector2 PivotFromAlignement( TextAnchor childAlignment ) {
        return new Vector2( 
            ( (int) childAlignment % 3 ) * 0.5f, 
            1 - ( (int) childAlignment / 3 ) * 0.5f 
        );
    }

    float FindPreferredWidth(RectTransform trs, LayoutControl ctr) {
        ILayoutElement[] elems;
        elems = trs.GetComponents<ILayoutElement>();
        float width = elems.Length > 0 ? 0f : trs.rect.width;

        // if (elems.Length > 0)
            // m_Tracker.Add(this, trs, DrivenTransformProperties.SizeDeltaX);
        
        // int i = 0;
        // for(i = 0; i < elems.Length; i++) {
        //     width = Mathf.Max(width, elems[i].preferredWidth);
        // }

        if (ctr && ctr.maxWidth != -1)
            return Mathf.Min(width, ctr.maxWidth);

        return width;
    }

    float FindHeight(RectTransform trs, LayoutControl ctr) {
        float height = trs.rect.height;
        if (m_usePreferedHeight) {
            ILayoutElement[] elems = trs.GetComponents<ILayoutElement>();

            if (elems.Length > 0) {
                height = 0;
                m_Tracker.Add( this, trs, DrivenTransformProperties.SizeDeltaY );
                for(int i = 0; i < elems.Length; i++){
                    height = Mathf.Max(height, elems[i].preferredHeight);
                }
            }
        }

        if (ctr && ctr.maxHeight != -1)
            height = Mathf.Min(height, ctr.maxHeight);
        
        trs.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        return height;
    }
}