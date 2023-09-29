    using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Coverts data into a binary stream and stores it in a file and vice versa
/// </summary>

public class SaveSystem
{
    //Stores the save data into the pc
    public static void SaveGameData(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.data"; //Path to be stored

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(saveData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //Loads Save data from the file
    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/gameData.data";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = (GameData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }


    //Stores the Setting configuration of the game
    public static void SaveSettingsData(SettingManager setting)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.ini";

        FileStream stream = new FileStream(path, FileMode.Create);

        SettingData data = new SettingData(setting);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //Loads Setting data from the file
    public static SettingData LoadSettingData()
    {
        string path = Application.persistentDataPath + "/settings.ini";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingData data = (SettingData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Setings not configured! " + path);   
            return null;
        }
    }
}
