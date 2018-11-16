using System;
using UnityEngine;

namespace DataTool
{
    public class SaveDataManager : Singleton<SaveDataManager>
    {
        private static readonly string s_userSaveDataFileName = "saveData";
        private string _saveFilePath;

        public void Init()
        {
            _saveFilePath = string.Format("{0}/{1}", Application.persistentDataPath, s_userSaveDataFileName);
        }

        public T LoadData<T>() where T : UserDataBase
        {
            return LoadData<T>(_saveFilePath);
        }

        public T LoadData<T>(string filePath) where T : UserDataBase
        {
            string saveDataFileContent;
            if (!FileTools.TryReadFileToString(filePath, out saveDataFileContent))
            {
                return null;
            }

            SaveDataHolder<T> holder = new SaveDataHolder<T>();
            holder.Deserialize(saveDataFileContent);
            return holder.UserData;
        }

        public void SaveData<T>(T userData, Version version) where T : UserDataBase
        {
            SaveData(userData, version, _saveFilePath);
        }

        public void SaveData<T>(T userData, Version version, string filePath) where T : UserDataBase
        {
            if (null == userData)
            {
                Debug.LogError("Save data failed, user data is null");
                return;
            }

            SaveDataHolder<T> holder = new SaveDataHolder<T>(version, System.DateTime.UtcNow, userData);
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