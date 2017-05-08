﻿//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FamilyNotes.UserDetection;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using FamilyNotes.Models;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using FamilyNotes.Services;
using System.Collections.ObjectModel;
using Windows.System.Threading;

namespace FamilyNotes
{
    /// <summary>
    /// Class responsible for detecting faces when face detection is enabled in the app.
    /// It is also responsible for capturing images for the purpose of identification or ID verification.
    /// </summary>
    class UserPresence : BindableBase
    {

        public static Person thisPerson = new Person();
        public static Face thisFace = new Face();
        //ObservableCollection<Face> addFaceList = new ObservableCollection<Face>();
        public static Warning thisWarning = new Warning();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPresence"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public UserPresence(CoreDispatcher dispatcher, string unfilteredName)
        {
            this._dispatcher = dispatcher;
            _unfilteredName = unfilteredName;

            // This is just here until we incorporate the NoFacesTime into the settings
            NoFacesTime = 5000;
        }

        /// <summary>
        /// Configures the camera and enables face detection.
        /// </summary>
        public async Task<bool> EnableFaceDetection()
        {
            if (_mediaCapture == null)
            {
                await CreateFaceDetectionEffectAsync();
                return true;
            }
            else
            {
                await CleanUpFaceDetectionEffectAsync();
                return false;
            }
        }


        /// <summary>
        /// Keeps track of whether we are using the default camera device to capture images.
        /// If we are not, we will need to look up the device by the DeviceID.
        /// </summary>
        public bool IsDefaultCapture { get; set; } = true;

        /// <summary>
        /// Keeps track of whether or not the notes are currently filtered for a set user.
        /// Otherwise this class has no knowledge of the state of the app, and whether or not we are filtering impacts
        /// how face detection works.
        /// </summary>
        public bool _currentlyFiltered
        {
            get
            {
                return currentlyFiltered;
            }
            set
            {
                currentlyFiltered = value;
                if (!value)
                {
                    // If we are being manually set to "no filters", then we need to dispose of the timer for face capture so it will restart.
                    _holdForTimer = false;
                    if (_pictureTimer != null)
                    {
                        _pictureTimer.Dispose();
                    }
                }
                else if (_noFacesTimer != null)
                {
                    // If we are being manually set to any filter, then we need to dispose of the _noFacesTimer so we don't immediately revert
                    _noFacesTimer.Dispose();
                }
            }
        }

        /// <summary>
        /// The length of time (in milliseconds) to wait before resetting back to an unfiltered state if no faces are detected
        /// </summary>
        public int NoFacesTime { get; set; }

        /// <summary>
        /// The DeviceID for the camera. This is only important if we are not using the default camera.
        /// </summary>
        public string CameraDeviceId { get; set; }

        /// <summary>
        /// The number of faces currently detected by our face detection logic.
        /// </summary>
        public string FaceCount
        {
            get { return _faceCount; }
            set
            {
                // Update the backing field and raise PropertyChanged for this property.
                SetProperty(ref _faceCount, value);
            }
        }


        /// <summary>
        /// Event that is raised when a specific users face is detected.
        /// </summary>
        public event EventHandler<UserIdentifiedEventArgs> FilterOnFace;
        protected virtual void OnFilterOnFace(string userName)
        {
            EventHandler<UserIdentifiedEventArgs> handler = FilterOnFace;
            UserIdentifiedEventArgs eventArgs = new UserIdentifiedEventArgs(userName);
            handler(this, eventArgs);
        }

        /// <summary>
        /// Custom event args for the FilterOnFace event that contains the name of the detected user.
        /// </summary>
        public class UserIdentifiedEventArgs : EventArgs
        {
            public UserIdentifiedEventArgs(string name)
            {
                User = name;
            }
            public string User { get; set; }
        }

        /// <summary>
        /// Initializes the MediaCapture and starts preview.
        /// </summary>
        private async Task InitializeCameraAsync()
        {
            if (_mediaCapture == null)
            {
                // Create MediaCapture and its settings
                _mediaCapture = new MediaCapture();

                var Settings = new MediaCaptureInitializationSettings();

                // If we are using the default device, get it
                if (IsDefaultCapture)
                {
                    // Attempt to get the front camera if one is available, but use any camera device if not
                    var CameraDevice = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Front);

                    if (CameraDevice == null)
                    {
                        Debug.WriteLine("No camera device found!");
                        return;
                    }

                    // Set the global camera device id
                    CameraDeviceId = CameraDevice.Id;
                }

                // Set the VideoDeviceId for the settings
                Settings.VideoDeviceId = CameraDeviceId;

                // Initialize MediaCapture
                try
                {
                    await _mediaCapture.InitializeAsync(Settings);
                    _isInitialized = true;
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine("The app was denied access to the camera.");
                }

                // If initialization succeeded, start the preview
                if (_isInitialized)
                {
                    await StartPreviewAsync();
                }
            }
        }

        /// <summary>
        /// Starts the preview
        /// </summary>
        private async Task StartPreviewAsync()
        {
            if (_cap == null)
            {
                _cap = new CaptureElement();
            }
            _cap.Source = _mediaCapture;


            // Start the preview
            await _mediaCapture.StartPreviewAsync();
        }

        /// <summary>
        /// Stops the preview 
        /// </summary>
        private async Task StopPreviewAsync()
        {
            // Stop the preview
            await _mediaCapture.StopPreviewAsync();
            _cap.Source = null;
            _cap = null;
        }

        /// <summary>
        /// Adds face detection to the preview stream, registers for its events, enables it, and gets the FaceDetectionEffect instance
        /// </summary>
        private async Task CreateFaceDetectionEffectAsync()
        {
            // First intialize the camera
            await InitializeCameraAsync();

            // Create the definition, which will contain some initialization settings
            var Definition = new FaceDetectionEffectDefinition();

            // To ensure preview smoothness, do not delay incoming samples
            Definition.SynchronousDetectionEnabled = false;

            // In this scenario, choose balance over speed or accuracy
            Definition.DetectionMode = FaceDetectionMode.Balanced;

            // Add the effect to the preview stream
            _faceDetectionEffect = (FaceDetectionEffect)await _mediaCapture.AddVideoEffectAsync(Definition, MediaStreamType.VideoPreview);

            // Register for face detection events
            _faceDetectionEffect.FaceDetected += FaceDetectionEffect_FaceDetected;

            // Choose the shortest interval between detection events
            _faceDetectionEffect.DesiredDetectionInterval = TimeSpan.FromMilliseconds(33);

            // Start detecting faces
            _faceDetectionEffect.Enabled = true;
        }

        /// <summary>
        ///  Disables and removes the face detection effect, and unregisters the event handler for face detection
        /// </summary>
        private async Task CleanUpFaceDetectionEffectAsync()
        {
            // Disable detection
            _faceDetectionEffect.Enabled = false;

            // Unregister the event handler
            _faceDetectionEffect.FaceDetected -= FaceDetectionEffect_FaceDetected;

            // Remove the effect from the preview stream
            await _mediaCapture.ClearEffectsAsync(MediaStreamType.VideoPreview);

            // Clear the member variable that held the effect instance
            _faceDetectionEffect = null;
            // Delete the CaptureElement
            if (_cap != null)
            {
                await StopPreviewAsync();
            }
            // Delete the media capture device if it exists
            if (_mediaCapture != null)
            {
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }
            // Kill the timer if it exists
            if (_pictureTimer != null)
            {
                _holdForTimer = false;
                _pictureTimer.Dispose();
                _pictureTimer = null;
            }
        }

        /// <summary>
        /// Cleans up the camera resources
        /// </summary>
        private async Task CleanupCameraAsync()
        {
            if (_isInitialized)
            {
                if (_faceDetectionEffect != null)
                {
                    await CleanUpFaceDetectionEffectAsync();
                }
                _isInitialized = false;
            }

            if (_mediaCapture != null)
            {
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }
        }

        /// <summary>
        /// Attempts to find and return a device mounted on the panel specified, and on failure to find one it will return the first device listed
        /// </summary>
        /// <param name="DesiredPanel">The desired panel on which the returned device should be mounted, if available</param>
        private static async Task<DeviceInformation> FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel DesiredPanel)
        {
            // Get available devices for capturing pictures
            var AllVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            // Get the desired camera by panel
            DeviceInformation DesiredDevice = AllVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == DesiredPanel);

            // If there is no device mounted on the desired panel, return the first device found
            return DesiredDevice ?? AllVideoDevices.FirstOrDefault();
        }

        private async void FaceDetectionEffect_FaceDetected(FaceDetectionEffect sender, FaceDetectedEventArgs args)
        {
            // Ask the UI thread to render the face count information
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CountDetectedFaces(args.ResultFrame.DetectedFaces));
        }

        /// <summary>
        /// Retrieves the count of detected faces
        /// </summary>
        /// <param name="faces">The list of detected faces from the FaceDetected event of the effect</param>
        private async void CountDetectedFaces(IReadOnlyList<DetectedFace> faces)
        {
            FaceCount = $"{_detectionString} {faces.Count.ToString()}";

            // If we detect any faces, kill our no faces timer
            if (faces.Count != 0)
            {
                if (_noFacesTimer != null)
                {
                    _noFacesTimer.Dispose();
                }
            }
            // Otherwise, if we are filtering and don't have a timer
            else if (_currentlyFiltered &&  (_noFacesTimer == null)) 
            {
                // Create a callback
                TimerCallback noFacesCallback = (object stateInfo) =>
                {
                    _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        OnFilterOnFace(_unfilteredName);
                        _noFacesTimer = null;
                    });
                    _noFacesTimer.Dispose();
                };

                // Set our timer
                _noFacesTimer = new Timer(noFacesCallback, null, NoFacesTime, Timeout.Infinite);
            }

            

            // We are also going to take an image the first time that we detect exactly one face.
            // Sidenote - to avoid a race-condition, I had to use a boolean. Just checking for _faceCaptureStill == null could produce an error.
            if ((faces.Count == 1) && !_holdForTimer) //  && !_currentlyFiltered   
            {
                // Kick off the timer so we don't keep taking pictures, but will resubmit if we are not filtered
                _holdForTimer = true;
                //MainPage.isOnTime = false;

                // Take the picture
                _faceCaptureStill = await ApplicationData.Current.LocalFolder.CreateFileAsync("FaceDetected.jpg", CreationCollisionOption.ReplaceExisting);
                await _mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), _faceCaptureStill);


                if (((App)Application.Current).AppSettings.FaceApiKey != "" && FacialSimilarity.InitialTrainingPerformed)
                {
                    var UserName = await FacialSimilarity.CheckForUserAsync(new Uri("ms-appdata:///local/FaceDetected.jpg"));
                    if (UserName != "")
                    {
                        OnFilterOnFace(UserName);
                    }
                    else //strange face is detected
                    {
                        #region a strange face is detected
                        //Create a new Person
                        thisPerson = new Person();
                        thisPerson.Name = "stranger";
                        thisPerson.FriendlyName = "stranger";
                        thisPerson.Relation = "";
                        thisPerson.PatientId = MainPage.PatientId;
                        await AzureDatabaseService.UploadPersonInfo(thisPerson);
                        thisPerson = new Person();
                        thisPerson = await AzureDatabaseService.GetLatestPerson();
                        thisPerson.FriendlyName += thisPerson.Id;
                        await AzureDatabaseService.UploadPersonInfo(thisPerson);
                        

                        //Create a new Face
                        thisFace = new Face();

                        // Prepare the captured image
                        StorageFile newImageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("FaceDetected.jpg");
                        //StorageFolder newPersonFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(("Users\\" + thisPerson.FriendlyName), CreationCollisionOption.OpenIfExists);
                        //await newImageFile.CopyAsync(newPersonFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
                        string faToken = null;
                        IRandomAccessStream irandom = await newImageFile.OpenAsync(FileAccessMode.Read);
                        faToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(newImageFile);

                        thisFace.Image = new BitmapImage();
                        await thisFace.Image.SetSourceAsync(irandom);
                        thisFace.ImageToken = faToken;
                        thisFace.IsDefault = true;
                        

                        //Upload the Face
                        //addFaceList.Add(thisFace);
                        await AzureDatabaseService.UploadOneFaceInfo(thisPerson.Id, thisFace);
                        //addFaceList.Clear();
                        thisFace = new Face();
                        thisFace = await AzureDatabaseService.GetLatestFace();
                        ObservableCollection<Face> newFaces = new ObservableCollection<Face>();
                        newFaces.Add(thisFace);


                        MainPage.Persons.Add(thisPerson);
                        //MainPage.Faces.Add(newFaces);

                        //Upload the image
                        //await AzureBlobService.UploadImageFromImageToken(faToken, thisFace.Id + ".jpg");
                        Face updatedFace = await AzureDatabaseService.UpdateFaceImageAddress(thisFace);
                        thisFace = updatedFace;
                        await AzureDatabaseService.UpdatePersonDefaultImageAddress(thisPerson.Id, thisFace);
                        thisPerson = await AzureDatabaseService.GetLatestPerson();

                        MainPage.Persons.Add(thisPerson);

                        //Upload a Stranger Warning Enquiry
                        thisWarning.PersonId = thisPerson.Id;
                        await AzureDatabaseService.UploadWarning(thisWarning);

                        // Update the profile picture for the person
                        //await FacialSimilarity.AddTrainingImageAsync(thisPerson.FriendlyName, new Uri($"ms-appdata:///local/Users/{thisPerson.FriendlyName}/ProfilePhoto.jpg"));
                        AddNewPersonFromDB(thisPerson, newFaces);

                        //Stranger Notice
                        MainPage.thisPhrase = "stranger";
                        StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Media");
                        StorageFile file = await folder.GetFileAsync("dong.wav");
                        var soundStream = await file.OpenAsync(FileAccessMode.Read);
                        MainPage.mediaElement.SetSource(soundStream, file.ContentType);
                        MainPage.mediaElement.Play();

                        await MainPage._speechManager.SpeakAsync("Stranger", MainPage.mediaElement);

                        
                        //Perform initialization for facial detection.
                        //await FacialSimilarity.TrainDetectionAsync();

                        #endregion
                    }
                }

                // Allow the camera to take another picture in 10 seconds
                TimerCallback callback = (Object stateInfo) =>
                {
                    // Now that the timer is expired, we no longer need to hold
                    // Nothing else to do since the timer will be restarted when the picture is taken
                    _holdForTimer = false;
                    if (_pictureTimer != null)
                    {
                        _pictureTimer.Dispose();
                    }
                };
                _pictureTimer = new Timer(callback, null, 10000, Timeout.Infinite);


                //// (Use Delay) Allow the camera to take another picture in 10 seconds
                //TimeSpan delay = TimeSpan.FromSeconds(10);

                //ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreateTimer(
                //    (source) =>
                //    {
                //        _holdForTimer = false;

                //    }, delay);

            }//end if ((faces.Count == 1) && !_holdForTimer && !_currentlyFiltered)


        }


        // MediaCapture and its state variables
        private bool currentlyFiltered;
        private MediaCapture _mediaCapture;
        private bool _isInitialized;
        private StorageFile _faceCaptureStill = null;
        private static bool _holdForTimer = false;
        private static Timer _pictureTimer;
        private CaptureElement _cap;
        private FaceDetectionEffect _faceDetectionEffect;
        private string _faceCount = "0";
        private CoreDispatcher _dispatcher;
        private const string _detectionString = "Detected faces : ";
        private static Timer _noFacesTimer;
        private string _unfilteredName;



        private async void AddNewPersonFromDB(Person person, ObservableCollection<Face> faces)
        {
            //var dialog = new AddPersonContentDialog();

            //dialog.ProvideExistingPerson(currentPerson);
            //await dialog.ShowAsync();

            //dialog.AddedPerson = person;
            //Person newPerson = dialog.AddedPerson;
            Person newPerson = person;


            // If there is a valid person to add, add them
            if (newPerson != null)
            {
                try
                {
                    // Get or create a directory for the user (we do this regardless of whether or not there is a profile picture)
                    StorageFolder userFolder;
                    if (person.FriendlyName != null && person.FriendlyName != "" && person.FriendlyName.Contains("stranger"))
                    {
                        userFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(("Users\\" + newPerson.FriendlyName), CreationCollisionOption.FailIfExists);
                    }
                    else
                    {
                        userFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(("Users\\" + newPerson.Name), CreationCollisionOption.FailIfExists);
                    }

                    StorageFile userFile = await userFolder.CreateFileAsync("ProfilePhoto.jpg", CreationCollisionOption.OpenIfExists);
                    foreach (Face face in faces)
                    {
                        await AzureBlobService.DownloadFaceImageAsFile(face, userFile);
                    }

                    newPerson.IsProfileImage = true;
                    newPerson.ImageFileName = userFolder.Path + "\\ProfilePhoto.jpg";


                }
                catch
                {

                }

                // See if we have a profile photo
                //if (dialog.TemporaryFile != null)
                //{
                // Save off the profile photo and delete the temporary file
                //await dialog.TemporaryFile.CopyAsync(userFolder, "ProfilePhoto.jpg", NameCollisionOption.GenerateUniqueName);
                //await dialog.TemporaryFile.DeleteAsync();

                // Update the profile picture for the person
                await FacialSimilarity.AddTrainingImageAsync(newPerson.FriendlyName, new Uri($"ms-appdata:///local/Users/{newPerson.FriendlyName}/ProfilePhoto.jpg"));

                person = newPerson;

                //}
                // Add the user if it is new now that changes have been made
                //if (currentPerson == null)
                //{
                //    await FamilyModel.AddPersonAsync(newPerson);
                //}
                // Otherwise we had a user, so update the current one
                //else
                //{
                //    //await FamilyModel.UpdatePersonImageAsync(newPerson);
                //    Person personToUpdate = FamilyModel.PersonFromName(currentPerson.FriendlyName);
                //    if (personToUpdate != null)
                //    {
                //        personToUpdate.IsProfileImage = true;
                //        personToUpdate.ImageFileName = userFolder.Path + "\\ProfilePhoto.jpg";
                //    }
                //}
            }



        }



    }//end class
}
