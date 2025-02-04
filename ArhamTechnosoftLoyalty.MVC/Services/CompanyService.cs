﻿using ArhamTechnosoftLoyalty.Models.Common;
using ArhamTechnosoftLoyalty.Models.Common.MasterModel;
using ArhamTechnosoftLoyalty.Models.EntityModel;
using ArhamTechnosoftLoyalty.Models.ViewModel.Company;
using ArhamTechnosoftLoyalty.MVC.Helper;
using ArhamTechnosoftLoyalty.MVC.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ArhamTechnosoftLoyalty.MVC.Services
{
    public class CompanyService : ICompanyService
    {
        private LoyaltyAPI _loyaltyAPI;
        private AppSettings _appSettings;
        public CompanyService(IOptions<AppSettings> appSettings)
        {
            _loyaltyAPI = new LoyaltyAPI();
            _appSettings = appSettings.Value;
        }
        public async Task<string> SaveCompany(CompanyMaster companyMaster)
        {
            try
            {
                HttpClient client = _loyaltyAPI.Initial(_appSettings.ApiUrl.ToString());
                var responseTask = await client.PostAsJsonAsync("company/add-company", companyMaster);
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content.ReadAsStringAsync().Result;
                    CustomResponse<bool> Listing = JsonConvert.DeserializeObject<CustomResponse<bool>>(result);
                    return "SUCCESS";
                }
                else
                {
                    return responseTask.Content.ToString();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<CompanyListModel>> GetCompanyList(long? companyId)
        {
            try
            {
                HttpClient client = _loyaltyAPI.Initial(_appSettings.ApiUrl.ToString());
                var responseTask = await client.GetAsync("company/get-company-list/" + companyId);
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content.ReadAsStringAsync().Result;
                    CustomResponse<List<CompanyListModel>> Listing = JsonConvert.DeserializeObject<CustomResponse<List<CompanyListModel>>>(result);
                    return Listing.data;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }

        public async Task<CompanyMaster> GetCompanyMaster(long? companyId)
        {
            try
            {
                HttpClient client = _loyaltyAPI.Initial(_appSettings.ApiUrl.ToString());
                var responseTask = await client.GetAsync("company/get-company-master/" + companyId);
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content.ReadAsStringAsync().Result;
                    CustomResponse<CompanyMaster> master = JsonConvert.DeserializeObject<CustomResponse<CompanyMaster>>(result);
                    return master.data;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }

        public async Task<ParentItems> GetMaster(List<string> Keys)
        {
            try
            {
                HttpClient client = _loyaltyAPI.Initial(_appSettings.ApiUrl.ToString());
                var responseTask = await client.PostAsJsonAsync("GenericApi/get-key-value-list", Keys);
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content.ReadAsStringAsync().Result;
                    var Dropdownmaster = JsonConvert.DeserializeObject<CustomResponseData<ParentItems>>(result);
                    return Dropdownmaster.data.parentItems;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<MasterDropdown>> GetMasterByParent(string Key, int parentId)
        {
            try
            {
                HttpClient client = _loyaltyAPI.Initial(_appSettings.ApiUrl.ToString());
                var responseTask = await client.GetAsync("GenericApi/get-key-value-by-parent/" + Key + "/" + parentId);
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content.ReadAsStringAsync().Result;
                    var Dropdownmaster = JsonConvert.DeserializeObject<CustomResponseData<ParentItems>>(result);
                    switch (Key)
                    {
                        case "city":
                            return Dropdownmaster.data.parentItems.city;
                        case "state":
                             return Dropdownmaster.data.parentItems.state;
                    }
                    return Dropdownmaster.data.parentItems.state;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }
    }
}
