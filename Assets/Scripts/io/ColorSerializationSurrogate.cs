using UnityEngine;
using System.Runtime.Serialization;

sealed class ColorSerializationSurrogate : ISerializationSurrogate {

    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {
        Color c = (Color) obj;
        info.AddValue("r", c.r);
        info.AddValue("g", c.g);
        info.AddValue("b", c.b);
        info.AddValue("a", c.a);
    }

    public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
    {
        Color c = (Color) obj;
        c.r = (float) info.GetValue("r", typeof(float));
        c.g = (float) info.GetValue("g", typeof(float));
        c.b = (float) info.GetValue("b", typeof(float));
        c.a = (float) info.GetValue("a", typeof(float));
        obj = c;
        return c;
    }
}
