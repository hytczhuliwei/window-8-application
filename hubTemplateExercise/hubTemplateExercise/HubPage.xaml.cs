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

                //如要控制“播放”与“停止”，可以如下进行：

                //ItemPage.myMediaElement.Play(); //播放

                //ItemPage.myMediaElement.Stop(); //停止

            }
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
