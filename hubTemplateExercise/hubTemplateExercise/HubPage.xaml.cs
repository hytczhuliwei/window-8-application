using hubTemplateExercise.Common;
using hubTemplateExercise.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Data.Json;
using System.Net;
using System.Threading.Tasks;
using System.Text;


//“中心页”项模板在 http://go.microsoft.com/fwlink/?LinkID=321224 上有介绍

namespace hubTemplateExercise
{
    /// <summary>
    /// 显示分组的项集合的页。
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper 在每页上用于协助导航和
        /// 进程生命期管理
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public static MediaElement myMediaElement = new MediaElement(); //依然采用MediaElement
        public static IRandomAccessStream soundStream = null;
        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            myMediaElement.AudioCategory = AudioCategory.BackgroundCapableMedia;
            myMediaElement.IsLooping = true; //循环播放开启
        }

        private IEnumerable<string> GetSectionList()
        {
            var sections = this.Hub.Sections;
            var headers = new List<string>();

            foreach (var item in sections)
            {
                var section = (HubSection)item;
                var header = (string)section.Header;
                if (string.IsNullOrWhiteSpace(header))
                {
                    continue;
                }

                yield return header;
            }
        }
        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源; 通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。 首次访问页面时，该状态将为 null。</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Featured recipe
           // var favorites = await SampleDataSource.GetFavoriteRecipesAsync(1);
           // this.DefaultViewModel["Section1Item"] = favorites.SingleOrDefault();

            // International Cuisine
            var groups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Section2Items"] = groups;

            // Top rated
            var topRated = await SampleDataSource.GetTopRatedRecipesAsync(10);
            this.DefaultViewModel["Section3Items"] = topRated;

            this.DefaultViewModel["ZoomedOutList"] = this.GetSectionList();
            if (HubPage.soundStream == null)
            {

                string fileLocation = "ms-appx:///Sound/Jay/";

                string filename = "Jay_10.mp3"; //音乐文件sound.wav同样在sound文件夹中

                var fs = RandomAccessStreamReference.CreateFromUri(new Uri(fileLocation + filename));

                HubPage.soundStream = await fs.OpenReadAsync();

                HubPage.myMediaElement.SetSource(HubPage.soundStream, ""); //这里已经开始播放了
            }

            //var Citys = await SampleDataSource.GetCitysAsync();



            ////获取本机ip
            //string reslocalIpInfo = httpPost("http://61.4.185.48:81/g/", "", "get", "gb2312");
            //string[] reslocalIp = reslocalIpInfo.Split(new String[] { "var ip=\"", "\";var id=" }, StringSplitOptions.RemoveEmptyEntries);

            ////根据本机ip获取城市名
            //string resCityNameInfo = httpPost("http://www.ip138.com/ips138.asp?ip=" + reslocalIp[0] + "&action=2", "", "get", "gb2312");
            //string[] resCityName = resCityNameInfo.Split(new String[] { "本站主数据：", "</li><li>参考数据" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] resCityNameN = resCityName[1].Split(new String[] { "省", "市" }, StringSplitOptions.RemoveEmptyEntries);

            ////根据城市名获取城市天气id
            //string cityId = "";
            //foreach (cityInfo a in Citys)
            //{
            //    if (a.CityName == resCityNameN[1])
            //    {
            //        cityId = a.CityID;
            //        break;
            //    }
            //}


            //string localIpInfo = httpPost("http://61.4.185.48:81/g/", "", "get", "gb2312");
            //string[] cityId = localIpInfo.Split(new String[] { "var id=", ";if" }, StringSplitOptions.RemoveEmptyEntries);


            ////根据城市天气id获取天气详情
            //string resWeatherInfo = httpPost("http://m.weather.com.cn/ks/" + cityId[1] + ".html", "", "get", "utf-8");
            ////string resWeatherInfo = httpPost("http://www.weather.com.cn/atad/" + cityId + ".html", "", "get", "utf-8");
            //JsonObject jsonObject = JsonObject.Parse(resWeatherInfo);
            //JsonObject jsonObjectR = jsonObject["sk_info"].GetObject();
            //string temp = jsonObjectR["temp"].GetString();
            //string resH = jsonObjectR["temp1"].GetString();

          //  this.DefaultViewModel["weatherInfo"] = jsonObjectR;

        }
        public static string httpPost(string url, string pars, string httpload, string charset)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.Method = httpload;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            byte[] channelUriInBytes = Encoding.UTF8.GetBytes(pars);

            if (httpload == "post")
            {
                Task<Stream> requestTask = webRequest.GetRequestStreamAsync();
                using (Stream requestStream = requestTask.Result)
                {
                    requestStream.Write(channelUriInBytes, 0, channelUriInBytes.Length);
                }
            }

            string result = null;
            Task<WebResponse> response = webRequest.GetResponseAsync();
            using (Stream responseStream = response.Result.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding(charset)))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 在单击 HubSection 标题时调用。
        /// </summary>
        /// <param name="sender">包含单击了其标题的 HubSection 的中心。</param>
        /// <param name="e">描述如何启动单击的事件数据。</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// 在单击节内的项时调用。
        /// </summary>
        /// <param name="sender">GridView 或 ListView
        /// 为 ListView)。</param>
        /// <param name="e">描述所单击项的事件数据。</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 导航至相应的目标页，并
            // 通过将所需信息作为导航参数传入来配置新页
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemPage), itemId);
        }

        private void Search_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var queryText = args.QueryText;
            this.Frame.Navigate(typeof(SearchResultsPage), queryText);
        }

        private void Search_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            string queryText = args.QueryText;
            if (!string.IsNullOrWhiteSpace(queryText))
            {
                var suggestionCollection = args.Request.SearchSuggestionCollection;

                // result suggestions
                var searchResults = SampleDataSource.Search(queryText, true);
                foreach (var result in searchResults.SelectMany(p => p.Items).Take(2))
                {
                    var imageUri = new Uri("ms-appx:///" + result.TileImagePath);
                    var imageSource = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(imageUri);
                    suggestionCollection.AppendResultSuggestion(result.Title, result.Description, result.UniqueId, imageSource, result.Description);
                }

                // separator
                suggestionCollection.AppendSearchSeparator("Suggestions");

                // query suggestions
                string[] suggestionList ={"米兰的小铁匠", "七里香", "乔克叔叔", "搁浅"};

                foreach (string suggestion in suggestionList)
                {
                    if (suggestion.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase))
                    {
                        suggestionCollection.AppendQuerySuggestion(suggestion);
                    }
                }
            }
        }
        private void Search_ResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            var itemId = args.Tag;
            this.Frame.Navigate(typeof(ItemPage), itemId);
        }
        void ItemView_GroupClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(SectionPage), itemId);
        }
        #region NavigationHelper 注册

        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// 
        /// 应将页面特有的逻辑放入用于
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// 和 <see cref="GridCS.Common.NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void audioStop_Click(object sender, RoutedEventArgs e)
        {
            audioStop.Visibility = Visibility.Collapsed;
            audioPlay.Visibility = Visibility.Visible;
            HubPage.myMediaElement.Pause(); //停止
        }

        private void audioPlay_Click(object sender, RoutedEventArgs e)
        {
            audioPlay.Visibility = Visibility.Collapsed;
            audioStop.Visibility = Visibility.Visible;
            HubPage.myMediaElement.Play(); //播放
        }
    }
}
