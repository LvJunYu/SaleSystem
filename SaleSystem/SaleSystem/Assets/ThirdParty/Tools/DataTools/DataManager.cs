using System;
using UnityEngine;

namespace MyTools
{
    public class DataManager : Singleton<DataManager>
    {
        private static readonly string s_userSaveDataFileName = "saveData";
        private string _saveFilePath;

        public void Init()
        {
            _saveFilePath = string.Format("{0}/{1}", Application.persistentDataPath, s_userSaveDataFileName);
        }

        public T LoadData<T>() where T : DataBase
        {
            return LoadData<T>(_saveFilePath);
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
            SaveData(userData, version, _saveFilePath);
        }

        public void SaveData<T>(T userData, Version version, string filePath) where T : DataBase
        {
            if (null == userData)
            {
                Debug.LogError("Save data failed, user data is null");
                return;
            }

            DataHolder<T> holder = new DataHolder<T>(version, System.DateTime.UtcNow, userData);
            FileTools.WriteStringToFile(holder.Serialize(), filePath);
        }

        public void ClearData()
        {
            ClearData(_saveFilePath);
        }

        public void ClearData(string filePath)
        {
            FileTools.DeleteFile(filePath);
        }
    }
}