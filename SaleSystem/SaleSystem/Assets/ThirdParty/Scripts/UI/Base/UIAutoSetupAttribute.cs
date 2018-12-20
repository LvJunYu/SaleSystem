using System;

namespace UITools
{
    /// <summary>
    ///     UI自动创建的类别
    /// </summary>
    public enum EUIAutoSetupType
    {
        /// <summary>
        ///     只创建UICtrl，不创建UIView
        /// </summary>
        Add,

        /// <summary>
        ///     创建UICtrl和UIView，但是不尝试直接显示
        /// </summary>
        Create,
    }

    /// <summary>
    ///     UI自动创建
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UIAutoSetupAttribute : Attribute
    {
        /// <summary>
        ///     自动创建类别
        /// </summary>
        private readonly EUIAutoSetupType _autoSetupType;

        public EUIAutoSetupType AutoSetupType
        {
            get { return _autoSetupType; }
        }

        public UIAutoSetupAttribute(EUIAutoSetupType autoSetupType = EUIAutoSetupType.Add)
        {
            _autoSetupType = autoSetupType;
        }
    }
}