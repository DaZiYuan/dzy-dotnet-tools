using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Services.Store;
using Windows.System;
namespace Common.Windows.Helpers
{
    public class StoreHelper
    {
        private StoreContext context = null;
        private readonly IntPtr mainHandler;

        public StoreHelper(IntPtr intptr)
        {
            mainHandler = intptr;
        }

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }

        public async Task<List<StoreProduct>> GetProductInfo(string[] productKinds, string[] storeIds)
        {
            List<StoreProduct> result = new List<StoreProduct>();
            if (context == null)
                context = GetContext();

            // Specify the kinds of add-ons to retrieve.
            List<String> filterList = new List<string>(productKinds);

            StoreProductQueryResult queryResult =
                await context.GetStoreProductsAsync(filterList, storeIds);

            if (queryResult.ExtendedError != null)
            {
                // The user may be offline or there might be some other server failure.
                string err = $"ExtendedError: {queryResult.ExtendedError.Message}";
                return null;
            }

            foreach (KeyValuePair<string, StoreProduct> item in queryResult.Products)
            {
                StoreProduct product = item.Value;
                result.Add(product);
            }

            return result;
        }

        public async Task<StorePurchaseResult> PurchaseAddOn(string storeId)
        {
            if (context == null)
                context = GetContext();

            StorePurchaseResult result = await context.RequestPurchaseAsync(storeId);
            return result;
        }

        public async Task GetLicenseInfo()
        {
            if (context == null)
                context = GetContext();

            StoreAppLicense appLicense = await context.GetAppLicenseAsync();

            if (appLicense == null)
                return;

            // Use members of the appLicense object to access license info...

            // Access the valid licenses for durable add-ons for this app.
            foreach (KeyValuePair<string, StoreLicense> item in appLicense.AddOnLicenses)
            {
                StoreLicense addOnLicense = item.Value;
                // Use members of the addOnLicense object to access license info
                // for the add-on.
            }
        }
        private StoreContext GetContext()
        {
            StoreContext context = null;

#if WINDOWS_UWP
            context = StoreContext.GetDefault();
#else
            context = StoreContext.GetDefault();
            IInitializeWithWindow initWindow = (IInitializeWithWindow)(object)context;
            initWindow.Initialize(mainHandler);
#endif
            return context;
        }

        public async Task<bool> ShowRatingStoreDialog()
        {
            //string id = Package.Current.Id.ProductId;
            //bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri($"ms-windows-store://review/?ProductId={id}"));
            //return result;

            //旧方法，不推荐的方式.但是推荐的方式获取不到ID
            var pfn = Package.Current.Id.FamilyName;
            var uri = new Uri($"ms-windows-store://review/?PFN={pfn}");
            bool success = await Launcher.LaunchUriAsync(uri);
            return success;
        }

        public async Task<bool> ShowProductInfo()
        {
            //https://docs.microsoft.com/zh-cn/windows/uwp/launch-resume/launch-store-app

            var pfn = Package.Current.Id.FamilyName;
            var uri = new Uri($"ms-windows-store://pdp/?PFN={pfn}");
            bool success = await Launcher.LaunchUriAsync(uri);
            return success;
        }

        public async Task<bool> ShowRatingReviewDialog()
        {
            if (context == null)
                context = GetContext();

            StoreSendRequestResult result = await StoreRequestHelper.SendRequestAsync(context, 16, string.Empty);

            if (result.ExtendedError == null)
            {
                //JObject jsonObject = JObject.Parse(result.Response);
                //if (jsonObject.SelectToken("status").ToString() == "success")
                //{
                //    // The customer rated or reviewed the app.
                    return true;
                //}
            }

            // There was an error with the request, or the customer chose not to
            // rate or review the app.
            return false;
        }

        public async Task<StorePackageUpdateResult> DownloadAndInstallAllUpdatesAsync(Func<bool> confirm, Action<StorePackageUpdateStatus> update)
        {
            try
            {
                if (context == null)
                    context = GetContext();

                // Get the updates that are available.
                IReadOnlyList<StorePackageUpdate> updates =
                    await context.GetAppAndOptionalStorePackageUpdatesAsync();

                if (updates.Count > 0)
                {
                    // Alert the user that updates are available and ask for their consent
                    // to start the updates.
                    bool ok = confirm.Invoke();
                    if (ok)
                    {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        ShowProductInfo();

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        // Download and install the updates.
                        IAsyncOperationWithProgress<StorePackageUpdateResult, StorePackageUpdateStatus> downloadOperation =
                            context.RequestDownloadAndInstallStorePackageUpdatesAsync(updates);

                        // The Progress async method is called one time for each step in the download
                        // and installation process for each package in this request.
                        downloadOperation.Progress = (asyncInfo, progress) =>
                       {
                           update(progress);
                       };

                        StorePackageUpdateResult result = await downloadOperation.AsTask();
                        return result;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
