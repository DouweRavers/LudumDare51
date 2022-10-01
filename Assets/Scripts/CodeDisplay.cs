using TMPro;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _memoryText, _lineNumberText, _codeText;

    void Update()
    {
        //if (Time.frameCount % 10 != 0) return;
        UpdateMemory();
        UpdateLineNumbers();
        //UpdateCode();
    }

    void UpdateMemory()
    {
        string text = "Memory:\n\n";
        for (int i = 0; i < CodeManager.Instance.Memory.Length; i++)
        {
            text += $"#{i}: {CodeManager.Instance.Memory[i]}\n";
        }
        _memoryText.text = text;
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

    void UpdateCode()
    {
        string text = _codeText.text;
        int lineStart = text.IndexOf('\n') + 1;
        int lineEnd = text.IndexOf('\n', lineStart);
        int lineNumber = 0;
        while (lineEnd != -1)
        {
            if (CodeManager.Instance.Line == lineNumber) text.Insert(lineStart, "<color=green>");
            if (CodeManager.Instance.ErrorLine == lineNumber) text.Insert(lineStart, "<color=red>");
            if (CodeManager.Instance.Line == lineNumber || CodeManager.Instance.ErrorLine == lineNumber)
                text.Insert(lineStart, "<color=white>");

            lineStart = text.IndexOf('\n') + 1;
            lineEnd = text.IndexOf('\n', lineStart);
            lineNumber++;
        }
        _codeText.text = text;
    }
}
