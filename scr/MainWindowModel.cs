#region Reference

using System.Collections.ObjectModel;
using Caliburn.Micro;
using System.Windows.Controls;
using System.Collections.Generic;
using System;

#endregion

namespace SoftwaresAnalyze
{
    using System.Windows;

    public class MainWindowModel : Screen
    {
        #region Fields

        private string _displayName;
        private IList<AppModel> _appList;
        private DataProvider _dataProvider;
        private bool _isAnalizing;
        private bool _isEnableVisiable;
        private int _count;
        private string _filterText;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            set
            {
                this._count = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._count; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is progressing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is progressing; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnableVisiable
        {
            set
            {
                this._isEnableVisiable = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isEnableVisiable; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is analizing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is analizing; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnalizing
        {
            set
            {
                this._isAnalizing = value;
                this.IsEnableVisiable = !value;
                this.NotifyOfPropertyChange();
            }
            get { return this._isAnalizing; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FilterText
        {
            set
            {
                this._filterText = value;
                this.Filter();
                this.NotifyOfPropertyChange();
            }
            get { return this._filterText; }
        }

        /// <summary>
        /// Gets or sets the application list.
        /// </summary>
        /// <value>
        /// The application list.
        /// </value>
        public ObservableCollection<AppModel> AppList { get; set; }
        
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            set
            {
                this._displayName = value;
                this.NotifyOfPropertyChange();
            }
            get { return this._displayName; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowModel"/> class.
        /// </summary>
        public MainWindowModel()
        {
            // Default constructor
            this.DisplayName = "SoftwareAnalyze".ToUpper();
            this.IsAnalizing = false;
            this._dataProvider = new DataProvider();
            this.AppList = new ObservableCollection<AppModel>();
            this._appList = new List<AppModel>();
            this.AppList.CollectionChanged += (sender, e) =>
            {
                this.Count = this.AppList.Count;
            };

            Application.Current.Exit += this.Current_Exit;
        }

        /// <summary>
        /// Handles the Exit event of the Current control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void Current_Exit(object sender, ExitEventArgs e)
        {
            if (this._appList == null || this._appList.Count <= 0)
            {
                return;
            }

            foreach (var app in this._appList)
            {
                try
                {
                    app.RegistryKey.Close();
                }
                catch
                {
                }
            }
        }

        #region Methods

        /// <summary>
        ///     Analizings this instance.
        /// </summary>
        public void Analizing()
        {
            if (!this.IsAnalizing)
            {
                this.IsAnalizing = true;

                var data = this._dataProvider.GetData();
                this._appList.Clear();

                foreach (var appModel in data)
                {
                    this._appList.Add(appModel);
                }

                this.Filter();
                this.IsAnalizing = false;
            }
        }

        /// <summary>
        /// Filters this instance.
        /// </summary>
        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(this.FilterText))
            {
                this.AppList.Clear();
                foreach (var item in this._appList)
                {
                    var currentDsp = item.DisplayName.ToLower();
                    var filterDsp = this.FilterText.ToLower();
                    if (currentDsp == filterDsp || currentDsp.Contains(filterDsp))
                    {
                        this.AppList.Add(item);
                    }
                }
            }
            else
            {
                this.AppList.Clear();
                foreach (var item in this._appList)
                {
                    this.AppList.Add(item);
                }
            }
            
        }

        /// <summary>
        /// Clears the filter.
        /// </summary>
        public void ClearFilter()
        {
            this.FilterText = string.Empty;
        }

        #endregion
    }
}