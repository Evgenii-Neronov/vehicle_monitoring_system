using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    internal class ApiClientBase : IServerConnectionStatus
    {
        public string ServerBaseUrl => ConfigurationManager.AppSettings["ReportServerBaseUrl"];
        
        public async Task<ReponseType> PostRequestAsync<RequestType, ReponseType>(string path, RequestType requestModel)
            where RequestType : new() where ReponseType : new()
        {
            OnConnectionStatusChanged?.Invoke(ConnectingStatus.Start);

            var client = new RestClient(ServerBaseUrl);

            var request = new RestRequest(path, Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(requestModel);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);

            OnConnectionStatusChanged?.Invoke(ConnectingStatus.Complete);

            if (response.ErrorException != null)
            {
                OnConnectionStatusChanged?.Invoke(ConnectingStatus.Error);
                throw new ServerException($"Server error: {response.ErrorMessage}", response.ErrorException);
            }

            var responseObject = JsonConvert.DeserializeObject<ReponseType>(response.Content);

            return responseObject;
        }

        public async Task<ReponseType> GetRequestAsync<ReponseType>(string path)
            where ReponseType : new()
        {
            OnConnectionStatusChanged?.Invoke(ConnectingStatus.Start);
            var client = new RestClient(ServerBaseUrl);

            var request = new RestRequest(path, Method.Get);
            var response = await client.ExecuteAsync(request);

            OnConnectionStatusChanged?.Invoke(ConnectingStatus.Complete);

            if (response.ErrorException != null)
            {
                OnConnectionStatusChanged?.Invoke(ConnectingStatus.Error);
                throw new ServerException(response.ErrorMessage, response.ErrorException);
            }


            return JsonConvert.DeserializeObject<ReponseType>(response.Content);
        }

        public event ConnectionStatus? OnConnectionStatusChanged;
    }
}