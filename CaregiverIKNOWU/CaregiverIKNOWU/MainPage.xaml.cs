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

using CaregiverIKNOWU.Models;
using CaregiverIKNOWU.Services;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Capture;

namespace CaregiverIKNOWU
{
    public sealed partial class MainPage : Page
    {
        #region Initializations

        private Settings AppSettings { get; set; }
        bool internetConnection = false;
        ContentDialog progressDialog;

        //Materials
        BitmapImage nullBitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));

        //User Attributes
        string PatientName = "John";
        public static string PatientId = "afcwf342ju2q3r";

        //Data and view model list of items
        private ObservableCollection<Person> Persons;
        Person thisPerson;
        private ObservableCollection<Face> Faces;
        Face thisFace;  
        private ObservableCollection<Face> addFaceList;
        private ObservableCollection<Face> deleteFaceList;








        public MainPage()
        {
            this.InitializeComponent();

            //Initialize the Attributes
            Persons = new ObservableCollection<Person>();
            thisPerson = new Person();
            Faces = new ObservableCollection<Face>();
            thisFace = new Face();
            addFaceList = new ObservableCollection<Face>();
            deleteFaceList = new ObservableCollection<Face>();

            //Initialize the app's settings
            AppSettings = new Settings();
            AppSettings.LoadSettings();

            //Initialize the UI
            PersonDialog.Visibility = Visibility.Visible;
            clearWarningStatus();


            //Test the Internet Connection
            testInternetConnection();



        }





        #endregion


        #region Methods

        private async void testInternetConnection()
        {
            //Test Internet Connection
            try
            {
                var testInternetConnection = await AzureDatabaseService.isPersonTableEmpty(PatientId);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                internetConnection = false;

                showProgressDialog("Internet Connection Error! Please check your Internet connection.");
                progressDialog.Title = "Internet Connection Error!";
                progressDialog.PrimaryButtonText = "Ok, I know";
                progressDialog.IsPrimaryButtonEnabled = true;
                await progressDialog.ShowAsync();

                return;
                //Test Internet Connection Again
                //testInternetConnection();
            }

            internetConnection = true;
            return;

        }


        private void initializePersonInfoDialog()
        {
            //Dialog Appearance
            PersonDialog.Title = "Person Info Dialog";
            PersonDialogPanel.BorderThickness = new Thickness(0,0,0,0);

            //Info
            familiarRadioButton.IsChecked = false;
            strangeRadioButton.IsChecked = false;
            PersonNameInput.Text = "";
            PersonRelationInput.Text = "";
            defaultImage.Source = nullBitmapImage;
            FaceImagePreview.Source = nullBitmapImage;

            //Data
            Faces.Clear();
        }

        /// <summary>
        /// Clear Warning Status
        /// </summary>
        private void clearWarningStatus()
        {
            WarningImage.Visibility = Visibility.Collapsed;

            //Dialog Appearance
            RiskGrid.Visibility = Visibility.Collapsed;
            VideoStackPanel.Visibility = Visibility.Collapsed;

            initializePersonInfoDialog();
        }

        private async void createWarningInfo(Person person)
        {
            WarningImage.Visibility = Visibility.Visible;

            //Dialog Appearance
            RiskGrid.Visibility = Visibility.Visible;
            VideoStackPanel.Visibility = Visibility.Visible;
            PersonDialog.Title = "Stranger Warning Enquiry";
            PersonDialogPanel.BorderThickness = new Thickness(3, 3, 3, 3);

            //Show the Person's Data
            defaultImage.Source = await AzureBlobService.DisplayImageFile(person.DefaultImageAddress);

            //TODO: find a list of face images...


        }

        private async void showProgressDialog(string content)
        {
            progressDialog = new ContentDialog()
            {
                Title = "Progress: ",
                Content = content,
                PrimaryButtonText = "Ok",
                FullSizeDesired = false
            };
            progressDialog.IsPrimaryButtonEnabled = false;
            await progressDialog.ShowAsync();
        }






        #endregion

        #region Bottom Bar
        /// <summary>
        /// Display the settings popup since the user has tapped the Settings button.
        /// </summary>
        private void SettingsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!SettingsPopup.IsOpen)
            {
                rootPopupBorder.Width = 346;
                rootPopupBorder.Height = ActualHeight;
                SettingsPopup.HorizontalOffset = Window.Current.Bounds.Width - rootPopupBorder.Width;
                SettingsPopup.IsOpen = true;
            }

        }
        #endregion

        #region Actions Outside
        private void addPersonButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //addPersonButton_imageBrush.Opacity = 100;

            //ControlTemplate controlTemplate = addPersonButton.Template;
            //Image btnImage = (Image)controlTemplate.FindName("addPersonButton_image", addPersonButton);

            //Button buttonObj = (Button)e.OriginalSource;
            //this.GetTemplateChild("addPersonButton_image").Opacity = 100%;
        }

        private void addPersonButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //addPersonButton_imageBrush.Opacity = 75;
        }

        private async void updateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            await PersonDialog.ShowAsync();


        }

        private async void addPersonButton_Click(object sender, RoutedEventArgs e)
        {
            testInternetConnection();

            if (internetConnection)
            {
                thisPerson = new Person();

                var result = await PersonDialog.ShowAsync();

                // Value 1 indicates primary button was selected by the user, which adds a new Person
                if ((int)result == 1)
                {
                    //Apply the Data
                    showProgressDialog("Apply the Data...");
                    thisPerson.Name = PersonNameInput.Text;
                    thisPerson.Relation = PersonRelationInput.Text;
                    thisPerson.RiskFactor = (int)riskSlider.Value;
                    thisPerson.PatientId = PatientId;

                    //Upload Person Info
                    progressDialog.Content = "Upload Person Info...";
                    await AzureDatabaseService.UploadPersonInfo(thisPerson);
                    thisPerson = await AzureDatabaseService.GetPersonFromNameAndRelation(thisPerson.Name, thisPerson.Relation, thisPerson.PatientId);
                    //statusTextBlock.Text = "thisPerson.Id = " + thisPerson.Id;

                    //Upload Face Info
                    progressDialog.Content = "Upload Faces and Images...";
                    foreach (Face face in addFaceList)
                    {
                        face.PersonId = thisPerson.Id;
                    }
                    await AzureDatabaseService.UploadFaceInfo(thisPerson.Id, addFaceList);
                    foreach (Face face in deleteFaceList)
                    {
                        await AzureDatabaseService.DeleteFace(face);
                    }

                    //Add a Person Finished!
                    progressDialog.Content = "Add a Person Finished !";
                    progressDialog.IsPrimaryButtonEnabled = true;

                }//end result == 1

            }//end internetConnection





        }//end addPersonButton_Click

        private async void WarningImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //get the person info




            var result = await PersonDialog.ShowAsync();


            
        }

        #endregion


        #region Actions Inside the Dialog

        private void FamiliarRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                string tag = radioButton.Tag.ToString();
                switch (tag)
                {
                    case "Familiar":
                        thisPerson.IsFamiliar = true;
                        RiskGrid.Visibility = Visibility.Collapsed;
                        break;
                    case "Strange":
                        thisPerson.IsFamiliar = false;
                        RiskGrid.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void faceGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            thisFace = e.ClickedItem as Face;
            FaceImagePreview.Source = thisFace.Image;
        }

        private void faceGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Any())
            {
                GridViewItem gridViewFaceItem = faceGridView.ContainerFromItem(e.AddedItems[0]) as GridViewItem;
                gridViewFaceItem.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                gridViewFaceItem.Background.Opacity = 0.40;

                //Show the image preview
                thisFace = e.AddedItems.First() as Face;
                FaceImagePreview.Source = thisFace.Image;
                if (thisFace.IsDefault)
                {
                    SetDefaultButton.IsEnabled = false;
                }
                else
                {
                    SetDefaultButton.IsEnabled = true;
                }
            }

            if (e.RemovedItems != null && e.RemovedItems.Any())
            {
                GridViewItem gridViewFaceItem = faceGridView.ContainerFromItem(e.RemovedItems[0]) as GridViewItem;
                gridViewFaceItem.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }

        }


        private void riskSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            thisPerson.RiskFactor = (int)riskSlider.Value;
        }

        private async void AddNewFaceFromCamera_Click(object sender, RoutedEventArgs e)
        {
            // Exit if addItem is null
            //if (addFace == null)
            //    return;

            //Initialize
            thisFace = new Face();
            //FaceImagePreview.Source = nullBitmapImage;

            // Take by camera
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            StorageFile faceStorageFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            // User cancelled
            if (faceStorageFile == null)
                return;

            // Prepare the photo
            IRandomAccessStream irandom = await faceStorageFile.OpenAsync(FileAccessMode.Read);
            BitmapImage bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(irandom);

            string faToken = null;
            faToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(faceStorageFile);
            //DisplayTokenCorespondingFilePath(faToken); // for testing purposes

            // Set the Token and Add it to the list
            thisFace.ImageToken = faToken;
            thisFace.Image = bitmapImage;
            addFaceList.Add(thisFace);
            Faces.Add(thisFace);

            // View the image
            FaceImagePreview.Source = bitmapImage;
            //faceGridView.SelectedItem = 0;
            //faceGridView.SelectedIndex = Faces.Count - 1;
            
            //TODO: select this item or unselect all items


        }

        private async void AddNewFaceFromFile_Click(object sender, RoutedEventArgs e)
        {
            // Exit if addItem is null
            //if (addFace == null)
            //    return;

            //Initialize
            thisFace = new Face();
            //FaceImagePreview.Source = nullBitmapImage;

            // Get an image file to represent the item
            FileOpenPicker openFile = new FileOpenPicker();
            openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openFile.ViewMode = PickerViewMode.List;
            openFile.FileTypeFilter.Add(".jpg");
            openFile.FileTypeFilter.Add(".jpeg");
            openFile.FileTypeFilter.Add(".png");

            StorageFile faceStorageFile = await openFile.PickSingleFileAsync();
            string faToken = null;

            // If no item selected, quit
            if (faceStorageFile == null)
                return;

            // Prepare the selected image
            IRandomAccessStream irandom = await faceStorageFile.OpenAsync(FileAccessMode.Read);
            BitmapImage bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(irandom);

            // Add to FA without metadata
            faToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(faceStorageFile);
            //DisplayTokenCorespondingFilePath(faToken); // for testing purposes

            // Set the Token and Add it to the list
            thisFace.ImageToken = faToken;
            thisFace.Image = bitmapImage;
            addFaceList.Add(thisFace);
            Faces.Add(thisFace);

            // View the image
            FaceImagePreview.Source = bitmapImage;

            //TODO: select this item or unselect all items


        }

        private void deleteFaceButton_Click(object sender, RoutedEventArgs e)
        {
            Faces.Remove(thisFace);

            if (!addFaceList.Contains(thisFace))
            {
                deleteFaceList.Add(thisFace);
            }

            thisFace = new Face();
            FaceImagePreview.Source = nullBitmapImage;
        }

        private void SetDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear the Default Face
            Faces.Remove(thisFace);
            foreach (Face face in Faces)
            {
                face.IsDefault = false;
            }
            foreach (Face face in addFaceList)
            {
                face.IsDefault = false;
            }

            //Check whether it is in isInAddFaceList
            bool isInAddFaceList = false;

            if (addFaceList.Contains(thisFace))
            {
                isInAddFaceList = true;
                addFaceList.Remove(thisFace);
            }

            //Set the Default Face
            thisFace.IsDefault = true;
            Faces.Insert(0, thisFace);
            if (isInAddFaceList)
            {
                addFaceList.Insert(0, thisFace);
            }

            //Show the Image and disable the button
            defaultImage.Source = thisFace.Image;
            SetDefaultButton.IsEnabled = false;
        }

        private void CameraStreamingButton_Click(object sender, RoutedEventArgs e)
        {

        }

















        #endregion

        private async void testButton_Click(object sender, RoutedEventArgs e)
        {







        }
    }//end class
}
