using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Content;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.API;
using GoShared.Helpers;
using GoShared.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GoPOS.Service.Common
{
    public class ApiRequest
    {
        //**---------------------------------------------------

        #region Constructor
        public ApiRequest(Uri serviceRoot, string shopCode, string posNo, string licenseId, string licenseKey, int? language, int numberOfAttempts = 3)
        {
            //if (string.IsNullOrWhiteSpace(shopCode))
            //    throw new ArgumentException("shopCode can not be empty.");
            //if (string.IsNullOrWhiteSpace(licenseId))
            //    throw new ArgumentException("LicenseId can not be empty.");
            //if (string.IsNullOrWhiteSpace(licenseKey))
            //    throw new ArgumentException("LicenseKey can not be empty.");
            ServiceRoot = serviceRoot;
            ShopCode = shopCode;
            PosNo = posNo;
            LicenseId = licenseId;
            LicenseKey = licenseKey;
            Language = language;
            NumberOfAttempts = numberOfAttempts;
        }

        public ApiRequest(string serviceRoot) : this(new Uri(serviceRoot), "", "", "", "", 0)
        {

        }
        public ApiRequest(string serviceRoot, string shopCode, string posNo, string licenseId, string licenseKey, int numberOfAttempts = 3)
        : this(new Uri(serviceRoot), shopCode, posNo, licenseId, licenseKey, 0, numberOfAttempts)
        {
        }
        public ApiRequest(string serviceRoot, string shopCode, string license)
            : this(new Uri(serviceRoot), shopCode, string.Empty, license, "", 3)
        {
        }
        public ApiRequest(ApiRequest apiRequest, FlurlRequest flurlRequest)
        {
            FlurlRequest = flurlRequest;
            CopyApiRequest(apiRequest);
        }
        private void CopyApiRequest(ApiRequest apiRequest)
        {
            ServiceRoot = apiRequest.ServiceRoot;
            ShopCode = apiRequest.ShopCode;
            PosNo = apiRequest.PosNo;
            LicenseId = apiRequest.LicenseId;
            LicenseKey = apiRequest.LicenseKey;
            Language = apiRequest.Language;
            NumberOfAttempts = apiRequest.NumberOfAttempts;
            Token = apiRequest.Token;
            Expire_dt = apiRequest.Expire_dt;
            LoginResponse = apiRequest.LoginResponse;
            Message = "";
            IsLogined = apiRequest.IsLogined;
            LastRequest = apiRequest.LastRequest;
        }
        private void SetFlurlRequest(IFlurlRequest flurlRequest)
        {
            FlurlRequest = flurlRequest;
        }

        #endregion

        //**---------------------------------------------------

        #region Fields
        internal IFlurlRequest FlurlRequest { get; private set; }
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private DateTime _lastRequest = DateTime.MinValue;
        private TokenResult _loginResponse = new TokenResult();
        private readonly HttpStatusCode[] _returnCodesToRetry = new[]
        {
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };
        #endregion


        //**---------------------------------------------------

        #region Properties

        /// <summary>
        /// Gets the Service root URI.
        /// </summary>
        public Uri ServiceRoot { get; private set; }

        /// <summary>
        /// Gets the Shop code to be used for the Service authentication.
        /// </summary>
        public string ShopCode { get; private set; }
        public string PosNo { get; private set; }
        public string LicenseId { get; private set; }

        /// <summary>
        /// Gets the License for the provided.
        /// </summary>
        public string LicenseKey { get; private set; }
        /// <summary>
        /// Gets the Service Layer language code provided.
        /// </summary>
        public int? Language { get; private set; }

        /// <summary>
        /// Gets authentication token
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// Gets token validity time
        /// </summary>
        public string Expire_dt { get; private set; }
        /// <summary>
        /// Gets or sets the number of attempts for each unsuccessfull request in case of an HTTP response code of 401, 500, 502, 503 or 504.
        /// </summary>
        public int NumberOfAttempts { get; set; }
        /// <summary>
        /// Gets information about the latest Login request.
        /// </summary>
        public TokenResult LoginResponse
        {
            get
            {
                // Returns a new object so the login control can't be manipulated externally
                return new TokenResult()
                {
                    ExpiryDt = _loginResponse.ExpiryDt,
                    AccessToken = _loginResponse.AccessToken
                };
            }
            private set
            {
                _loginResponse = value;
            }
        }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; private set; }
        public DateTime LastRequest
        {
            get
            {
                return _lastRequest;
            }
            private set
            {
                _lastRequest = value;
            }
        }
        public DateTime LoginTime { get; private set; } = DateTime.MinValue;
        #endregion

        //**---------------------------------------------------

        #region Public
        public void Set(Uri serviceRoot, string shopCode, string password, int? language, int numberOfAttempts = 3)
        {
            if (string.IsNullOrWhiteSpace(shopCode))
                throw new ArgumentException("shopCode can not be empty.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password can not be empty.");
            ServiceRoot = serviceRoot;
            ShopCode = shopCode;
            //Password = password;
            Language = language;
            NumberOfAttempts = numberOfAttempts;
        }

        #endregion


        //**---------------------------------------------------

        #region Private Method

        private object _lockTokenUpdate = new object();

        private void SetToken(TokenResult tokenResult, bool isUpdate = true)
        {
            lock (_lockTokenUpdate)
            {
                _loginResponse.Set(tokenResult);
                Expire_dt = tokenResult.ExpiryDt;
                Token = tokenResult.AccessToken;
                LoginTime = DateTime.Now;
                IsLogined = true;
                if (isUpdate)
                {
                    using (var context = new DataContext())
                    {
                        DbContextTransaction trans = null;
                        try
                        {
                            trans = context.Database.BeginTransaction();
                            var token = context.pOS_KEY_MANGs.FirstOrDefault(t => t.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE &&
                            t.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo
                            && t.POS_NO == DataLocals.AppConfig.PosInfo.PosNo);
                            if (token != null)
                            {
                                //context.pOS_KEY_MANGs.Attach(token);
                                token.VALIDDT = tokenResult.ExpiryDt;
                                token.TOKEN = tokenResult.AccessToken;
                                context.pOS_KEY_MANGs.AddOrUpdate(token);
                                context.SaveChanges();
                                DataLocals.TokenInfo = token;
                            }
                            else
                            {
                                token = new POS_KEY_MANG();
                                token.VALIDDT = tokenResult.ExpiryDt;
                                token.TOKEN = tokenResult.AccessToken;
                                token.HD_SHOP_CODE = DataLocals.AppConfig.PosInfo.HD_SHOP_CODE;
                                token.SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo;
                                token.POS_NO = DataLocals.AppConfig.PosInfo.PosNo;
                                token.LICENSE_KEY = LicenseKey;
                                token.LICENSE_ID = LicenseId;

                                context.pOS_KEY_MANGs.Add(token);
                                context.SaveChanges();

                                DataLocals.TokenInfo = token;
                            }

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (trans != null)
                                trans.Rollback();
                            LogHelper.Logger.Error(ex.ToFormattedString());
                        }
                    }
                }
            }
        }

        public bool UpdateToken()
        {
            using (var context = new DataContext())
            {
                DbContextTransaction trans = null;
                try
                {
                    return true;

                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    LogHelper.Logger.Error(ex.ToFormattedString());
                    return false;
                }
            }

            return false;
        }
        private void EntityToToken(POS_KEY_MANG entity)
        {
            TokenResult token = new TokenResult();
            token.AccessToken = entity.TOKEN;
            token.ExpiryDt = entity.VALIDDT;
            SetToken(token, false);
        }
        private bool ValidateLogin()
        {
            if (IsLogined)
            {
                if (DateTime.Now < DataLocals.TokenInfo.ExpiredDate)
                {
                    return true;
                }
            }
            else
            {
                if (DataLocals.TokenInfo != null)
                {
                    if (DateTime.Now < DataLocals.TokenInfo.ExpiredDate)
                    {
                        EntityToToken(DataLocals.TokenInfo);
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        //**---------------------------------------------------

        #region Authentication Methods
        /// <summary>
        /// If the current session is expired or non-existent, performs a POST Login request with the provided information.
        /// Manually performing the Login is often unnecessary because it will be performed automatically anyway whenever needed.
        /// </summary>
        /// <param name="forceLogin">
        /// Whether the login request should be forced even if the current session has not expired.
        /// </param>
        public async Task<ApiRequest> LoginAsync(bool forceLogin = false)
        {
            return await ExecuteLoginAsync(forceLogin, true);
        }

        /// <summary>
        /// If the current session is expired or non-existent, performs a POST Login request with the provided information.
        /// Manually performing the Login is often unnecessary because it will be performed automatically anyway whenever needed.
        /// </summary>
        /// <param name="forceLogin">
        /// Whether the login request should be forced even if the current session has not expired.
        /// </param>
        public async void ExLoginAsync(bool forceLogin = false)
        {
            await ExecuteLoginAsync(forceLogin, true);
        }

        /// <summary>
        /// Internal login method where a return is not needed.
        /// </summary>
        private async Task LoginInternalAsync(bool forceLogin = false)
        {
            await ExecuteLoginAsync(forceLogin, false);
        }
        public bool IsLogined { get; set; } = false;
        /// <summary>
        /// Performs the POST Login request to the Service Layer.
        /// </summary>
        /// <param name="forceLogin">
        /// Whether the login request should be forced even if the current session has not expired.
        /// </param>
        /// <param name="expectReturn">
        /// Wheter the login information should be returned.
        /// </param>
        private async Task<ApiRequest> ExecuteLoginAsync(bool forceLogin = false, bool expectReturn = false)
        {
            // Prevents multiple login requests in a multi-threaded scenario
            await _semaphoreSlim.WaitAsync();

            try
            {
                IsLogined = false;
                if (forceLogin)
                    _lastRequest = default;

                ////Session still valid, no need to login again
                if (DataLocals.TokenInfo != null)
                {
                    if (DateTime.Now < DataLocals.TokenInfo.ExpiredDate)
                    {
                        EntityToToken(DataLocals.TokenInfo);
                        return expectReturn ? this : null;
                    }
                }


                var loginResponse = await ServiceRoot
                    .AppendPathSegment($"client/auth/token")
                    .SetQueryParam("licenseId", LicenseId)
                    .SetQueryParam("licenseKey", LicenseKey)
                    .GetAsync()
                    .ReceiveJson<ApiResult>();

                if (loginResponse != null)
                {
                    if (loginResponse.status == ResultCode.Success)
                    {
                        var tokens = JsonHelper.JsonToModel<TokenResult>(Convert.ToString(loginResponse.results));
                        if (tokens.result == ResultCode.Ok)
                        {
                            SetToken(tokens.model, true);
                        }
                        else
                        {
                            IsLogined = false;
                            Message = tokens.result;
                        }
                    }
                    else
                    {
                        IsLogined = false;
                        Message = loginResponse.ResultMsg;
                    }
                }
                else
                    return this;

                return expectReturn ? this : null;
            }
            catch (FlurlHttpException ex)
            {
                try
                {
                    if (ex.Call.HttpResponseMessage == null) throw;
                    //var response = await ex.GetResponseJsonAsync<ResponseError>();
                    throw new SLException(ex.Message, ex);
                }
                catch { throw ex; }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <summary>
        /// Performs a POST Logout request, ending the current session.
        /// </summary>
        public async Task LogoutAsync()
        {

        }
        #endregion

        //**---------------------------------------------------

        #region Request Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="SLRequest"/> class that represents a request to the associated <see cref="SLConnection"/>. 
        /// </summary>
        /// <remarks>
        /// The request can be configured using the extension methods provided in <see cref="SLRequestExtensions"/>.
        /// </remarks>
        /// <param name="resource">
        /// The resource name to be requested.
        /// </param>
        public ApiRequest Request(string resource)
        {
            var flurlRequest = new FlurlRequest(ServiceRoot.AppendPathSegment(resource));
            SetFlurlRequest(flurlRequest);
            return new ApiRequest(this, flurlRequest);
        }
        public ApiRequest Request(string resource, object id, string method)
        {
            var flurlRequest = new FlurlRequest(ServiceRoot.AppendPathSegment(id is string ? $"{resource}('{id}')/{method}" : $"{resource}({id})/{method}"));
            SetFlurlRequest(flurlRequest);
            return new ApiRequest(this, flurlRequest);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SLRequest"/> class that represents a request to the associated <see cref="SLConnection"/>. 
        /// </summary>
        /// <remarks>
        /// The request can be configured using the extension methods provided in <see cref="SLRequestExtensions"/>.
        /// </remarks>
        /// <param name="resource">
        /// The resource name to be requested.
        /// </param>
        /// <param name="id">
        /// The entity ID to be requested.
        /// </param>

        /// <summary>
        /// Initializes a new instance of the <see cref="SLRequest"/> class that represents a request to the associated <see cref="SLConnection"/>. 
        /// </summary>
        /// <remarks>
        /// The request can be configured using the extension methods provided in <see cref="SLRequestExtensions"/>.
        /// </remarks>
        /// <param name="resource">
        /// The resource name to be requested.
        /// </param>
        /// <param name="id">
        /// The entity ID to be requested.
        /// </param>
        public ApiRequest Request(string resource, object id)
        {
            var flurlRequest = new FlurlRequest(ServiceRoot.AppendPathSegment(id is string ? $"{resource}('{id}')" : $"{resource}({id})"));
            SetFlurlRequest(flurlRequest);
            return new ApiRequest(this, flurlRequest);
        }

        /// <summary>
        /// Calls the Login method to ensure a valid session and then executes the provided request.
        /// If the request is unsuccessfull with any return code present in <see cref="_returnCodesToRetry"/>, 
        /// it will be retried for <see cref="NumberOfAttempts"/> number of times.
        /// </summary>
        internal async Task<T> ExecuteRequest<T>(Func<Task<T>> action)
        {
            _lastRequest = DateTime.Now;

            if (NumberOfAttempts < 1)
                throw new ArgumentException("The number of attempts can not be lower than 1.");

            int retryCount = NumberOfAttempts;
            var exceptions = new List<Exception>();

            if (!ValidateLogin())
            {
                await LoginInternalAsync();
            }

            while (true)
            {
                try
                {
                    var result = await action();
                    return result;
                }
                catch (FlurlHttpException ex)
                {
                    try
                    {
                        if (ex.Call.HttpResponseMessage == null)
                            throw;

                        //var response = await ex.GetResponseJsonAsync<ResponseError>();
                        exceptions.Add(new SLException(ex.Message, ex));
                    }
                    catch
                    {
                        exceptions.Add(ex);
                    }

                    // Whether the request should be retried
                    if (!_returnCodesToRetry.Any(x => x == ex.Call.HttpResponseMessage?.StatusCode))
                    {
                        break;
                    }

                    // Forces a new login request in case the response is 401 Unauthorized
                    if (ex.Call.HttpResponseMessage?.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await LoginInternalAsync(true);
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                retryCount--;

                if (retryCount <= 0)
                {
                    break;
                }

                await Task.Delay(200);
            }

            var uniqueExceptions = exceptions.Distinct(new ExceptionEqualityComparer());

            if (uniqueExceptions.Count() == 1)
                throw uniqueExceptions.First();

            throw new AggregateException("Could not process request", uniqueExceptions);
        }

        #endregion

        //**---------------------------------------------------

        #region Get

        private void SetHead_Get()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token)
                .SetQueryParam("posNo", PosNo);
        }
        private void SetHead_GetMoPara()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token);
        }
        private void SetHead_Post()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token)
                .WithHeader("Content-Type", "application/json");

        }
        private void SetHead_Put()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token)
                .WithHeader("Content-Type", "application/json");
        }
        private void SetHead_Patch()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token)
                .WithHeader("Content-Type", "application/json");
        }

        private void SetHead_Delete()
        {
            FlurlRequest
                .WithHeader("GP-AUTH-ID", LicenseId)
                .WithHeader("GP-AUTH-TOKEN", Token);
        }
        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a new instance of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The object type for the result to be deserialized into.
        /// </typeparam>
        /// <param name="unwrapCollection">
        /// Whether the result should be unwrapped from the 'value' JSON array in case it is a collection.
        /// </param>
        public async Task<T> GetAsync<T>(bool unwrapCollection = true) where T : class
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();

                string stringResult = await FlurlRequest.GetStringAsync();
                var jObject = JObject.Parse(stringResult);
                try
                {
                    if (unwrapCollection)
                    {
                        // Checks if the result is a collection by selecting the "value" token
                        var valueCollection = jObject.SelectToken("results");
                        return valueCollection == null ? jObject.ToObject<T>() : valueCollection.ToObject<T>();
                    }
                    else
                    {
                        return jObject.ToObject<T>();
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }

            });
        }

        /// <summary>
        /// Sends an asynchronous POST request.
        /// </summary>
        /// <param name="request">This IFlurlRequest</param>
        /// <param name="data">An object representing the request body, which will be serialized to JSON.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="completionOption">The HttpCompletionOption used in the request. Optional.</param>
        /// <returns>A Task whose result is the received IFlurlResponse.</returns>
        public Task<IFlurlResponse> GetStringAsync(object data, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var content = new CapturedJsonContent(FlurlRequest.Settings.JsonSerializer.Serialize(data));
            return FlurlRequest.SendAsync(HttpMethod.Get, content: content, cancellationToken: cancellationToken, completionOption: completionOption);
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a new instance of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The object type for the result to be deserialized into.
        /// </typeparam>
        /// <param name="unwrapCollection">
        /// Whether the result should be unwrapped from the 'value' JSON array in case it is a collection.
        /// </param>
        public async Task<ApiResult> GetDatasAsync<T>(IDictionary<string, string> paras = null) where T : class
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                string jsonBody = string.Empty;
                if (paras != null && paras.Any())
                {
                    foreach (var item in paras)
                    {
                        if (item.Key.StartsWith("B_"))
                        {
                            jsonBody = item.Value;
                            continue;
                        }
                        FlurlRequest.SetQueryParam(item.Key, item.Value);
                    }
                }

                ApiResult result = new ApiResult();
                var bodyData = new CapturedJsonContent(jsonBody).Content;

                //string stringResult = await FlurlRequest.GetStringAsync();
                string stringResult = await FlurlRequest.SendStringAsync(HttpMethod.Get, bodyData).ReceiveString();

                var jObject = JObject.Parse(stringResult);
                try
                {
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken("results");
                    result.results = string.Empty + valueCollection;
                    result.ResultCode = ResultCode.Success;
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }

            });
        }
        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a new instance of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The object type for the result to be deserialized into.
        /// </typeparam>
        /// <param name="unwrapCollection">
        /// Whether the result should be unwrapped from the 'value' JSON array in case it is a collection.
        /// </param>
        public async Task<ApiResult> GetDatasAsync<T>(IDictionary<string, string> paras = null, object body = null) where T : class
        {
            if (body == null)
            {
                return await GetDatasAsync<T>(paras);
            }
            else
            {
                return await ExecuteRequest(async () =>
                {
                    SetHead_Get();
                    if (paras != null && paras.Any())
                    {
                        foreach (var item in paras)
                        {
                            FlurlRequest.SetQueryParam(item.Key, item.Value);
                        }
                    }
                    var content = new CapturedJsonContent(FlurlRequest.Settings.JsonSerializer.Serialize(body));
                    ApiResult result = new ApiResult();
                    string stringResult = await FlurlRequest.SendAsync(HttpMethod.Get, content: content, cancellationToken: default(CancellationToken), completionOption: HttpCompletionOption.ResponseContentRead).ReceiveString();
                    var jObject = JObject.Parse(stringResult);
                    try
                    {
                        // Checks if the result is a collection by selecting the "value" token
                        var valueCollection = jObject.SelectToken("results");
                        result.results = string.Empty + valueCollection;
                        result.ResultCode = ResultCode.Success;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                });
            }



        }
        public async Task<ApiResult> GetDatasAsync(IDictionary<string, string> paras = null, object body = null)
        {
            if (body == null)
            {
                return await GetDatasAsync(paras);
            }
            else
            {
                return await ExecuteRequest(async () =>
                {
                    SetHead_Get();
                    if (paras != null && paras.Any())
                    {
                        foreach (var item in paras)
                        {
                            FlurlRequest.SetQueryParam(item.Key, item.Value);
                        }
                    }
                    var content = new CapturedJsonContent(FlurlRequest.Settings.JsonSerializer.Serialize(body));
                    ApiResult result = new ApiResult();
                    string stringResult = await FlurlRequest.SendAsync(HttpMethod.Get, content: content, cancellationToken: default(CancellationToken), completionOption: HttpCompletionOption.ResponseContentRead).ReceiveString();
                    var jObject = JObject.Parse(stringResult);
                    try
                    {
                        // Checks if the result is a collection by selecting the "value" token
                        var valueCollection = jObject.SelectToken("results");
                        result.results = string.Empty + valueCollection;
                        result.ResultCode = ResultCode.Success;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                });
            }



        }
        public async Task<ApiResult> GetDatasBodyAsync(IDictionary<string, string> paras = null, object body = null)
        {
            if (body == null)
            {
                return await GetDatasAsync(paras);
            }
            else
            {
                return await ExecuteRequest(async () =>
                {
                    try
                    {
                        var json = JsonHelper.ModelToJson(body);
                        var serviceRoot = new Uri(FlurlRequest.Url);

                        if (paras != null && paras.Any())
                        {
                            foreach (var item in paras)
                            {
                                serviceRoot.SetQueryParam(item.Key, item.Value);
                            }
                        }
                        var result = await serviceRoot
                             .WithHeader("GP-AUTH-ID", LicenseId)
                         .WithHeader("GP-AUTH-TOKEN", Token)
                         .WithHeader("Content-Type", "application/json")
                        .SendStringAsync(HttpMethod.Get, json, cancellationToken: default(CancellationToken), completionOption: HttpCompletionOption.ResponseContentRead)
                        .ReceiveJson<ApiResult>();
                        return result;
                        //   var jObject = JObject.Parse(stringResult);

                        // Checks if the result is a collection by selecting the "value" token
                        //var valueCollection = jObject.SelectToken("results");
                        //result.results = string.Empty + valueCollection;
                        //result.ResultCode = ResultCode.Success;

                    }
                    catch (Exception ex)
                    {
                        return new ApiResult() { status = ResultCode.Fail, ResultMsg = ex.InnerException == null ? ex.Message : ex.InnerException.Message };
                    }

                });
            }



        }


        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a dynamic object.
        /// </summary>
        public async Task<dynamic> GetAsync()
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                return await FlurlRequest.GetJsonAsync();
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a <see cref="string"/>.
        /// </summary>
        public async Task<string> GetStringAsync()
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                return await FlurlRequest.GetStringAsync();
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in an instance of the given anonymous type.
        /// </summary>
        /// <param name="anonymousTypeObject">
        /// The anonymous type object.
        /// </param>
        /// <param name="jsonSerializerSettings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is null, 
        /// default serialization settings will be used.
        /// </param>
        public async Task<T> GetAnonymousTypeAsync<T>(T anonymousTypeObject, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                string stringResult = await FlurlRequest.GetStringAsync();
                return JsonConvert.DeserializeAnonymousType(stringResult, anonymousTypeObject, jsonSerializerSettings);
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a <see cref="byte"/> array.
        /// </summary>
        public async Task<byte[]> GetBytesAsync()
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                return await FlurlRequest.GetBytesAsync();
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a <see cref="Stream"/>.
        /// </summary>
        public async Task<Stream> GetStreamAsync()
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                return await FlurlRequest.GetStreamAsync();
            });
        }

        /// <summary>
        /// Performs a GET request that returns the count of an entity collection.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                string result = await FlurlRequest.AppendPathSegment("$count").GetStringAsync();
                long.TryParse(result, out long quantity);
                return quantity;
            });
        }
        /// <summary>
        /// Performs a GET request that returns the count of an entity collection.
        /// </summary>
        public async Task<long> GetAsync(object body)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Get();
                string result = await FlurlRequest.AppendPathSegment("$count").GetStringAsync();
                long.TryParse(result, out long quantity);
                return quantity;
            });
        }

        public async Task<HttpResponseMessage> GetCall(object requestData, string apiUrl, bool licenseCheck = false, AuthenRequestHeader header = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (licenseCheck == true && header != null)
                    {
                        client.DefaultRequestHeaders.Add("GP-AUTH-TOKEN", header.Token);
                    }
                    // Create an HTTP request message with a GET method
                    var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                    // Create an anonymous object to represent the data you want to send in the request body

                    // Serialize the data to JSON and set it as the content of the request
                    string jsonContent = JsonConvert.SerializeObject(requestData);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send the request and get the response
                    var response = await client.SendAsync(request);

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Performs a POST request with the provided parameters and returns the result in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="data">
        /// The object to be sent as the JSON body.
        /// </param>
        /// <typeparam name="T">
        /// The object type for the result to be deserialized into.
        /// </typeparam>
        /// <param name="unwrapCollection">
        /// Whether the result should be unwrapped from the 'value' JSON array in case it is a collection.
        /// </param>
        public async Task<T> PostAsync<T>(object data, string innerKey)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                string stringResult = await FlurlRequest.PostJsonAsync(data).ReceiveString();
                var jObject = JObject.Parse(stringResult);

                if (!string.IsNullOrEmpty(innerKey))
                {
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken(innerKey);
                    return valueCollection == null ? jObject.ToObject<T>() : valueCollection.ToObject<T>();
                }
                else
                {
                    return jObject.ToObject<T>();
                }
            });
        }

        public async Task<string> Post_Async<T>(object data, bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                string stringResult = await FlurlRequest.PostJsonAsync(data).ReceiveString();
                return stringResult;
            });
        }

        public async Task<ApiResult> PostBodyAsync(object data, bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                var result = new ApiResult() { ResultCode = ResultCode.Fail };
                var json = JsonHelper.Info2Json(data);
                try
                {
                    string stringResult = await FlurlRequest.PostStringAsync(json).ReceiveString();
                    var jObject = JObject.Parse(stringResult);
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken("results");
                    result.results = string.Empty + valueCollection;
                    result.status = string.Empty + jObject.SelectToken("status");
                    result.ResultCode = ResultCode.Success;
                    return result;
                }
                catch (Exception ex)
                {
                    result.ResultMsg = ex.Message;
                    return result;
                }

                return result;
            });
        }

        public async Task<ApiResult> PostBody(object data, bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                var result = new ApiResult() { ResultCode = ResultCode.Fail };
                var json = JsonHelper.Info2Json(data);
                try
                {
                    string stringResult = await FlurlRequest.PostStringAsync(json).ReceiveString();
                    var jObject = JObject.Parse(stringResult);
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken("results");
                    result.results = string.Empty + valueCollection;
                    result.status = string.Empty + jObject.SelectToken("status");
                    result.ResultCode = ResultCode.Success;
                    return result;
                }
                catch (Exception ex)
                {
                    result.ResultMsg = ex.Message;
                    return result;
                }

                return result;
            });
        }

        public async Task<string> SPost_Async<T>(string data, bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                string stringResult = await FlurlRequest.PostStringAsync(data).ReceiveString();
                return stringResult;
            });
        }
        public async Task<T> PostStringAsync<T>(string data, bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                string stringResult = await FlurlRequest.PostStringAsync(data).ReceiveString();
                var jObject = JObject.Parse(stringResult);

                if (unwrapCollection)
                {
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken("value");
                    return valueCollection == null ? jObject.ToObject<T>() : valueCollection.ToObject<T>();
                }
                else
                {
                    return jObject.ToObject<T>();
                }
            });
        }

        /// <summary>
        /// Performs a POST request with the provided parameters and returns the result in the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The object type for the result to be deserialized into.
        /// </typeparam>
        /// <param name="unwrapCollection">
        /// Whether the result should be unwrapped from the 'value' JSON array in case it is a collection.
        /// </param>
        public async Task<T> PostAsync<T>(bool unwrapCollection = true)
        {
            return await ExecuteRequest(async () =>
            {
                SetHead_Post();
                string stringResult = await FlurlRequest.PostAsync().ReceiveString();
                var jObject = JObject.Parse(stringResult);

                if (unwrapCollection)
                {
                    // Checks if the result is a collection by selecting the "value" token
                    var valueCollection = jObject.SelectToken("value");
                    return valueCollection == null ? jObject.ToObject<T>() : valueCollection.ToObject<T>();
                }
                else
                {
                    return jObject.ToObject<T>();
                }
            });
        }

        /// <summary>
        /// Performs a POST request with the provided parameters.
        /// </summary>
        /// <param name="data">
        /// The object to be sent as the JSON body.
        /// </param>
        public async Task PostAsync(object data)
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Post();
                return await FlurlRequest.PostJsonAsync(data);
            });
        }

        /// <summary>
        /// Performs a POST request with the provided parameters.
        /// </summary>
        public async Task PostAsync()
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Post();
                return await FlurlRequest.PostAsync();
            });
        }

        /// <summary>
        /// Performs a PATCH request with the provided parameters.
        /// </summary>
        /// <param name="data">
        /// The object to be sent as the JSON body.
        /// </param>
        public async Task PatchAsync(object data)
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Patch();
                return await FlurlRequest.PatchJsonAsync(data);
            });
        }
        /// <summary>
        /// Performs a PATCH request with the provided parameters.
        /// </summary>
        /// <param name="data">
        /// The object to be sent as the JSON body.
        /// </param>
        public async Task SPatchAsync(string data)
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Patch();
                return await FlurlRequest.PatchStringAsync(data);
            });
        }
        /// <summary>
        /// Performs a PATCH request with the provided file.
        /// </summary>
        /// <param name="path">
        /// The path to the file to be sent.
        /// </param>
        public async Task PatchWithFileAsync(string path)
        {
            await PatchWithFileAsync(Path.GetFileName(path), File.ReadAllBytes(path));
        }

        /// <summary>
        /// Performs a PATCH request with the provided file.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the file including the file extension.
        /// </param>
        /// <param name="file">
        /// The file to be sent.
        /// </param>
        public async Task PatchWithFileAsync(string fileName, byte[] file)
        {
            await PatchWithFileAsync(fileName, new MemoryStream(file));
        }
        /// <summary>
        /// Performs a PATCH request with the provided file.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the file including the file extension.
        /// </param>
        /// <param name="file">
        /// The file to be sent.
        /// </param>
        public async Task PatchWithFileAsync(string fileName, Stream file)
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Patch();
                return await FlurlRequest.PatchMultipartAsync(mp =>
                {
                    // Removes double quotes from boundary, otherwise the request fails with error 405 Method Not Allowed
                    var boundary = mp.Headers.ContentType.Parameters.First(o => o.Name.Equals("boundary", StringComparison.OrdinalIgnoreCase));
                    boundary.Value = boundary.Value.Replace("\"", string.Empty);

                    var content = new StreamContent(file);
                    content.Headers.Add("Content-Disposition", $"form-data; name=\"files\"; filename=\"{fileName}\"");
                    content.Headers.Add("Content-Type", "application/octet-stream");
                    mp.Add(content);
                });
            });
        }


        /// <summary>
        /// Performs a PUT request with the provided parameters.
        /// </summary>
        /// <param name="data">
        /// The object to be sent as the JSON body.
        /// </param>
        public async Task PutAsync(object data)
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Put();
                return await FlurlRequest.PutJsonAsync(data);
            });
        }

        /// <summary>
        /// Performs a DELETE request with the provided parameters.
        /// </summary>
        public async Task DeleteAsync()
        {
            await ExecuteRequest(async () =>
            {
                SetHead_Delete();
                return await FlurlRequest.DeleteAsync();
            });
        }
        #endregion

        //**---------------------------------------------------

        #region Private Classes
        /// <summary>
        /// Used to aggregate exceptions that occur on request retries. 
        /// </summary>
        /// <remarks>
        /// In most cases, the same exception will occur multiple times, 
        /// but we don't want to return multiple copies of it. This class is used 
        /// to find exceptions that are duplicates by type and message so we can
        /// only return one of them.
        /// </remarks>
        private class ExceptionEqualityComparer : IEqualityComparer<Exception>
        {
            public bool Equals(Exception e1, Exception e2)
            {
                if (e2 == null && e1 == null)
                    return true;
                else if (e1 == null | e2 == null)
                    return false;
                else if (e1.GetType().Name.Equals(e2.GetType().Name) && e1.Message.Equals(e2.Message))
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Exception e)
            {
                return (e.GetType().Name + e.Message).GetHashCode();
            }
        }

        /// <summary>
        /// Custom HttpClientFactory implementation for Service Layer.
        /// </summary>
        private class CustomHttpClientFactory : DefaultHttpClientFactory
        {
            public override HttpMessageHandler CreateMessageHandler()
            {
                var handler = (HttpClientHandler)base.CreateMessageHandler();
                handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
                return handler;
            }

            public override HttpClient CreateHttpClient(HttpMessageHandler handler)
            {
                var httpClient = base.CreateHttpClient(handler);
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                return httpClient;
            }
        }
        #endregion



        //**---------------------------------------------------
    }

}
