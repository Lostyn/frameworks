using TMPro;
using UnityEngine;

public class VersionField : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = $"Version: {Application.version}";
    }
}
