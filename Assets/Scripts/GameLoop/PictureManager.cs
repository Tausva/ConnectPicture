using System;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    private const int CanvasSize = 1000;
    private bool isLevelFinished = false;

    //Node information
    private Vector2 startingPosition;
    private float pictureHeight;
    private float pictureWidth;

    private int currentIndex;
    private int finalIndex;
    private int pictureIndex;

    private Vector2 startingNodePosition;
    private Vector2 lastNodePosition;

    //Rope information
    private GameObject ropeInstance;
    private LineRenderer ropeLineRenderer;

    private Queue<Tuple<Vector2, Vector2>> ropeDrawingQueue;
    private Tuple<Vector2, Vector2> currectRope;
    private bool ropeDrawingFinished = true;
    
    private Vector2 travelingPosition;
    [SerializeField] private float ropeSpeed = 40;
    private float acceptableDistance = 0.1f;
    private float startingDistance;

    private void Start()
    {
        ropeDrawingQueue = new Queue<Tuple<Vector2, Vector2>>();

        Camera mainCamera = Camera.main;
        Vector2 sceneDimensions = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float adjustedX = -sceneDimensions.x + (sceneDimensions.x - sceneDimensions.y);
        startingPosition = mainCamera.transform.position + new Vector3(adjustedX, sceneDimensions.y, 0);

        pictureHeight = sceneDimensions.y * 2;
        pictureWidth = sceneDimensions.y * 2;

        LoadPicture(PlayerPrefs.GetInt("PictureId", 0));
    }

    private void Update()
    {
        //Check if there is a rope to draw
        if (ropeDrawingFinished)
        {
            if (ropeDrawingQueue.Count > 0)
            {
                currectRope = ropeDrawingQueue.Dequeue();
                travelingPosition = currectRope.Item1;
                ropeDrawingFinished = false;

                startingDistance = Vector2.Distance(currectRope.Item1, currectRope.Item2);
            }
            else if (isLevelFinished)
            {
                LevelLoader.Instance.SetLevelCompletionStatusToTrue(pictureIndex);
                GameController.StartEndSequence();
            }
        }

        //Draw rope
        if (!ropeDrawingFinished)
        {
            if (ropeInstance == null)
            {
                ropeInstance = ObjectPool.GetPooledObject(ObjectPoolEnum.Rope);
                ropeInstance.transform.parent = transform;
                travelingPosition = currectRope.Item1;

                ropeLineRenderer = ropeInstance.GetComponent<LineRenderer>();
                ropeLineRenderer.SetPosition(0, currectRope.Item1);
            }

            Vector2 direction = (currectRope.Item2 - currectRope.Item1).normalized;
            float distance = Vector2.Distance(currectRope.Item1, travelingPosition);
            travelingPosition += direction * ropeSpeed * Time.deltaTime;

            ropeLineRenderer.SetPosition(1, travelingPosition);

            if (distance >= startingDistance - acceptableDistance)
            {
                ropeLineRenderer.SetPosition(1, currectRope.Item2);

                ropeInstance = null;
                ropeDrawingFinished = true;
            }
        }
    }

    public bool LoadPicture()
    {
        if (pictureIndex + 1 < LevelLoader.Instance.GetLevelCount())
        {
            LoadPicture(pictureIndex + 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadPicture(int levelId)
    {
        UnloadPicture();

        finalIndex = -1;
        Level newLevel = LevelLoader.Instance.GetLevelInformation(levelId);

        int coordsCount = newLevel.level_data.Length;
        coordsCount = coordsCount % 2 == 0 ? coordsCount : coordsCount--;

        int sortingOrder = 1000;
        for (int i = 0; i < coordsCount; i += 2)
        {
            int x, y;

            bool xIntParsed = int.TryParse(newLevel.level_data[i], out x);
            bool yIntParsed = int.TryParse(newLevel.level_data[i + 1], out y);

            if (xIntParsed && yIntParsed)
            {
                Vector2 nodePosition = startingPosition + new Vector2(((float)x / CanvasSize) * pictureWidth, ((float)-y / CanvasSize) * pictureHeight);

                GameObject childNode = ObjectPool.GetPooledObject(ObjectPoolEnum.Node);
                childNode.transform.parent = transform;
                childNode.GetComponent<NodeAction>().InstantiateNode(nodePosition, i / 2, this);

                childNode.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder--;

                finalIndex++;
            }
        }

        currentIndex = -1;
        pictureIndex = levelId;
    }

    public void UnloadPicture()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.GetComponent<NodeAction>() != null)
            {
                ObjectPool.ReturnPooledObject(ObjectPoolEnum.Node, child.gameObject);
            }
            else
            {
                ObjectPool.ReturnPooledObject(ObjectPoolEnum.Rope, child.gameObject);
            }
        }
        isLevelFinished = false;
    }

    public bool CheckCondition(int index, Vector2 position)
    {
        if (index - 1 == currentIndex)
        {
            currentIndex++;

            if (index > 0)
            {
                AddNewRopeToQueue(lastNodePosition, position);
            }

            if (index == 0)
            {
                startingNodePosition = position;
            }
            lastNodePosition = position;

            if (currentIndex == finalIndex)
            {
                if (index > 0)
                {
                    AddNewRopeToQueue(position, startingNodePosition);
                }
                isLevelFinished = true;
            }
            return true;
        }
        return false;
    }

    private void AddNewRopeToQueue(Vector2 origin, Vector2 target)
    {
        ropeDrawingQueue.Enqueue(new Tuple<Vector2, Vector2>(origin, target));
    }
}