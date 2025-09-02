using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experience2Notion.Services;
public class GoogleImageSearcher
{
    public GoogleImageSearcher()
    {
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_API_KEY");
        var searcher = new CustomSearchAPIService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "Experience2Notion"
        });
    }
}
