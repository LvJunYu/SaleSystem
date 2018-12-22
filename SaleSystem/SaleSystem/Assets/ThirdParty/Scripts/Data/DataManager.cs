using System;
using UnityEngine;

namespace MyTools
{
    public class DataManager : Singleton<DataManager>
    {
        private const string UserSaveDataFileName = "SaveData";
        private string _saveFilePath;

        private string SaveFilePath
        {
            get
            {
                if (_saveFilePath == null)
                {
                    _saveFilePath = string.Format("{0}/{1}", Application.persistentDataPath, UserSaveDataFileName);
                }

                return _saveFilePath;
            }
        }

        public T LoadData<T>() where T : DataBase
        {
            return LoadData<T>(SaveFilePath);
        }

        public T LoadData<T>(string filePath) where T : DataBase
        {
            string saveDataFileContent;
            if (!FileTools.TryReadFileToString(filePath, out saveDataFileContent))
            {
                return null;
            }

            DataHolder<T> holder = new DataHolder<T>();
            holder.Deserialize(saveDataFileContent);
            return holder.UserData;
        }

        public void SaveData<T>(T userData, Version version) where T : DataBase
        {
            SaveData(userData, version, SaveFilePath);
        }

        public void SaveData<T>(T userData, Version version, string filePath) where T : DataBase
        {
            if (null == userData)
            {
                Debug.LogError("Save data failed, user data is null");
                return;
            }

            DataHolder<T> holder = new DataHolder<T>(version, DateTime.Now, userData);
            FileTools.WriteStringToFile(holder.Serialize(), filePath);
        }

        public void ClearData()
        {
            ClearData(SaveFilePath);
        }

        private void ClearData(string filePath)
        {
            FileTools.DeleteFile(filePath);
        }
    }
}