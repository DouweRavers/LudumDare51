using UnityEngine;
using UnityEngine.UI;

public class MainMenuDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject greeting;

    [SerializeField]
    Button _level1, _level2, _level3;

    private void Start()
    {
        if (SceneManager.Instance.LastUnlockedLevel == 0) greeting.SetActive(true);
        switch (SceneManager.Instance.LastUnlockedLevel)
        {
            case 0:
                _level1.interactable = true;
                _level2.interactable = false;
                _level3.interactable = false;
                break;
            case 1:
                _level1.interactable = true;
                _level2.interactable = true;
                _level3.interactable = false;
                break;
            default:
            case 2:
                _level1.interactable = true;
                _level2.interactable = true;
                _level3.interactable = true;
                break;
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.Instance.LoadLevel(levelNumber);
    }
}
