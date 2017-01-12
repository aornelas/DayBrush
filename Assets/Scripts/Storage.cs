using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Provides save and load functions for Paintings.
/// Based mostly on:
/// http://answers.unity3d.com/questions/967840/saving-your-scene-and-location-in-game.html
/// </summary>
public static class Storage {

    private static string filePath = Application.persistentDataPath + "/painting.txt";

    public static void Save (Painting p)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (filePath);
        bf.Serialize(file, p);
        file.Close();
        Debug.Log("Saved Painting: " + p.name);
    }

    public static Painting Load ()
    {
        if (File.Exists(filePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Painting p = (Painting) bf.Deserialize(file);
            file.Close();
            Debug.Log("Loaded Painting: " + p.name);
            return p;
        } else {
            Debug.Log("File not found: " + filePath);
            return null;
        }
    }
}
