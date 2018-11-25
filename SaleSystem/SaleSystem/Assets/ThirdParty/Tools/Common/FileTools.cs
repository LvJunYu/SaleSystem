using System.IO;
using System.Text;
using UnityEngine;

namespace MyTools
{
    public static class FileTools
    {
        public static bool TryReadFileToBytes (string fileFullName, out byte[] bytes)
        {
            bytes = null;
            FileInfo fi = new FileInfo (fileFullName);
            if (!fi.Exists) {
                return false;
            }
            using (FileStream fs = new FileStream (fileFullName, FileMode.Open, FileAccess.Read)) {
                bytes = new byte[fs.Length];
                fs.Read (bytes, 0, (int)fs.Length);
                fs.Close ();
                return true;
            }
        }

        public static byte[] ReadFileToBytes (string fileFullName)
        {
            FileInfo fi = new FileInfo(fileFullName);
            if (!fi.Exists)
            {
                Debug.LogErrorFormat("File {0} doesn't exist.", fileFullName);
                return null;
            }
            using (FileStream fs = new FileStream (fileFullName, FileMode.Open, FileAccess.Read)) {
                byte[] buffer = new byte[fs.Length];
                fs.Read (buffer, 0, (int)fs.Length);
                fs.Close ();
                return buffer;
            }
        }

        public static bool TryReadFileToString (string fileFullName, out string content)
        {
            content = string.Empty;
            byte[] bytes;
            if (TryReadFileToBytes (fileFullName, out bytes))
            {
                if (null == bytes || bytes.Length == 0)
                {
                    return false;
                }
                content = Encoding.UTF8.GetString(bytes);
                return true;
            } 
            else
            {
                return false;
            }
        }
        public static string ReadFileToString (string fileFullName)
        {
            byte[] bytes = ReadFileToBytes(fileFullName);
            if (null == bytes || bytes.Length == 0)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        public static bool WriteBytesToFile (byte[] bytes, string fileFullName, bool overwrite = true)
        {
            FileInfo fi = new FileInfo(fileFullName);
            if (fi.Exists)
            {
                if (overwrite)
                {
                    File.Delete(fileFullName);
                }
                else
                {
                    Debug.LogErrorFormat("File {0} already exist, write file failed.", fileFullName);
                    return false;
                }
            }
            using (FileStream fs = new FileStream (fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write (bytes, 0, bytes.Length);
                fs.Flush ();
                fs.Close ();
                return true;
            }
        }

        public static bool WriteStringToFile(string content, string fileFullName, bool overwrite = true)
        {
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogErrorFormat("WriteStringToFile failed content IsNullOrEmpty! FileFullName: {0} .", fileFullName);
                return false;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            if (bytes.Length == 0)
            {
                Debug.LogErrorFormat("WriteStringToFile failed array size is 0! FileFullName: {0}", fileFullName);
                return false;
            }

            return WriteBytesToFile(bytes, fileFullName, overwrite);
        }

        public static bool DeleteFile (string fullFileName)
        {
            FileInfo fi = new FileInfo (fullFileName);
            {
                if (fi.Exists)
                {
                    fi.Delete ();
                    return true;
                }
            }
            return false;
        }
    }
}