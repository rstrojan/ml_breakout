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
        Debug.Log("Savegame called");
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
        // PUT VAR ASSIGNMENT HERE
        HighScoreEntry entry = new HighScoreEntry();
        entry.name = newName;
        entry.score = newScore;
        int insertResult = AddHighScore(entry);
        if (insertResult != -1)
        {
            Debug.Log("New High Score at Rank #" + (insertResult + 1));
        }
        else
        {
            Debug.Log("Score did not make the top 10.");
        }

        // PUT ADD/SORT HERE

        bf.Serialize(file, HighScoreList);
        file.Close();
        Debug.Log("Game data saved.");
    }


    public void LoadGame()
    {
        Debug.Log("Lodagame called");
        HighScoreList = new List<HighScoreEntry>(); //set or reset highscorelist

        //check if there is a game file.
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            //if there is open it.
            FileStream file = File.Open(Application.persistentDataPath
                            + "/MySaveData.dat", FileMode.Open);
            try //check if there is data inside
            {
                BinaryFormatter bf = new BinaryFormatter();
                HighScoreList = (List<HighScoreEntry>)bf.Deserialize(file);
                Debug.Log("Game data loaded!");
            }
            catch //let us know if not
            {
                Debug.Log("No values inside game file. Saving default values.");
            }
            finally //close the file
            {
                file.Close();
            }
        }
        else 
        {
            //if there was no file let us.
            Debug.Log("No game file found. Saving default values.");
            
        }

        // if there was no file, or no data, then put in default val
        if (HighScoreList.Count == 0)
        {
            SaveGame("abc", 0);
        }

    }

    public void ClearData()
    {
        LoadGame(); //get game data
        Debug.LogError("inside ClearData");
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            //delete the file
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            //clear the list
            HighScoreList.Clear();
            // Add a default value so object isn't empty
            Debug.Log("Game data cleared.");
        }
        else
            Debug.LogError("No save file found.");

    }

    public int AddHighScore(HighScoreEntry newEntry)
    {
        Debug.Log("In AddHighScore");
        int count = 0; // track where we are in the list
        int insertIndex = -1; //if inserted, store in here
        if(HighScoreList.Count != 0) // if the list is not empty
        {
            foreach (HighScoreEntry oldEntry in HighScoreList)
            {
                Debug.Log("In highscore loop");
                int comparedResult = CompareHighScore(newEntry, oldEntry);
                if (comparedResult == 1)
                {
                    HighScoreList.Insert(count, newEntry);
                    insertIndex = count;
                    Debug.LogError("added new");
                    break;
                }
                count++;
            }

            //Trim scores to keep at 10.
            while (HighScoreList.Count > 10)
            {
                Debug.Log("trimming list");
                HighScoreList.RemoveAt(HighScoreList.Count - 1);

            }
        }
        else
        {
            HighScoreList.Insert(count, newEntry);
        }


        Debug.LogError("returning index");
        return insertIndex;
    }


    //Compares two highscore entries. Results are relative to left hand param
    // If left is greater than right, result is 1, equal 0, less than -1
    public int CompareHighScore(HighScoreEntry x, HighScoreEntry y)
    {
      if (x.score > y.score)
        {
            return 1;
        }
      else if (x.score == y.score)
        {
            return 0;
        }
      else if (x.score < y.score)
        {
            return -1;
        }
      else
            return -99;
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



