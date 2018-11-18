using System;
using UnityEngine;

namespace MyTools
{
    [Serializable]
    public class ResolutionManager : Singleton<ResolutionManager>
    {
        [SerializeField] private int _curHalfHeight;
        [SerializeField] private int _curHalfWidth;
        [SerializeField] private int _curHeight;
        [SerializeField] private int _curViewHeight;
        [SerializeField] private int _curWidth;

        /// <summary>
        ///     实际屏幕宽
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return _curWidth; }
        }

        /// <summary>
        ///     实际屏幕高
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return _curHeight; }
        }

        /// <summary>
        ///     实际屏幕宽一半
        /// </summary>
        /// <value>The width of the half.</value>
        public int HalfWidth
        {
            get { return _curHalfWidth; }
        }

        /// <summary>
        ///     实际屏幕高一半
        /// </summary>
        /// <value>The height of the half.</value>
        public int HalfHeight
        {
            get { return _curHalfHeight; }
        }

        public int ViewHeight
        {
            get { return _curViewHeight; }
        }

        public ResolutionManager()
        {
            Caculate();
        }

        private void Caculate()
        {
            _curHeight = Screen.height;
            _curHalfHeight = _curHeight / 2;
            _curViewHeight = _curHeight * 4 / 7;

            _curWidth = Screen.width;
            _curHalfWidth = _curWidth / 2;
        }
    }
}