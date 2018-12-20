using UnityEngine;
using System;
using System.Collections;

namespace MyTools
{
    public class CoroutineProxy : MonoBehaviour
    {
        private static CoroutineProxy _instance;

        public static CoroutineProxy Instance
        {
            get
            {
                if (null == _instance)
                {
                    GameObject go = new GameObject("CoroutineProxy");
                    _instance = go.AddComponent<CoroutineProxy>();
                }
                return _instance;
            }
        }

        public static IEnumerator RunNextFrame(Action action)
        {
            yield return null;
            action.Invoke();
        }

        public static IEnumerator RunWaitForSeconds(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }

        public static IEnumerator RunWaitFrames(int count, Action action)
        {
            while (count > 0)
            {
                count--;
                yield return null;
            }
            action.Invoke();
        }
    }
}
