#if UNITY_EDITOR
using System;
using System.IO;

namespace DR.DRSound.Editor
{
    public class EnumGenerator : IDisposable
    {
        public void GenerateEnum (string enumName, string[] tags)
        {
            string ident = "	";
            string filePathAndName = "Assets/_Projects/Scripts/Framework/DRSound/" + enumName + ".cs";
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("namespace DR.DRSound");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine(ident + "public enum " + enumName);
                streamWriter.WriteLine(ident + "{");
                for (int i = 0; i < tags.Length; i++)
                {
                    streamWriter.WriteLine(ident + ident + tags[i] + (i == tags.Length - 1 ? "" : ","));
                }
                streamWriter.WriteLine(ident + "}");
                streamWriter.WriteLine("}");
            }
        }

        public void Dispose ()
        {
            
        }
    }
}
#endif
