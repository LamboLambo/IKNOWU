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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CaregiverIKNOWU
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Initializations
        
        private Settings AppSettings { get; set; }

        // Data and view model list of items
        private ObservableCollection<Person> Persons;
        private ObservableCollection<Face> Faces;




        public MainPage()
        {
            this.InitializeComponent();

            // Initialize the app's settings.
            AppSettings = new Settings();
            AppSettings.LoadSettings();

            //Initialize the UI
            initializeWarningStatus();





        }





        #endregion


        #region Methods
        private void initializeWarningStatus()
        {
            WarningImage.Visibility = Visibility.Collapsed;
            RiskGrid.Visibility = Visibility.Collapsed;
            VideoStackPanel.Visibility = Visibility.Collapsed;
        }

        private void createWarningInfo()
        {
            WarningImage.Visibility = Visibility.Visible;
            RiskGrid.Visibility = Visibility.Visible;
            VideoStackPanel.Visibility = Visibility.Visible;
        }




        #endregion


        #region Actions
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
            await PersonDialog.ShowAsync();
        }

        private void faceGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void faceGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WarningImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void riskSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void FamiliarRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CameraStreamingButton_Click(object sender, RoutedEventArgs e)
        {

        }











        #endregion
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
    }//end class
}
