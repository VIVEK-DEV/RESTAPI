using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GoodRestApiWebMvc.Models
{
    public    class ApiHelper
    {
        public static string newstartdate;
        public   List<RootObject> finalFeeed = new List<RootObject>();
        public static int countFeed;
        public  async Task<List<RootObject>> GetAPIData(string param, string startdate, string enddate)
        {
            try
            {
                //Make First API call between start date to End Date.
                //Get 100 chunks of feeds.
                //Keep EndDate Fixed.
                //Update StartDate in each call  with last(100th) feed time stamp.
                //while feed count is 100  , call again with updated start date 
                //when feed count is less 100 execute last call
                //

                HttpClient client = new HttpClient();
                enddate = "2017-12-31T04%3A07%3A56.271Z";
                if (String.IsNullOrEmpty(startdate))
                {
                    startdate = "2016-01-01T01%3A07%3A56.271Z";
                }
                string url = "https://badapi.iqvia.io/api/v1/Tweets?startDate=" + startdate + "&endDate=" + enddate;
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    List<RootObject> detailsitem = new List<RootObject>();
                    var result = await content.ReadAsStringAsync();
                    detailsitem = JsonConvert.DeserializeObject<List<RootObject>>(result);
                    finalFeeed = finalFeeed.Concat(detailsitem).ToList();
                    while (detailsitem.Count == 100)
                    {
                        {
                            var updatedstartdate = detailsitem[detailsitem.Count - 1].stamp;
                            DateTime d = updatedstartdate;
                            string updatedStartDateString = d.ToString("MM/dd/yyyy");
                            newstartdate = updatedStartDateString;
                            string isodate = d.ToString("yyyy-MM-ddTHH:mm:ssZ");
                            string newurl = "https://badapi.iqvia.io/api/v1/Tweets?startDate=" + isodate + "&endDate=" + enddate;
                            using (HttpResponseMessage response1 = await client.GetAsync(newurl))
                            using (HttpContent content1 = response1.Content)
                            {
                                var result1 = await content1.ReadAsStringAsync();
                                var detailsitem1 = JsonConvert.DeserializeObject<List<RootObject>>(result1);
                                finalFeeed = finalFeeed.Concat(detailsitem1).ToList();
                                detailsitem = detailsitem1;

                            }
                        }

                    }

                    {

                        var updatedstartdate = finalFeeed[finalFeeed.Count - 1].stamp;
                        DateTime d = updatedstartdate;
                        string updatedStartDateString = d.ToString("yyyy-MM-ddTHH:mm:ssZ");
                        string lasturl = "https://badapi.iqvia.io/api/v1/Tweets?startDate=" + updatedStartDateString + "&endDate=" + enddate;
                        using (HttpResponseMessage response2 = await client.GetAsync(lasturl))
                        using (HttpContent content2 = response2.Content)
                        {
                            var result2 = await content2.ReadAsStringAsync();
                            var detailsitem2 = JsonConvert.DeserializeObject<List<RootObject>>(result2);
                            finalFeeed = finalFeeed.Concat(detailsitem2).ToList();
                            var distinct = finalFeeed.GroupBy(x => x.id).Select(y => y.First());
                            countFeed = distinct.Count();


                        }
                    }

                    return finalFeeed;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}