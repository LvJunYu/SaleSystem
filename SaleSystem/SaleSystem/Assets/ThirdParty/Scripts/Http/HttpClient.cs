using System;
using System.Collections;
using UnityEngine;

namespace MyTools
{
    public class HttpClient
    {
        private MonoBehaviour _coroutineProxy;
        private float _timeOut = 30f;

        public HttpClient(MonoBehaviour cp)
        {
            _coroutineProxy = cp;
        }

        private IEnumerator InnerFunc(WWW www, Action callbackAction)
        {
            float startTime = Time.realtimeSinceStartup;
            while (true)
            {
                if (www.isDone)
                {
                    break;
                }

                if (Time.realtimeSinceStartup - startTime > _timeOut && www.progress < 0.01f)
                {
                    break;
                }

                yield return null;
            }

            if (Application.internetReachability == NetworkReachability.NotReachable
                || !string.IsNullOrEmpty(www.error))
            {
                float waitTime = 0.4f - (Time.realtimeSinceStartup - startTime);
                if (waitTime > 0)
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            if (callbackAction != null)
            {
                callbackAction.Invoke();
            }
        }

        public void Post(string url, WWWForm form = null, Action<WWW> callbackAction = null)
        {
            form = new WWWForm();
            form.AddField("dingdan", "231");
            var www = new WWW(url, form);
            _coroutineProxy.StartCoroutine(InnerFunc(www, () =>
            {
                if (callbackAction != null)
                {
                    callbackAction.Invoke(www);
                }
            }));
        }
    }
}