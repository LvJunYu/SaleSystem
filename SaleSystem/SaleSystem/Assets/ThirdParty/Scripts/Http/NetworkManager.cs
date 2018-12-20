using UnityEngine;

namespace MyTools
{
    public class NetworkManager : MonoBehaviour
    {
        private HttpClient _appHttpClient;
        private static NetworkManager _instance;

        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("NetworkManager").AddComponent<NetworkManager>();
                }

                return _instance;
            }
        }

        public static HttpClient AppHttpClient
        {
            get { return Instance._appHttpClient; }
        }

        private void Awake()
        {
            _appHttpClient = new HttpClient(this);
        }
    }
}