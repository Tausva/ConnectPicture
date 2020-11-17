using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreen : MonoBehaviour
{
    [SerializeField] GameObject levelButtonPrefab = default;
    [SerializeField] Sprite completedLevelSprite = default;
    [SerializeField] float ButtonDimentionMultiplyer = 1.1f;
    [Range(0, 1f)]
    [SerializeField] float offsetFromTop = 0.15f;

    private void Start()
    {
        int levelCount = LevelLoader.Instance.GetLevelCount();

        RectTransform canvasRectTransform = gameObject.GetComponent<RectTransform>();
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        RectTransform buttonRectTransform = (RectTransform)levelButtonPrefab.transform;
        float buttonWidth = buttonRectTransform.rect.width * ButtonDimentionMultiplyer;
        float buttonHeight = buttonRectTransform.rect.height * ButtonDimentionMultiplyer;

        int collumnAmount = (int)(canvasWidth / buttonWidth);

        int buttonPositionX = 0;
        int buttonPositionY = 0;

        for (int i = 0; i < levelCount; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, transform);

            if (LevelLoader.Instance.GetLevelCompletionStatus(i))
            {
                levelButton.GetComponent<Image>().sprite = completedLevelSprite;
            }

            levelButton.GetComponentInChildren<Text>().text = (i + 1).ToString();
            levelButton.GetComponent<LoadPicture>().LevelId = i;
            
            if (buttonPositionX == collumnAmount)
            {
                buttonPositionX = 0;
                buttonPositionY++;
            }

            float x = buttonPositionX * buttonWidth + buttonWidth / 2;
            float y = (-offsetFromTop * canvasHeight) - (buttonPositionY * buttonHeight + buttonHeight / 2);

            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            buttonPositionX++;
        }
    }
}