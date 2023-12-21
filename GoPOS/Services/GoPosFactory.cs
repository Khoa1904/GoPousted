using GoPOS.Interface;
using GoPOS.Models;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using GoPOS.Service.Service.API;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoPOS.Common.Service;
using ILog = GoShared.Interface.ILog;
using GoPOS.Models.Common;
using GoPOS.Service;
using System.Dynamic;
using AutoMapper.Internal;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using NPOI.Util.ArrayExtensions;
using Caliburn.Micro;
using GoPOS.ViewModels;
using GoPOS.Service.Service.POS;

namespace GoPOS.Services
{
    public class GoPosFactory
    {
        //**--------------------------------------------------------------------------------

        #region Singleton

        public static GoPosFactory Instance
        {
            get { return Nested.Factory; }
        }

        public class Nested
        {
            internal static readonly GoPosFactory Factory;
            // Explicit static constructor to tell C# compiler
            // not to mark type as before field initialize
            static Nested()
            {
                Factory = new GoPosFactory();
            }
        }

        #endregion

        //**--------------------------------------------------------------------------------

        #region Contructor
        internal GoPosFactory()
        {
            _log = LogHelper.GetLog;

            DataLocals.KDSDataChanged += DataLocals_KDSDataChanged;
            DataLocals.KPrinterDataChanged += DataLocals_KPrinterDataChanged;

            #region api settup

            string serverUrl = string.Format("http://{0}:{1}", DataLocals.AppConfig.PosComm.MainPOSIP,
                DataLocals.AppConfig.PosComm.TRPort);
            var domain = "1".Equals(DataLocals.AppConfig.PosOption.POSFlag) ? serverUrl : DataLocals.AppConfig.PosComm.SvrURLServer;
            _apiRequest = new ApiRequest(domain, DataLocals.AppConfig.PosInfo.StoreNo,
                                    DataLocals.AppConfig.PosInfo.PosNo,
                                    DataLocals.TokenInfo.LICENSE_ID,
                                    DataLocals.TokenInfo.LICENSE_KEY);
            #endregion

            #region Trn

            _timerTrn = new Timer();
            _timerTrn.Interval = new TimeSpan(0, 0, PosOption.SyncTrnTime);
            _timerTrn.Tick += (send, e) => { OnProcessingCycleTrn(); };

            _tranApiService = new TranApiService(new POSTMangService());

            _backgroundWorkerTrn = new BackgroundWorker();
            _backgroundWorkerTrn.WorkerReportsProgress = true;
            _backgroundWorkerTrn.WorkerSupportsCancellation = true;
            _backgroundWorkerTrn.DoWork += BackgroundWorker_DoWorkTrn;


            #endregion

            #region Kds

            _timerKDS = new Timer();
            _timerKDS.Interval = new TimeSpan(0, 0, PosOption.SyncKDSTime);
            _timerKDS.Tick += (send, e) => { OnProcessingCycleKds(); };

            _backgroundWorkerKds = new BackgroundWorker();
            _backgroundWorkerKds.WorkerReportsProgress = true;
            _backgroundWorkerKds.WorkerSupportsCancellation = true;
            _backgroundWorkerKds.DoWork += BackgroundWorker_DoWorkKds;
            #endregion

            #region Print

            _timerPrt = new Timer();
            _timerPrt.Interval = new TimeSpan(0, 0, PosOption.SyncKDSTime);
            _timerPrt.Tick += (send, e) => { OnProcessingCyclePrt(); };

            _backgroundWorkerPrt = new BackgroundWorker();
            _backgroundWorkerPrt.WorkerReportsProgress = true;
            _backgroundWorkerPrt.WorkerSupportsCancellation = true;
            _backgroundWorkerPrt.DoWork += BackgroundWorker_DoWorkPrt;

            _pOSPrintService = IoC.Get<IPOSPrintService>();
            #endregion

            using (var context = new DataContext())
            {
                _deviceInfos = context.mST_INFO_KDSs.Where(t => t.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo && t.USE_YN == "Y").ToList();
                _productDevices = context.mST_INFO_KDS_PRDs.Where(t => t.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo).ToList();

                string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, "GetProducts");
                _productInfo = DapperORM.ReturnListAsync<ProductInfo>(sql, null).Result;

            }

            if (_deviceInfos == null) _deviceInfos = new List<MST_INFO_KDS>();
            if (_productDevices == null) _productDevices = new List<MST_INFO_KDS_PRD>();
            if (_productInfo == null) _productInfo = new List<ProductInfo>();

        }

        #endregion

        //**--------------------------------------------------------------------------------

        #region Member

        private ILog _log;
        private IList<MST_INFO_KDS>? _deviceInfos = new List<MST_INFO_KDS>();
        private IList<MST_INFO_KDS_PRD>? _productDevices = new List<MST_INFO_KDS_PRD>();
        private IList<ProductInfo>? _productInfo = new List<ProductInfo>();


        #region Trn
        private object _lockTrn = new object();
        private object _lockNoTrn = new object();
        private ITimer _timerTrn;
        private BackgroundWorker _backgroundWorkerTrn;
        private ApiRequest _apiRequest;
        private readonly ITranApiService _tranApiService;
        #endregion

        #region Kds
        private ITimer _timerKDS;
        private BackgroundWorker _backgroundWorkerKds;
        private Queue<SendDataInfo> _postKds = new Queue<SendDataInfo>();
        private object _lockKds = new object();
        #endregion

        #region Print
        private ITimer _timerPrt;
        private BackgroundWorker _backgroundWorkerPrt;
        private Queue<SendDataInfo> _postPrt = new Queue<SendDataInfo>();
        private object _lockPrt = new object();
        private IPOSPrintService _pOSPrintService;

        #endregion

        #endregion

        //**--------------------------------------------------------------------------------

        #region Property


        #endregion

        //**--------------------------------------------------------------------------------

        #region Public
        public void Start()
        {
            _timerTrn.Start();
        }
        public void SendKds(POS_POST_MANG pOS)
        {
            var info = new SendDataInfo(pOS);
            Task.Factory.StartNew(() => { SendToKds(info); });
        }
        public void AddKds(SendDataInfo info)
        {
            lock (_lockKds)
            {
                _postKds.Enqueue(info);
            }
        }
        //**--------------------------------------------------------------------------------
        public void SendPrint(POS_POST_MANG pOS)
        {
            var info = new SendDataInfo(pOS);
            Task.Factory.StartNew(() => { SendToPrint(info); });
        }
        public void AddPrint(SendDataInfo info)
        {
            lock (_lockPrt)
            {
                _postPrt.Enqueue(info);
            }
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Private

        private void DataLocals_KPrinterDataChanged(object sender, GoShared.Events.EventArgs<POS_POST_MANG> e)
        {
            if (e.Data == null) return;
            SendPrint(e.Data);
        }

        private void DataLocals_KDSDataChanged(object sender, GoShared.Events.EventArgs<POS_POST_MANG> e)
        {
            if (e.Data == null) return;
            SendKds(e.Data);
        }

        //**--------------------------------------------------------------------------------

        private void UpdateTranStatus(SendDataInfo info, ApiResponse response)
        {
            _tranApiService.UpdateTranPOSTMangStatus(info.Data, response);
        }

        #region Synchronized TRN data

        private void OnProcessingCycleTrn()
        {
            if (_backgroundWorkerTrn.IsBusy)
                _backgroundWorkerTrn.CancelAsync();
            else
                _backgroundWorkerTrn.RunWorkerAsync();
        }
        private void BackgroundWorker_DoWorkTrn(object? sender, DoWorkEventArgs e)
        {
            lock (_lockTrn)
            {
                // TRN DATA
                #region TRAN DATA
                var datas = _tranApiService.PickTranDataToSend(PosOption.Send_RecordPick, PosOption.Send_ErorCount);
                if (datas != null && datas.Any())
                {
                    foreach (var item in datas)
                    {
                        try
                        {
                            var tran = _tranApiService.GetTRData(item.SHOP_CODE, item.POS_NO, item.SALE_DATE, item.REGI_SEQ, item.BILL_NO);
                            if (tran != null)
                            {
                                _apiRequest.Request("client/tran").PostStringAsync<ApiResponse>(JsonHelper.InfoToJson(tran)).ContinueWith(t =>
                                {
                                    if (!t.IsFaulted)
                                    {
                                        if (t.Result != null)
                                        {
                                            _tranApiService.UpdateTranPOSTMangStatus(item, t.Result);
                                        }
                                    }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                }
                #endregion

                #region NONTRAN DATA

                //None TRN DATA
                var datasNonTran = _tranApiService.PickNonTranDataToSend(PosOption.Send_RecordPick, PosOption.Send_ErorCount);
                if (datasNonTran != null && datasNonTran.Any())
                {
                    foreach (var item in datasNonTran)
                    {
                        try
                        {
                            var tran = _tranApiService.GetNonTRData(item.SHOP_CODE, item.POS_NO, item.SALE_DATE, item.REGI_SEQ, item.BILL_NO);
                            if (tran != null)
                            {
                                var tranTest = JsonHelper.InfoToJson(tran);
                                _apiRequest.Request("client/tran/non").PostStringAsync<ApiResponse>(JsonHelper.InfoToJson(tran)).ContinueWith(t =>
                                {
                                    if (!t.IsFaulted)
                                    {
                                        if (t.Result != null)
                                        {
                                            _tranApiService.UpdateTranPOSTMangStatus(item, t.Result);
                                        }
                                    }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                }

                #endregion

                #region SET ACCOUNT


                var accDatas = _tranApiService.PickSettAccDataToSend(PosOption.Send_RecordPick, PosOption.Send_ErorCount);
                if (accDatas != null && accDatas.Any())
                {
                    foreach (var item in accDatas)
                    {
                        try
                        {
                            var tran = _tranApiService.GetTranAccount(item);
                            if (tran != null)
                            {
                                _apiRequest.Request("client/tran/account").PostStringAsync<ApiResponse>(JsonHelper.InfoToJson(tran)).ContinueWith(t =>
                                {
                                    if (!t.IsFaulted)
                                    {
                                        if (t.Result != null)
                                        {
                                            _tranApiService.UpdateTranPOSTMangStatus(item, t.Result);
                                        }
                                    }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                }

                #endregion


            }

        }

        #endregion

        #region Synchronized Kds data

        private SendDataInfo KdsPick()
        {
            lock (_lockKds)
            {
                return _postKds.Dequeue();
            }
        }
        private void OnProcessingCycleKds()
        {
            if (_backgroundWorkerKds.IsBusy)
                _backgroundWorkerKds.CancelAsync();
            else
                _backgroundWorkerKds.RunWorkerAsync();
        }
        private void BackgroundWorker_DoWorkKds(object? sender, DoWorkEventArgs e)
        {
            try
            {
                while (_postKds.Any())
                {
                    var info = KdsPick();
                    if (info != null)
                    {
                        SendToKds(info);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void SendToKds(SendDataInfo info)
        {
            if (info == null) return;
            try
            {
                var tran = _tranApiService.GetTRData(info.Data.SHOP_CODE ?? "", info.Data.POS_NO ?? "", info.Data.SALE_DATE ?? "", info.Data.REGI_SEQ ?? "", info.Data.BILL_NO ?? "");
                if (tran != null)
                {
                    var _productDeviceskds = (from device in _deviceInfos?.Where(t => t.KDS_TYPE == ((char)EKDS_Type.KDS).ToString()).ToList()
                                              join product in _productDevices on device.KDS_NO equals product.KDS_NO
                                              select product).ToList();

                    if (tran.TranProduct != null && tran.TranProduct.Any())
                    {
                        var kdsSends = new Dictionary<string, KDSDataSendInfo>();
                        var list = (from product in tran.TranProduct
                                    join device in _productDeviceskds on product.PRD_CODE equals device.PRD_CODE
                                    into pd
                                    from sybDev in pd.DefaultIfEmpty()
                                    select new { Product = product, KDS_NO = sybDev?.KDS_NO }).ToList();
                        var dev_Prod = list.GroupBy(x => x.KDS_NO).Select(group => new { KDS_NO = group.Key, Products = group.Select(s => s.Product).ToList() }).ToList();

                        foreach (var item in dev_Prod)
                        {
                            MST_INFO_KDS? device = null;

                            if (string.IsNullOrEmpty(item.KDS_NO))
                            {
                                switch (DataLocals.eKDSSendType)
                                {
                                    case EKDSSendNoConfig.AllDevice:
                                        var devices = _deviceInfos.Where(t => t.KDS_TYPE == ((char)EKDS_Type.KDS).ToString()).ToList();
                                        foreach (var s in devices)
                                        {
                                            KDSDataSendInfo? sendInfo;
                                            if (kdsSends.TryGetValue(s.KDS_NO ?? "", out sendInfo))
                                            {
                                                sendInfo.AddProducts(item.Products, _productInfo);
                                            }
                                            else
                                            {
                                                sendInfo = new KDSDataSendInfo()
                                                {
                                                    DeviceInfo = s,
                                                };
                                                sendInfo.AddProducts(item.Products, _productInfo);
                                                kdsSends.Add(s.KDS_NO ?? "", sendInfo);
                                            }
                                        }
                                        break;
                                    case EKDSSendNoConfig.NoSend:

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                device = _deviceInfos.FirstOrDefault(t => t.KDS_NO == item.KDS_NO);
                                if (device != null)
                                {
                                    KDSDataSendInfo? sendInfo;
                                    if (kdsSends.TryGetValue(device.KDS_NO ?? "", out sendInfo))
                                    {
                                        sendInfo.AddProducts(item.Products, _productInfo);
                                    }
                                    else
                                    {
                                        sendInfo = new KDSDataSendInfo()
                                        {
                                            DeviceInfo = device,
                                        };
                                        sendInfo.AddProducts(item.Products, _productInfo);
                                        kdsSends.Add(device.KDS_NO ?? "", sendInfo);
                                    }
                                }
                                else
                                {
                                    ValidateReTrySendKds(info, $"Can not find device KDS.");
                                    _log.Warn($"Can not find device KDS.");
                                }
                            }
                        }
                        if (kdsSends.Any())
                        {
                            kdsSends.ForAll(s =>
                            {
                                switch (char.Parse(s.Value.DeviceInfo.SEND_TYPE ?? ""))
                                {
                                    case (char)EKDS_SenbdType.Socket:
                                        SendToKds_TCP(info, s.Value.DeviceInfo, tran, s.Value.Products);
                                        break;
                                    case (char)EKDS_SenbdType.Http:
                                        SendToKds_HTTP(info, s.Value.DeviceInfo, tran, s.Value.Products);
                                        break;
                                }
                            });
                        }

                    }
                    else
                    {
                        ValidateReTrySendKds(info, $"Tran Product is empty.");
                        _log.Warn($"Tran Product is empty.");
                    }
                }
                else
                {
                    ValidateReTrySendKds(info, $"data Tran is empty.");
                    _log.Warn($"data Tran is empty.");
                }
            }
            catch (Exception ex)
            {
                ValidateReTrySendKds(info, ex.Message);
                _log.Error(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void SendToKds_TCP(SendDataInfo info, MST_INFO_KDS device, TranData tran, IList<TRN_PRDT> prods)
        {
            if (info == null) return;
            try
            {
                System.Net.Sockets.TcpClient tcpClient = new();
                tcpClient.Connect(IPAddress.Parse(device.KDS_TCP_IP ?? ""), int.Parse(device.KDS_TCP_PORT ?? "0"));
                var stream = tcpClient.GetStream();
                var writer = new StreamWriter(stream);
                string? sendTxt = KdsDataFormat(info, tran, prods);
                if (tcpClient.Connected)
                {
                    if (sendTxt != "")
                    {
                        writer.WriteLine(sendTxt);
                        writer.Flush();
                        UpdateTranStatus(info, new ApiResponse() { status = ResultCode.Success });
                    }
                }
                else
                {
                    ValidateReTrySendKds(info, $"Can not connect ip {device.KDS_TCP_IP}:{device.KDS_TCP_PORT} via tcp/ip.");
                }
                tcpClient.Dispose();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                ValidateReTrySendKds(info, $"{ex.Message} => {device.KDS_TCP_IP}:{device.KDS_TCP_PORT} via tcp/ip.");
                _log.Error(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void SendToKds_HTTP(SendDataInfo info, MST_INFO_KDS device, TranData tran, IList<TRN_PRDT> prods)
        {
            if (info == null) return;
            try
            {
                string? sendTxt = KdsDataFormat(info, tran, prods);
                _log.Info($"Not support.");
                UpdateTranStatus(info, new ApiResponse() { status = ResultCode.Success });
            }
            catch (Exception ex)
            {
                if (info.SendCount < PosOption.KDSErrorCount)
                {
                    info.Caculater();
                    AddKds(info);
                }
                else
                {
                    _log.Error($"Send Kds error => {JsonHelper.ModelToJson(info)}");
                }
                _log.Error(ex.Message);
            }
        }
        private string KdsDataFormat(SendDataInfo info, TranData tran, IList<TRN_PRDT> prods)
        {
            dynamic dic = new ExpandoObject();
            dic.TRN_HEADER = new
            {
                SHOP_CODE = tran.TranHeader.SHOP_CODE ?? "",
                SALE_DATE = tran.TranHeader.SALE_DATE ?? "",
                POS_NO = tran.TranHeader.POS_NO ?? "",
                BILL_NO = tran.TranHeader.BILL_NO ?? "",
                ORDER_NO = tran.TranHeader.ORDER_NO ?? "",
                EXCHANGE_NO = tran.TranHeader.POS_NO.Substring(1) + tran.TranHeader.BILL_NO.Substring(1),
                DLV_ORDER_FLAG = tran.TranHeader.DLV_ORDER_FLAG ?? "",
                EMP_NO = tran.TranHeader.EMP_NO ?? "",
                FD_TBL_CODE = "000",
                INSERT_DT = tran.TranHeader.INSERT_DT ?? "",
            };
            dic.TRN_PRDT = prods.OrderBy(t => t.SEQ_NO).Select(t => new
            {
                SHOP_CODE = t.SHOP_CODE ?? "",
                SALE_DATE = t.SALE_DATE ?? "",
                POS_NO = t.POS_NO ?? "",
                BILL_NO = t.BILL_NO ?? "",
                SEQ_NO = t.SEQ_NO ?? "",
                PRD_CODE = t.PRD_CODE ?? "",
                PRD_NAME = t.PRD_NAME ?? "",
                PRD_TYPE_FLAG = t.PRD_TYPE_FLAG ?? "",
                CORNER_CODE = t.CORNER_CODE ?? "",
                SALE_QTY = Convert.ToString(t.SALE_QTY),
                SALE_UPRC = Convert.ToString(t.SALE_UPRC),
                DLV_PACK_FLAG = t.DLV_PACK_FLAG ?? "",
                SDS_PARENT_CODE = t.SDS_PARENT_CODE ?? "",
                INSERT_DT = t.INSERT_DT ?? "",
                COMPLETE_DT = ""
            });
            return JsonHelper.InfoToJson(dic);
        }
        private void ValidateReTrySendKds(SendDataInfo info, string mes)
        {
            if (info.SendCount < PosOption.KDSErrorCount)
            {
                info.Caculater();
                AddKds(info);
            }
            else
            {
                _log.Error($"Send Kds error => {JsonHelper.ModelToJson(info)}");
                UpdateTranStatus(info, new ApiResponse() { status = ResultCode.Fail, results = $"Send Kds error: Count={info.SendCount}, LastSend = {info.LastSend},Mes= {mes}" });
            }
        }
        #endregion

        #region Synchronized Print

        private SendDataInfo PrintPick()
        {
            lock (_lockPrt)
            {
                return _postPrt.Dequeue();
            }
        }
        private void OnProcessingCyclePrt()
        {
            if (_backgroundWorkerPrt.IsBusy)
                _backgroundWorkerPrt.CancelAsync();
            else
                _backgroundWorkerPrt.RunWorkerAsync();
        }
        private void BackgroundWorker_DoWorkPrt(object? sender, DoWorkEventArgs e)
        {
            try
            {
                while (_postPrt.Any())
                {
                    var info = PrintPick();
                    if (info != null)
                    {
                        SendToPrint(info);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }
        private void SendToPrint(SendDataInfo info)
        {
            if (info == null) return;
            try
            {
                var order = new OrderInfo()
                {
                    ShopCode = info.Data.SHOP_CODE,
                    BillNo = info.Data.BILL_NO,
                    PosNo = info.Data.POS_NO,
                    SaleDate = info.Data.SALE_DATE
                };

                var result = _pOSPrintService.PrintKitchenOrder(order);
                if (!result.Success)
                {
                    UpdateTranStatus(info, new ApiResponse() { status = ResultCode.Fail, results = "Send to print error." });
                    LogHelper.Logger.Error(result.Message);
                }
                else
                {
                    UpdateTranStatus(info, new ApiResponse() { status = ResultCode.Success });
                    LogHelper.Logger.Info($"Print kitchen success.");
                }
            }
            catch (Exception ex)
            {
                _tranApiService.UpdateTranPOSTMangStatus(info.Data, new ApiResponse() { status = ResultCode.Fail, results = ex.Message });
                if (info.SendCount < PosOption.PrtErrorCount)
                {
                    info.Caculater();
                    AddPrint(info);
                }
                else
                {
                    _log.Error($"Send Print error => {JsonHelper.ModelToJson(info)}");
                }
                _log.Error(ex.Message);
            }
        }
        private string PrintDataFormat(SendDataInfo info)
        {
            return "";
        }

        #endregion

        #endregion

        //**--------------------------------------------------------------------------------
    }
}
