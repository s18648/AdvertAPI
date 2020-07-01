using AdvertAPI.DTOs.Requests;
using AdvertAPI.DTOs.Responses;
using AdvertAPI.Entities;
using AdvertAPI.Handlers;
using AdvertAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AdvertAPI.Services
{
    public class SqlUserService
    {
        private readonly AdvertContext _advertContext;

        public SqlUserService(AdvertContext advertContext)
        {
            _advertContext = advertContext;
        }

        public Client AddUser(AddUserRequest iur)
        {
            try
            {
                var cl = new Client
                {
                    FirstName = iur.FirstName,
                    LastName = iur.LastName,
                    Email = iur.Email,
                    Phone = iur.Phone,
                    Login = iur.Login,
                    Hash = AuthHandler.Create(iur.Password, AuthHandler.CreateSalt()),
                    TokenString = "xxx"
                };


                _advertContext.SaveChanges();


                return cl;

            }
            catch (SqlException ex)
            {
                return null;
            }
        }

        public LoginAttemptResponse checkLogin(string login)
        {




            LoginAttemptResponse loginAttemptResponse = _advertContext.Client.Where(s => s.Login == login).Select(p => new LoginAttemptResponse
            {
                FirstName = p.FirstName,
                Hash = p.Hash,
                IdClient = p.IdClient,
                Login = p.Login,
                Salt = p.Salt,
                TokenString = p.TokenString
            }).FirstOrDefault();


            return loginAttemptResponse;


        }

        public void RecordToken(RecordTokenRequest rtr)
        {

            var res = _advertContext.Client.Where(c => c.IdClient == rtr.IdClient).FirstOrDefault();
            res.TokenString = rtr.refreshTokenValue;
            _advertContext.SaveChanges();

        }

        public ValidateTokenResponse ValidateTheToken(string requestToken)
        {


            int reqToken = Int32.Parse(requestToken);

            ValidateTokenResponse valTokenResp = _advertContext.Client.Where(c => c.IdClient == reqToken).Select(p => new ValidateTokenResponse
            {
                Login = p.Login,
                IdClient = p.IdClient,
                FirstName = p.FirstName,
                TokenString = p.TokenString

            }).FirstOrDefault();


            return valTokenResp;


        }


        public List<CampaignResponse> GetListOfAllCampaigns()
        {



            List<CampaignResponse> resp = _advertContext.Campaign.Select(s2 => new CampaignResponse
            {
                IdCampaign = s2.IdCampaign,
                StartDate = s2.StartDate,
                EndDate = s2.EndDate,
                FromIdBuilding = s2.FromIdBuilding,
                ToldIdBuilding = s2.ToldBuilding,
                ClientsInfo = _advertContext.Campaign.Include(cm => cm.IdClientNavigation)
                                                                                            .Where(cm => cm.IdCampaign == s2.IdCampaign)
                                                                                            .Select(s => new Clients
                                                                                            {
                                                                                                FirstName = s.IdClientNavigation.FirstName,
                                                                                                LastName = s.IdClientNavigation.LastName,
                                                                                                Email = s.IdClientNavigation.Email,
                                                                                            }).FirstOrDefault(),

                ListOfAdds = _advertContext.Banner.Include(cm2 => cm2.IdCampaignNavigation)
                                                                                            .Where(cm => cm.IdCampaign == s2.IdCampaign)
                                                                                            .Select(s => new Adds
                                                                                            {
                                                                                                Name = s.Name
                                                                                            }).ToList()




            }).OrderByDescending(h => h.StartDate).ToList();

            return resp;
        }

        public void SaveLogData(string method, string path, string body, string query)
        {
            string filePath = "requestsLog.txt";
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"HTTP Method: {method}");
                sw.WriteLine($"Endpoint path: {path}");
                sw.WriteLine($"Body of request: {body}");
                sw.WriteLine($"Query string: {query}");
                sw.WriteLine();
            }
        }



    }
}
