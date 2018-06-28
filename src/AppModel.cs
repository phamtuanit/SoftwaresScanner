#region Reference

using System.Drawing;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Win32;

#endregion

namespace SoftwaresAnalyze
{
    public enum AppType
    {
        X86,
        X64,
        NONE
    }

    public class AppModel : PropertyChangedBase
    {
        #region Fields

        private string _displayName;
        private string _displayVersion;
        private string _displayNameTnp;
        private string _publisher;
        private Image _icon;
        private bool _isReadOnly;
        private bool _isAllowEdit;
        private bool _isAllowCancel;
        private bool _isAllowSave;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the type of the application.
        /// </summary>
        /// <value>
        ///     The type of the application.
        /// </value>
        public string AppTypeStr => this.AppType.ToString();

        /// <summary>
        /// Gets or sets the type of the application.
        /// </summary>
        /// <value>
        /// The type of the application.
        /// </value>
        public AppType AppType { set; get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is allow edit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is allow edit; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowEdit
        {
            set
            {
                this._isAllowEdit = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isAllowEdit; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is allow cancel.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is allow cancel; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowCancel
        {
            set
            {
                this._isAllowCancel = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isAllowCancel; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is allow save.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is allow save; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowSave
        {
            set
            {
                this._isAllowSave = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isAllowSave; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [edit able].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [edit able]; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            set
            {
                this._isReadOnly = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isReadOnly; }
        }

        /// <summary>
        ///     Gets or sets the icon.
        /// </summary>
        /// <value>
        ///     The icon.
        /// </value>
        public Image Icon
        {
            set
            {
                this._icon = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._icon; }
        }

        // The field-name property
        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName
        {
            set
            {
                this._displayName = value;

                if (!this.IsAllowEdit)
                {
                    this.IsAllowSave = true;
                }

                this.NotifyOfPropertyChange();
            }
            get { return this._displayName; }
        }

        /// <summary>
        /// Gets or sets the display version.
        /// </summary>
        /// <value>
        /// The display version.
        /// </value>
        public string DisplayVersion
        {
            set
            {
                this._displayVersion = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._displayVersion; }
        }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        /// <value>
        /// The publisher.
        /// </value>
        public string Publisher
        {
            set
            {
                this._publisher = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._publisher; }
        }


        /// <summary>
        ///     Gets or sets the registry key.
        /// </summary>
        /// <value>
        ///     The registry key.
        /// </value>
        public RegistryKey RegistryKey { set; get; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppModel" /> class.
        /// </summary>
        public AppModel()
        {
            // Default constructor
            this.Cancel();
        }

        #region Methods

        /// <summary>
        /// Changes the control status.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        private void ChangeControlStatus(bool value)
        {
            this.IsAllowEdit = !value;
            this.IsReadOnly = !value;
            this.IsAllowCancel = value;
            if (!value)
            {
                this.IsAllowSave = false;
            }
        }

        /// <summary>
        ///     Edits this instance.
        /// </summary>
        public void Edit()
        {
            this._displayNameTnp = this.DisplayName;
            this.ChangeControlStatus(true);
        }

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            this.ChangeControlStatus(false);
            this.DisplayName = this._displayNameTnp;
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            try
            {
                this.RegistryKey.SetValue("DisplayName", this.DisplayName);
                this.ChangeControlStatus(false);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }
        }

        public string ToolTip
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        ///     Previews the mouse double click event.
        /// </summary>
        public void PreviewMouseDoubleClickEvent()
        {
            if (this.IsReadOnly)
            {
                this.Edit();
            }
            else
            {
                this.Cancel();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"DisplayName = {this.DisplayName}, DisplayVersion = {this.DisplayVersion}, RegistryKey = {this.RegistryKey.Name}";
        }

        #endregion
    }
}