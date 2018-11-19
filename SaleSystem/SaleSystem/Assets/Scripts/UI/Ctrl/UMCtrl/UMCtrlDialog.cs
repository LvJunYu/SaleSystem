using System;
using System.Collections.Generic;
using System.Collections;
using MyTools;
using UnityEngine;
using UITools;

namespace Sale
{
    public class UMCtrlDialog : UMCtrlGenericBase<UMViewDialog>
    {
        private readonly WaitForSeconds _autoCloseSeconds = new WaitForSeconds(2f);
        private Action[] _callbackAry = new Action[3];

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.ButtonAry[0].onClick.AddListener(OnButton1Click);
            _cachedView.ButtonAry[1].onClick.AddListener(OnButton2Click);
            _cachedView.ButtonAry[2].onClick.AddListener(OnButton3Click);
        }

        private void OnCloseBtn()
        {
            if (_cachedView)
            {
                Destroy();
            }
        }

        public void Set(string msg, string title, params KeyValuePair<string, Action>[] btnParam)
        {
            _cachedView.Content.text = msg;
            //这一版没有标题
            if (string.IsNullOrEmpty(title))
            {
                _cachedView.Title.text = "提示";
            }
            else
            {
                _cachedView.Title.text = title;
            }

            for (int i = 0; i < 3; i++)
            {
                if (i >= btnParam.Length)
                {
                    _cachedView.ButtonAry[i].gameObject.SetActive(false);
                }
                else
                {
                    _cachedView.ButtonAry[i].gameObject.SetActive(true);
                    _cachedView.ButtonTextAry[i].text = btnParam[i].Key;
                    _callbackAry[i] = btnParam[i].Value;
                }
            }

            _cachedView.Trans.sizeDelta = new Vector2(40f, 40f);
            if (btnParam.Length == 0)
            {
                CoroutineProxy.Instance.StartCoroutine(AutoClose());
                _cachedView.ButtonAry[0].SetActiveEx(true);
                _cachedView.ButtonTextAry[0].text = "确定";
            }
        }

        IEnumerator AutoClose()
        {
            yield return _autoCloseSeconds;
            OnCloseBtn();
        }

        private void OnButton1Click()
        {
            try
            {
                if (_callbackAry[0] != null)
                {
                    _callbackAry[0].Invoke();
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                OnCloseBtn();
            }
        }

        private void OnButton2Click()
        {
            try
            {
                if (_callbackAry[1] != null)
                {
                    _callbackAry[1].Invoke();
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                OnCloseBtn();
            }
        }

        private void OnButton3Click()
        {
            try
            {
                if (_callbackAry[2] != null)
                {
                    _callbackAry[2].Invoke();
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                OnCloseBtn();
            }
        }
    }
}