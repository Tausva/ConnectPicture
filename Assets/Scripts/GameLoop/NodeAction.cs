using UnityEngine;
using TMPro;

public class NodeAction : MonoBehaviour, IObjectPoolController
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeNodeSprite = default;
    [SerializeField] private Sprite completedNodeSprite = default;

    [Header("Text component")]
    [SerializeField] private Vector3 textOffset = default;
    [SerializeField] private float textFadeAwayDuration = 1;
    private GameObject textInstance;

    private PictureManager parentHandler;
    private Camera mainCamera;

    public int Index { get; private set; }
    public bool IsPressed { get; private set; } = false;

    public void PressNode ()
    {
        if (!IsPressed && parentHandler.CheckCondition(Index, transform.position))
        {
            GetComponent<SpriteRenderer>().sprite = completedNodeSprite;

            TextFadeAway fadeAwayEffect = textInstance.AddComponent<TextFadeAway>();
            fadeAwayEffect.Duration = textFadeAwayDuration;

            IsPressed = true;
        }
    }

    public void InstantiateNode(Vector2 position, int index, PictureManager parent)
    {
        transform.position = position;
        this.Index = index;
        parentHandler = parent;

        DrawNumber();
    }

    private void DrawNumber()
    {
        textInstance = ObjectPool.GetPooledObject(ObjectPoolEnum.NodeText);
        mainCamera = mainCamera ?? Camera.main;
        textInstance.transform.position = mainCamera.WorldToScreenPoint(transform.position + textOffset);
        textInstance.GetComponent<TextMeshProUGUI>().text = (Index + 1).ToString();
    }

    public void Activate()
    {
        IsPressed = false;
        GetComponent<SpriteRenderer>().sprite = activeNodeSprite;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.transform.parent = null;
        gameObject.SetActive(false);
    }
}