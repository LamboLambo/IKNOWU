using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaregiverIKNOWU.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Windows.Storage.Streams;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.AccessCache;

namespace CaregiverIKNOWU.Services
{
    class AzureBlobService
    {
        #region Attributes
        //Set the Storage Credential
        private static readonly StorageCredentials cred = new StorageCredentials(
    "iknowu", "SqJoJfVI0DlaeL0rrsnprGvovrSkzPT03RwVAY7l01YeDImxA7gH4AnvbFTHUFs1qbDmIt389pfrSWgjsh5toQ==");

        public static string clientName = "http://iknowu.blob.core.windows.net/";
        public static string containerName = MainPage.PatientId; 


        #endregion


        #region Get Methods
        ///<summary>
        ///Get the container. Create one if not exist. 
        ///</summary>
        public async static Task<CloudBlobContainer> GetContainer()
        {
            CloudBlobContainer container = new CloudBlobContainer(
                new Uri(clientName + containerName + "/"), cred);

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container,
                new BlobRequestOptions(), new OperationContext());

            return container;
        }

        ///<summary>
        ///Get the address of an image
        ///</summary>
        public static string GetImageAddress(string uploadImageNameWithType)
        {
            return clientName + containerName + "/" + uploadImageNameWithType;
        }

        ///<summary>
        ///Display an image file
        ///</summary>
        public static async Task<BitmapImage> DisplayImageFile(string imageAddress)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(imageAddress);
            return bitmapImage;
        }



        #endregion


        #region Upload Methods
        ///<summary>
        ///Upload an image file from imageToken
        ///</summary>
        public async static Task UploadImageFromImageToken(string imageToken, string uploadImageNameWithType)
        {
            //Prepare the blob
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(uploadImageNameWithType);
            await blob.DeleteIfExistsAsync();

            //Upload Image to Blob
            StorageFile imageStorageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(imageToken);
            var imageStream = await imageStorageFile.OpenStreamForReadAsync();

            await blob.UploadFromStreamAsync(imageStream);
        }





        #endregion


        #region Delete Methods



        #endregion







    }//end class
}
