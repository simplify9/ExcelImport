using SW.Pmm.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SW.Pmm.UnitTests
{
    public static class ExtensionMethods
    {
        public static HttpClient<ApiRequest<TDto>, object, ErrorDto[]> GetHttpClient<TDto>(this ApiRequest<TDto> apiRequest , HttpClient httpClient)
        {
            return new HttpClient<ApiRequest<TDto>, object, ErrorDto[]>(httpClient);
        }
    }
}
