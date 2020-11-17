using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPicture : MonoBehaviour
{
    private int levelId;

    public int LevelId { set => levelId = value; }

    public void Load()
    {
        PlayerPrefs.SetInt("PictureId", levelId);
        SceneManager.LoadScene(SceneEnum.GameScene.ToString());
    }
}