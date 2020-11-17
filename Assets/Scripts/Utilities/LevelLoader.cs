using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : Singleton<LevelLoader>
{
    [SerializeField] private string levelFilePath = default;
    private LevelList levelList; 
    
    public LevelList LevelList { set => levelList = value; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        LoadLevelData();
    }

    public Level GetLevelInformation(int levelId)
    {
        if (CheckIfValidId(levelId))
        {
            return levelList.levels[levelId];
        }
        return new Level();
    }

    public int GetLevelCount()
    {
        return levelList.levels.Length;
    }

    public bool GetLevelCompletionStatus(int levelId)
    {
        if (CheckIfValidId(levelId))
        {
            return levelList.levels[levelId].isCompleted;
        }
        return false;
    }

    public void SetLevelCompletionStatusToTrue(int levelId)
    {
        if (CheckIfValidId(levelId))
        {
            levelList.levels[levelId].isCompleted = true;
        }
    }

    public bool CheckIfValidId(int levelId)
    {
        if (levelId < 0 || levelId >= GetLevelCount())
        {
            return false;
        }
        return true;
    }

    private void LoadLevelData()
    {
        string json = ReadFromFile(levelFilePath);

        if (json != null)
        {
            levelList = JsonUtility.FromJson<LevelList>(json);
        }
    }

    private string ReadFromFile(string filePath)
    {
        TextAsset file = Resources.Load(filePath) as TextAsset;

        if (file != null)
        {
            return file.ToString();
        }
        return null;
    }
}

[Serializable]
public class Level
{
    public string[] level_data;
    public bool isCompleted;
}

[Serializable]
public class LevelList
{
    public Level[] levels;
}