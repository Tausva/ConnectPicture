using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private float touchDistance = 0.005f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 position = mainCamera.ScreenToWorldPoint(touch.position);
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(position, touchDistance);

                NodeAction node = null;
                int nodeIndex = int.MaxValue;
                foreach (Collider2D collider in colliderArray)
                {
                    NodeAction newNode = collider.gameObject.GetComponent<NodeAction>();
                    if (nodeIndex > newNode.Index && !newNode.IsPressed)
                    {
                        node = newNode;
                        nodeIndex = newNode.Index;
                    }
                }

                if (node != null)
                {
                    node.PressNode();
                }
            }
        }
    }
}