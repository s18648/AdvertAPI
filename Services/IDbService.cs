using AdvertAPI.DTOs.Requests;
using AdvertAPI.DTOs.Responses;
using AdvertAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertAPI.Services
{
    public interface IDbService
    {
        public Client AddUser(AddUserRequest iur);
        public LoginAttemptResponse checkLogin(string login);
        public void RecordToken(RecordTokenRequest rtr);
        public ValidateTokenResponse ValidateTheToken(string requestToken);
        public List<CampaignResponse> GetListOfAllCampaigns();
        public void SaveLogData(string method, string path, string body, string query);


    }
}
