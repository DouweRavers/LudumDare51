using TMPro;
using UnityEngine;

public class ChallengesDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] challenges;

    [SerializeField]
    TextMeshProUGUI overview;

    void Update()
    {
        string text = "";
        var challengeValues = LevelManager.Instance.Challenges;
        for (int i = 0; i < challengeValues.Length; i++)
        {
            challenges[i].color = challengeValues[i] ? Color.grey : Color.white;
            text += challengeValues[i] ? "<color=green>" : "<color=red>";
            text += $"Challenge {i}\n";
        }
        overview.text = text;
    }

    public void LoadMainMenu()
    {
        SceneManager.Instance.LoadMainMenu();
    }
}
