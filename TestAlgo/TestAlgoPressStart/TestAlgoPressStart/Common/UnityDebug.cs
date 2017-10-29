using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public static class Debug
    {
        public static void LogFormat(string format, params object[] data)
        {
            //System.Diagnostics.Debug.WriteLine(format, data);
        }
        public static void LogErrorFormat(string format, params object[] data)
        {
            System.Diagnostics.Debug.WriteLine(format, data);
            System.Diagnostics.Debug.Assert(false);
        }

        public static void AssertFormat(bool test, string format, params object[] data)
        {
            System.Diagnostics.Debug.Assert(test, string.Format(format, data));
        }   
        
        public static void Assert(bool test, string message="")
        {
            System.Diagnostics.Debug.Assert(test, message);
        }
    }

    public static class Random
    {
        static System.Random random = new System.Random();
        static public int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
    /*public static class Random
    {
        
    }*/

    public static class Resources
    {
        public static object Load(string fileName)
        {
            return new TextAsset(File.ReadAllText("Resources/" + fileName + ".txt"));
        }
    }

    public class TextAsset
    {
        public string text { get; set; }
        public TextAsset(string data)
        {
            text = data;
        }
    }

}



