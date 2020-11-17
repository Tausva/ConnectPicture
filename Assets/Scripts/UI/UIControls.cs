using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPanel = default;
    [SerializeField] private PictureManager picture = default;

    private void Start()
    {
        GameController.InjectUiControls(this);
    }

    public void EnableButtonsPanel()
    {
        buttonsPanel.SetActive(true);
    }

    public void DisableButtonsPanel()
    {
        buttonsPanel.SetActive(false);
    }

    public void GoToLevelMenu()
    {
        SceneManager.LoadScene(SceneEnum.LevelSelection.ToString());
    }

    public void LoadNextLevel()
    {
        DisableButtonsPanel();
        if (!picture.LoadPicture())
        {
            GoToLevelMenu();
        }
    }
}