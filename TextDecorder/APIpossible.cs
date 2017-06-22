using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using System.Threading.Tasks;

namespace TextDecorder
{
    class APIpossible
    {
        public List<string> readFB(string Page)
        {

            var client = new WebClient();

            string oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", "330005260749375", "ddaf754a4aac993d15217f1ece9fd5ae");

            string accessToken = client.DownloadString(oauthUrl);
            string AccessToken2 = accessToken.Substring(17, (accessToken.Count() - 24 - 17));

            string pageInfo = client.DownloadString(string.Format("https://graph.facebook.com/"+ Page + "?access_token={0} ", AccessToken2));
            string pagePosts = client.DownloadString(string.Format("https://graph.facebook.com/" + Page + "/posts?access_token={0} ", AccessToken2));
            JObject result = JsonConvert.DeserializeObject<JObject>(pagePosts);
            List<string> Posts = new List<string>();
            foreach (JObject Item in result.First.First)
            {
                try
                {
                    Posts.Add(((Newtonsoft.Json.Linq.JValue)Item.GetValue("message")).Value.ToString());
                }
                catch
                {

                }

            }
            return Posts;
        }

        public void test()
        {


        }

        public DictionResonse Read(string word_id, string before, string after)
        {
            HttpWebRequest req = null;

            string language = "en";
            //string word_id = "Clever";

            string url = "https://od-api.oxforddictionaries.com:443/api/v1/entries/" + language + "/" + word_id.ToLower() + "/synonyms";

            req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Headers.Add("app_id", "76db9132");
            req.Headers.Add("app_key", "eff83c5721a720de8fc33295c8f80c3b");

            req.Method = WebRequestMethods.Http.Get;
            req.Accept = "application/json";
            DictionResonse PartItem = new DictionResonse() { SearchedWord = word_id };

            using (HttpWebResponse HWR_Response = (HttpWebResponse)req.GetResponse())
            using (Stream respStream = HWR_Response.GetResponseStream())
            using (StreamReader sr = new StreamReader(respStream, Encoding.UTF8))
            {
                //string theJson = sr.ReadToEnd();

                var item = JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                PartItem.Response = item;
                if (before != "")
                    PartItem.BeforeWord.Add(new KeyValuePair<string, int>(key:before,value: 1));
                if (before != "")
                    PartItem.AfterWord.Add(new KeyValuePair<string, int>(key: after, value: 1 ));
            }
            return PartItem;

        }

    }


    public class TotalGroup
    {
        public List<KeyValuePair<string, DictionResonse>> Words { get; set; }

    }


    public class DictionResonse
    {
        public string SearchedWord { get; set; }
        public JObject Response
        {
            set
            {
                BeforeWord = new List<KeyValuePair<string, int>>();
                AfterWord = new List<KeyValuePair<string, int>>();
                ResultList = new List<KeyValuePair<string, Results>>();
                foreach (JObject Item232 in value["results"].First["lexicalEntries"].First["entries"].First["senses"])
                {
                    string example = ((Newtonsoft.Json.Linq.JValue)Item232["examples"].First.First.First).ToString();

                    JArray Itemtest1 = (Newtonsoft.Json.Linq.JArray)Item232["synonyms"];
                    Dictionary<string, Results> Object = new Dictionary<string, Results>();
                    foreach (JObject itemtt in Itemtest1)
                    {

                        Object.Add(itemtt.Value<JValue>("text").Value.ToString(), new Results() { ID = itemtt.Value<JValue>("id").Value.ToString(), Lang = itemtt.Value<JValue>("language").Value.ToString(), Example = example });
                    }
                    foreach (var item in Object)
                        try
                        {
                            ResultList.Add(new KeyValuePair<string, Results>(key:item.Key,value: item.Value));
                        }
                        catch { }
                }
            }
        }
        public List<KeyValuePair<string, Results>> ResultList { get; set; }
        public List<KeyValuePair<string, int>> AfterWord { get; set; }
        public List<KeyValuePair<string, int>> BeforeWord { get; set; }
        public string TotalUse { get; set; }
    }
    public class Results
    {
        public string ID { get; set; }
        public string Lang { get; set; }
        public string Example { get; set; }
    }

    public class TotalGroupMain
    {
        public Dictionary<string, DictionResonse> Words { get; set; }
    }
}
