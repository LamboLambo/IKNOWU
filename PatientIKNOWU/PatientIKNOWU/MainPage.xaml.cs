using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using PatientIKNOWU.Models;
using PatientIKNOWU.Services;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using Windows.Foundation;
using Windows.System.Display;
using Windows.Graphics.Display;
using Windows.ApplicationModel;
using Windows.UI.Core;


namespace PatientIKNOWU
{
    public sealed partial class MainPage : Page
    {



        //Materials
        public static BitmapImage nullBitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));

        //User Attributes
        string PatientName = "John";
        public static string PatientId = "test"; //afcwf342ju2q3r



        //Webcam Attributes
        private MediaCapture _mediaCapture;

        bool _isPreviewing;
        DisplayRequest _displayRequest;
        private int saveFileCount = 0;
        StorageFile file;
        string fileNameWithType;





        public MainPage()
        {
            this.InitializeComponent();

            _displayRequest = new DisplayRequest();

            Application.Current.Suspending += Application_Suspending;



        }


        #region Webcam

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanupCameraAsync();
                deferral.Complete();
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanupCameraAsync();
        }

        private async Task StartPreviewAsync()
        {
            try
            {

                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();

                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                //textBlock.Text = "The app was denied access to the camera";
                return;
            }

            try
            {
                CamPreview.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (System.IO.FileLoadException)
            {
                //_mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
            }

        }

        private async Task CleanupCameraAsync()
        {
            if (_mediaCapture != null)
            {
                if (_isPreviewing)
                {
                    await _mediaCapture.StopPreviewAsync();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    CamPreview.Source = null;
                    if (_displayRequest != null)
                    {
                        _displayRequest.RequestRelease();
                    }

                    _mediaCapture.Dispose();
                    _mediaCapture = null;
                });
            }

        }


        /// <summary>
        /// Handles "streaming" button clicks to start/stop webcam streaming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CameraStreamingButton_Click(object sender, RoutedEventArgs e)
        {
            await StartPreviewAsync();
        }

        /// <summary>
        /// CaptureButton to capture a picture to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            CaptureToFile();            
        }

        /// <summary>
        /// UploadButton to upload a picture to blob
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            UploadToBlob();
        }


        /// <summary>
        /// Capture a Picture Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CaptureToFile()
        {
            try
            {
                // Prepare and capture photo
                var lowLagCapture = await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

                var capturedPhoto = await lowLagCapture.CaptureAsync();
                var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

                await lowLagCapture.FinishAsync();

                if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
                softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                var bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync(softwareBitmap);

                captureImage.Source = bitmapSource;


                //to file
                saveFileCount++;
                fileNameWithType = saveFileCount.ToString() + " " + fileNameBox.Text + ".jpg";
                
                var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
                file = await myPictures.SaveFolder.CreateFileAsync(fileNameWithType, CreationCollisionOption.GenerateUniqueName);

                using (var captureStream = new InMemoryRandomAccessStream())
                {
                    await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

                    using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var decoder = await BitmapDecoder.CreateAsync(captureStream);
                        var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

                        var properties = new BitmapPropertySet {
            { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
        };
                        await encoder.BitmapProperties.SetPropertiesAsync(properties);

                        await encoder.FlushAsync();
                    }
                }


            }
            catch
            {

            }

        }//end CaptureToFile

        
        /// <summary>
        /// Upload the Picture to Blob
        /// </summary>
        private async void UploadToBlob() //TODO: AndDeleteFile
        {
            //Upload to Azure Blob

            // User cancelled
            if (file == null)
                return;

            // Prepare the photo
            IRandomAccessStream irandom = await file.OpenAsync(FileAccessMode.Read);
            BitmapImage bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(irandom);

            string faToken = null;
            faToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);

            // Set the Token and Add it to the list
            await AzureBlobService.UploadImageFromImageToken(faToken, fileNameWithType);


            //Delete the file
            //file.DeleteAsync();
            await CleanupCameraAsync();

        }












        #endregion








    }//end class
}
