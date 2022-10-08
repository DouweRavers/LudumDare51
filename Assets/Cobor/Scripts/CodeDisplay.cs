using TMPro;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _lineNumberText;
    [SerializeField]
    Transform _memory;

    void Update()
    {
        UpdateMemory();
        UpdateLineNumbers();
    }

    void UpdateMemory()
    {
        for (int i = 0; i < _memory.childCount; i++)
        {
            var textGUIs = _memory.GetChild(i).GetComponentsInChildren<TextMeshProUGUI>();
            textGUIs[0].text = "#" + i;
            textGUIs[1].text = "" + CodeManager.Instance.Memory[i];
        }
    }

    void UpdateLineNumbers()
    {
        string text = "<color=grey>";
        for (int i = 0; i < 150; i++)
        {
            if (CodeManager.Instance.Line == i) text += "<color=green>";
            if (CodeManager.Instance.ErrorLine == i) text += "<color=red>";
            text += i + "\n";
            if (CodeManager.Instance.Line == i || CodeManager.Instance.ErrorLine == i) text += "<color=grey>";
        }
        _lineNumberText.text = text;
    }
}
