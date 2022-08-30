using Microsoft.AspNetCore.WebUtilities;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetClub.AppService.AppServices.UtilAppService
{
    public class UriAppService : IUriAppService
    {
        private readonly string _baseUri;

        public UriAppService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllUri(PagingParametersViewModel pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "NumberPage", pagination.NumberPage.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "NumberRecords", pagination.NumberRecords.ToString());

            return new Uri(modifiedUri);
        }

    }
}
