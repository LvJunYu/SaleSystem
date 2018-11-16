using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyTools
{
    public class DataHolder<T> where T : DataBase
    {
        private Version _appVersion;
        private DateTime _saveTime;
        private T _userData;

        public T UserData
        {
            get { return _userData; }
        }

        public DataHolder () { }

        public DataHolder (Version appVersion, DateTime saveTime, T data)
        {
            _appVersion = appVersion;
            _saveTime = saveTime;
            _userData = data;
        }

        public string Serialize ()
        {
            string appVersionStr = _appVersion.ToString ();
            string saveTimeStr = Newtonsoft.Json.JsonConvert.SerializeObject (_saveTime);
            string userDataStr = Newtonsoft.Json.JsonConvert.SerializeObject (_userData);
            List<string> strList = new List<string> ();
            strList.Add (appVersionStr);
            strList.Add (saveTimeStr);
            strList.Add (userDataStr);
            return Newtonsoft.Json.JsonConvert.SerializeObject (strList);
        }

        public void Deserialize (string content)
        {
            List<string> strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>> (content);
            if (strList.Count != 3)
            {
                Debug.LogError ("Deserialze save data failed");
            }
            _appVersion = new Version (strList[0]);
            _saveTime = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime> (strList [1]);
            _userData = Newtonsoft.Json.JsonConvert.DeserializeObject<T> (strList [2]);
        }
    }
}