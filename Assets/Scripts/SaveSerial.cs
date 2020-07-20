using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSerial
{
    public List<HighScoreEntry> HighScoreList;

    public void SaveGame(string newName, int newScore)
    {
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        // persistant path is C:\Users\[user]\AppData\LocalLow\[company name]
        if (!File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
            HighScoreList = new List<HighScoreEntry>();
        }
        else
        {
            LoadGame();
            file = File.Open(Application.persistentDataPath
                                        + "/MySaveData.dat", FileMode.Open);
        }
        HighScoreEntry entry = new HighScoreEntry();
        // PUT VAR ASSIGNMENT HERE
        entry.name = newName;
        entry.score = newScore;
        HighScoreList.Add(entry);

        // PUT ADD/SORT HERE

        bf.Serialize(file, HighScoreList);
        file.Close();
        Debug.Log("Game data saved.");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath
                                        + "/MySaveData.dat", FileMode.Open);
            HighScoreList = (List<HighScoreEntry>)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game data loaded!");
        }
        else
            Debug.Log("No save file found.");
    }

    public void ClearData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            HighScoreList.Clear();
            Debug.Log("Game data cleared.");
        }
        else
            Debug.LogError("No save file found.");
    }
}

[Serializable]
class SaveData
{
    public int savedInt;
    public float savedFloat;
    public bool savedBool;
}

[Serializable]
public class HighScoreEntry
{
    public string name;
    public int score;
}



