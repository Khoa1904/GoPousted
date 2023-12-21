using System.Windows;
using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Views;
using GoPOS.Services;
using GoPOS.Common.ViewModels;
using GoPOS.Interface;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using static GoShared.Events.GoPOSEventHandler;
using GoShared.Events;
using GoPOS.Common.Interface.Model;
using System.Threading.Tasks;
using GoPOS.Helpers;
using System;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Diagnostics;
using GoShared.Helpers;
using GoPOS.Service.Interface.MST;
using System.Data.Entity.Core.Metadata.Edm;
using GoPOS.Models.Config;
using GoPOS.Service;
using ICSharpCode.SharpZipLib.Zip;
using GoPOS.Common.Helpers;
using GoPOS.Common.Service;
using System.Windows.Controls;
using System.Windows.Media;
using GoPOS.Service.Service.MST;
using static GoPOS.Common.PrinterLib.GeneralPrinter;
using System.Text;
using GoPOS.Servers;
using System.Dynamic;
using GoPOS.Common;
using NPOI.SS.Formula.Functions;
using GoPOS.Database;
using System.IO;

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>

    public class InitViewModel : BaseItemViewModel
    {
        public delegate void TaskProgressDelegate(POSInitItemStatus task);

        private readonly IPOSInitService initService;
        private readonly IWebInquiryService inquiryAPIService;
        private readonly IMasterVersionMangService masterVersionMangService;
        private readonly IPOSPrintService iPOSPrintService;
        private readonly IOrderPayService orderPayService;
        private int currentProgress;
        private int totalProgress;
        private int currentTaskIdx = -1;
        private SynchronizationContext _syncContext;
        private IInitPOSView _view;
        private POSInitItemStatus[] initItems = new POSInitItemStatus[]
        {
            new POSInitItemStatus()
            {
                AreaName = "프로그램 업데이트",
                MethodName = "VersionUpdate"
            },
            new POSInitItemStatus()
            {
                AreaName = "초기작업",
                MethodName = "POSInitialize"
            },
            new POSInitItemStatus()
            {
                AreaName = "API인증토큰",
                MethodName = "GetAuthToken"
            },
            new POSInitItemStatus()
            {
                AreaName = "서버시간",
                MethodName = "SyncServerTime"
            },
            new POSInitItemStatus()
            {
                AreaName = "마스터 다운로드",
                MethodName = "MasterTablesDownload"
            },
            new POSInitItemStatus()
            {
                AreaName = "포스장비 초기화",
                MethodName = "POSDevicesInit"
            },
            new POSInitItemStatus()
            {
                AreaName = "포스로드",
                MethodName = "POSStartedProcess"
            }
        };
        //private ObservableCollection<POSInitItemStatus> initItemMessages;
        private string currentText;
        private string totalText;
        private ObservableCollection<POSInitItemStatus> progressItems;
        private string selectedMessage;
        private BackgroundWorker bgw = null;

        public InitViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IPOSInitService initService,
            IWebInquiryService inquiryAPIService, IMasterVersionMangService masterVersionMangService,
            IPOSPrintService iPOSPrintService, IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            _syncContext = SynchronizationContext.Current;
            this.initService = initService;
            this.inquiryAPIService = inquiryAPIService;
            this.iPOSPrintService = iPOSPrintService;
            this.orderPayService = orderPayService;
            this.masterVersionMangService = masterVersionMangService;
            this.ProgressItems = new ObservableCollection<POSInitItemStatus>();
            this.ProgressItems.CollectionChanged += ProgressItems_CollectionChanged;
            this.ViewLoaded += InitViewModel_ViewLoaded;
            this.bgw = new BackgroundWorker();
            this.bgw.DoWork += Bgw_DoWork;
        }

        private void Bgw_DoWork(object? sender, DoWorkEventArgs e)
        {
            ProcessNextTask();
        }

        private void ProgressItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(_view.ProgressMessages) > 0)
            {
                Decorator border = VisualTreeHelper.GetChild(_view.ProgressMessages, 0) as Decorator;
                ScrollViewer scrollViewer = border.Child as ScrollViewer;
                scrollViewer.ScrollToBottom();
            }
        }

        private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private void InitViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            bgw.RunWorkerAsync();
        }

        public override bool SetIView(IView view)
        {
            _view = (IInitPOSView)view;
            return base.SetIView(view);
        }

        public ObservableCollection<POSInitItemStatus> ProgressItems
        {
            get => progressItems; set
            {
                progressItems = value;
                NotifyOfPropertyChange(() => ProgressItems);
            }
        }

        public string SelectedMessage
        {
            get => selectedMessage; set
            {
                selectedMessage = value;
                NotifyOfPropertyChange(() => SelectedMessage);
            }
        }

        public int CurrentProgress
        {
            get => currentProgress; set
            {
                currentProgress = value;
                NotifyOfPropertyChange(() => CurrentProgress);
            }
        }

        public string CurrentText
        {
            get => currentText; set
            {
                currentText = value;
                NotifyOfPropertyChange(() => CurrentText);
            }
        }

        public int TotalProgress
        {
            get => totalProgress; set
            {
                totalProgress = value;
                NotifyOfPropertyChange(() => TotalProgress);
            }
        }

        public string TotalText
        {
            get => totalText; set
            {
                totalText = value;
                NotifyOfPropertyChange(() => TotalText);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessNextTask()
        {
            currentTaskIdx++;
            var task = currentTaskIdx <= initItems.Length - 1 ? initItems[currentTaskIdx] : null;
            if (task == null)
            {
                ActivatePageItemYN(typeof(MainPageViewModel), "ActiveItem", typeof(LoginViewModel));
                return;
            }

            // Invoke methods
            var mi = this.GetType().GetMethod(task.MethodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public);
            if (mi == null)
            {
                ProcessNextTask();
                return;
            }


            mi?.Invoke(this, new object[]
            {
                task
            });
        }

        private POSInitItemStatus UpdateProgressStatus(POSInitItemStatus taskStatus, InitItemStates itemState,
            bool isSubItem = false, int internalProgress = 0, string stateMessage = "")
        {
            _syncContext.Send(state =>
            {
                if (taskStatus.FinishStatus != itemState && itemState == InitItemStates.Running)
                {
                    ProgressItems.Add(taskStatus);
                    NotifyOfPropertyChange(nameof(ProgressItems));
                    LogHelper.Logger.Trace(taskStatus.ToString());
                }

                taskStatus.FinishStatus = itemState;
                taskStatus.StateMessage = stateMessage;

                if (!isSubItem)
                {
                    SelectedMessage = taskStatus.FinishMsg;
                }
                else
                {
                    CurrentProgress = internalProgress;
                }

                if (taskStatus.FinishStatus == InitItemStates.Completed ||
                        taskStatus.FinishStatus == InitItemStates.Errored)
                {
                    TotalProgress = Convert.ToInt32(100 * (Convert.ToDouble(currentTaskIdx) / this.initItems.Length));
                }
            }, null);

            if ((taskStatus.FinishStatus == InitItemStates.Completed ||
                taskStatus.FinishStatus == InitItemStates.Errored))
            {
                LogHelper.Logger.Trace(taskStatus.ToString());
                if (!isSubItem)
                {
                    ProcessNextTask();
                }
            }

            return taskStatus;
        }

        #region Tasks

        /// <summary>
        /// SQL file in data\update folder need to be runned and deleted
        /// </summary>
        /// <param name="taskStatus"></param>
        public void VersionUpdate(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);

            #region Update DB Schema - db table schema 업데이트
            
            string directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            string dataUpdatePath = directory + @"\data\update";
            if (Directory.Exists(dataUpdatePath))
            {
                string[] sqlFiles = Directory.GetFiles(dataUpdatePath, "*.sql", SearchOption.TopDirectoryOnly);

                foreach (string sqlFile in sqlFiles)
                {
                    LogHelper.Logger.Trace("DbUpdate:{0}", Path.GetFileName(sqlFile));

                    try
                    {
                        string sqlText = File.ReadAllText(sqlFile);
                        string[] sqlLines = sqlText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        using (var db = new DataContext())
                        {
                            foreach (var sqlScript in sqlLines)
                            {
                                db.ExecuteNonQuery(sqlScript);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error(string.Format("DbUpdate:{0}-{1}", Path.GetFileName(sqlFile), ex.ToFormattedString()));
                    }

                    // Delete file
                    File.Delete(sqlFile);
                }
            }

            #endregion

            UpdateProgressStatus(taskStatus, InitItemStates.Completed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStatus"></param>
        public void POSInitialize(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);
            using (var db = new DataContext())
            {
                DataLocals.TokenInfo = db.pOS_KEY_MANGs.FirstOrDefault(p =>
                                            p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                            p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                                            p.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE);
            }

            UpdateProgressStatus(taskStatus, InitItemStates.Completed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStatus"></param>
        public void GetAuthToken(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);
            try
            {
                var result = inquiryAPIService.InqAccessToken().Result;
                taskStatus.FinishStatus = result.Item2.ResultType == EResultType.SUCCESS ? InitItemStates.Completed : InitItemStates.Errored;
            }
            catch
            {
            }
            UpdateProgressStatus(taskStatus, InitItemStates.Completed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStatus"></param>
        public void SyncServerTime(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);
            string resultMessage = string.Empty;
            bool hasErrors = false;
            try
            {
                var result = inquiryAPIService.InqServerTime().Result;
                resultMessage = result.Item2.ResultMessage;

                if (result.Item1 != null && result.Item2.ResultType == EResultType.SUCCESS)
                {
                    // Update server time
                    Debug.WriteLine(result.Item1.SYS_DT);
                    SystemTime.SetTime(result.Item1.SYS_DT);
                }
                else
                {
                    hasErrors = true;
                }
            }
            catch (Exception ex)
            {
                hasErrors = true;
                resultMessage = ex.ToFormattedString();
            }

            UpdateProgressStatus(taskStatus, hasErrors ? InitItemStates.Completed : InitItemStates.Errored, false, 0, resultMessage);
        }

        public void POSDevicesInit(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);

            initService.POSInitialize();

            if (DataLocals.AppConfig.PosOption.ReceiptPrinterYN)
            {
                iPOSPrintService.CheckShowPrinterStatus(true);
                iPOSPrintService.EndPrinting();
            }

            _eventAggregator.PublishOnUIThreadAsync(new ConfigUpdatedEventArgs());
            UpdateProgressStatus(taskStatus, iPOSPrintService.Printer._status.isOnline ?
                                                InitItemStates.Completed :
                                                InitItemStates.Errored, false, 0,
                                                iPOSPrintService.Printer._status.ToString());
        }

        public void POSStartedProcess(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);
            _syncContext.Send(state =>
            {
                if (DataLocals.AppConfig.PosInfo?.PosNo != null)
                {
                    GoPosFactory.Instance.Start();
                }
                else
                {
                    // show waning.
                }
            }, null);

            // 서버, 시리얼, 서비스 등록
            WebApiServer.Go.StartWebApiServer();

            UpdateProgressStatus(taskStatus, InitItemStates.Completed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStatus"></param>
        public async void MasterTablesDownload(POSInitItemStatus taskStatus)
        {
            UpdateProgressStatus(taskStatus, InitItemStates.Running);
            string errorMessage = string.Empty;
            try
            {
                var subStatus = new POSInitItemStatus()
                {
                    MethodName = "GET_LOCALVER",
                    AreaName = "로컬버전 확인"
                };

                UpdateProgressStatus(subStatus, InitItemStates.Running, true, 0);
                var result = await inquiryAPIService.InqMasterTableVersions();
                UpdateProgressStatus(subStatus, InitItemStates.Completed, true, 100);

                if (result.Item1 != null && result.Item2.ResultType == EResultType.SUCCESS)
                {
                    // loop to get
                    subStatus = new POSInitItemStatus()
                    {
                        MethodName = "CHK_NEWVER",
                        AreaName = "동기화 항목을 가져오기"
                    };

                    UpdateProgressStatus(subStatus, InitItemStates.Running, true, 0);
                    var updateList = masterVersionMangService.GetNeededUpdateMasterTBs(result.Item1.CHANGE_MST);
                    UpdateProgressStatus(subStatus, InitItemStates.Completed, true, 100);

                    int i = 0;
                    foreach (var itemUd in updateList)
                    {
                        var localHave = ResourceHelpers.MasterTableIds.FirstOrDefault(p => p.MST_ID == itemUd.MST_ID);
                        if (localHave == null)
                        {
                            continue;
                        }

                        subStatus = new POSInitItemStatus()
                        {
                            MethodName = itemUd.MST_TPNAME,
                            AreaName = itemUd.MST_TLNAME + " 다운로드"
                        };

                        // add new item for sub
                        UpdateProgressStatus(subStatus, InitItemStates.Running, true, 10);
                        var res = await masterVersionMangService.DownloadMasterTb(itemUd);
                        UpdateProgressStatus(subStatus, res.Item1 ? InitItemStates.Completed : InitItemStates.Errored,
                                true, 100, res.Item2);

                        // update main item
                        int progress = Convert.ToInt32(Convert.ToDouble(i + 1) * 100 / updateList.Count);
                        UpdateProgressStatus(taskStatus, InitItemStates.Running, false, progress);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToFormattedString();
                LogHelper.Logger.Error(errorMessage);
            }

            UpdateProgressStatus(taskStatus, string.IsNullOrEmpty(errorMessage) ?
                            InitItemStates.Completed : InitItemStates.Errored, false, 0, errorMessage);
        }

        #endregion
    }

    public class POSInitItemStatus : INotifyPropertyChanged
    {
        private InitItemStates finishStatus = InitItemStates.None;
        private string areaName;
        //private int progress = 0;

        public POSInitItemStatus()
        {
            FinishStatus = InitItemStates.None;
        }

        public string AreaName
        {
            get => areaName; set
            {
                areaName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AreaName)));
            }
        }

        public string MethodName { get; set; }
        /// <summary>
        /// 0: not started
        /// 1: in progress
        /// 2: completed
        /// 3: error
        /// </summary>
        public InitItemStates FinishStatus
        {
            get => finishStatus; set
            {
                finishStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FinishStatus)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FinishMsg)));
            }
        }

        public string FinishMsg
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(areaName);
                sb.Append("...");
                sb.AppendFormat(finishStatus == InitItemStates.Running ? "" : (
                        finishStatus == InitItemStates.Completed ? "완료" : "오류"));

                return sb.ToString();
            }
        }

        public string StateMessage { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}] {1}", MethodName, FinishMsg);
            if (FinishStatus == InitItemStates.Errored)
            {
                sb.AppendLine();
                sb.AppendLine(StateMessage);
            }
            return sb.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public enum InitItemStates
    {
        None = 0,
        Running = 1,
        Completed = 2,
        Errored = 3
    }
}
