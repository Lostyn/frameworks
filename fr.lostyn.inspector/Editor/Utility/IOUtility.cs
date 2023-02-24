using System.IO;

namespace fr.lostyneditor.inspector {
    public static class IOUtility {

        public static void WriteToFile(string filePath, string content) {
            using( FileStream fileStream = new FileStream( filePath, FileMode.Create, FileAccess.Write ) ) {
                using( StreamWriter streamWriter = new StreamWriter( fileStream, System.Text.Encoding.ASCII ) ) {
                    streamWriter.WriteLine( content );
                }
            }
        }
    }
}