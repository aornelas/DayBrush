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

    public static void Save (PaintingData p)
    {
        BinaryFormatter bf = CreatePaintingBinaryFormatter();
        FileStream file = File.Create (filePath);
        bf.Serialize(file, p);
        file.Close();
        Debug.Log("Saved Painting: " + p.name);
    }

    public static PaintingData Load ()
    {
        if (File.Exists(filePath)) {
            BinaryFormatter bf = CreatePaintingBinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            PaintingData p = (PaintingData) bf.Deserialize(file);
            file.Close();
            Debug.Log("Loaded Painting: " + p.name);
            return p;
        } else {
            Debug.Log("File not found: " + filePath);
            return null;
        }
    }

    private static BinaryFormatter CreatePaintingBinaryFormatter ()
    {
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector ss = new SurrogateSelector();
        StreamingContext sc = new StreamingContext(StreamingContextStates.All);
        ColorSerializationSurrogate css = new ColorSerializationSurrogate();
        Vector3SerializationSurrogate vss = new Vector3SerializationSurrogate();

        // any non-serializable class needs to have a serialization surrogate
        ss.AddSurrogate(typeof(Color), sc, css);
        ss.AddSurrogate(typeof(Vector3), sc, vss);

        bf.SurrogateSelector = ss;
        return bf;
    }
}
