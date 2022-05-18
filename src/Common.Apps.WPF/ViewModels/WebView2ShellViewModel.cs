using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

namespace Common.Apps.WPF.ViewModels
{
    public class WebView2ShellViewModel : ObservableObject
    {
        #region fileds
        #endregion

        #region construct
        public WebView2ShellViewModel()
        {
            try
            {
                OpenDownloadUrlCommand = new AsyncRelayCommand(OpenDownloadUrl);
                Initlizing = true;
                var version = CoreWebView2Environment.GetAvailableBrowserVersionString();
            }
            catch (WebView2RuntimeNotFoundException)
            {
                ShowTips = true;
            }
            finally
            {
                Initlizing = false;
            }
        }

        #endregion


        #region properties
        bool _initlizing;
        public bool Initlizing { get => _initlizing; set => SetProperty(ref _initlizing, value); }
        bool _showTips;
        public bool ShowTips { get => _showTips; set => SetProperty(ref _showTips, value); }
        #endregion

        #region public
        public IAsyncRelayCommand? OpenDownloadUrlCommand { get; }

        private Task OpenDownloadUrl()
        {
            try
            {
                string url = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
                if (url != null)
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
