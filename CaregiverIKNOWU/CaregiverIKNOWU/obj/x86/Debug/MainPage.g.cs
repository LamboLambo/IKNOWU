﻿#pragma checksum "C:\Users\LamboSAMA\Documents\GitHub\IKNOWU\CaregiverIKNOWU\CaregiverIKNOWU\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D07C52C9D013FE86C9E6A380F5808D5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaregiverIKNOWU
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_Panel_Background(global::Windows.UI.Xaml.Controls.Panel obj, global::Windows.UI.Xaml.Media.Brush value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.Brush) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.Brush), targetNullValue);
                }
                obj.Background = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(global::Windows.UI.Xaml.Controls.ToggleSwitch obj, global::System.Boolean value)
            {
                obj.IsOn = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Image_Source(global::Windows.UI.Xaml.Controls.Image obj, global::Windows.UI.Xaml.Media.ImageSource value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.ImageSource) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.ImageSource), targetNullValue);
                }
                obj.Source = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        private class MainPage_obj2_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::CaregiverIKNOWU.Models.Face dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Image obj3;
            private global::Windows.UI.Xaml.Controls.TextBlock obj4;

            public MainPage_obj2_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3:
                        this.obj3 = (global::Windows.UI.Xaml.Controls.Image)target;
                        break;
                    case 4:
                        this.obj4 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 global::CaregiverIKNOWU.Models.Face data = args.NewValue as global::CaregiverIKNOWU.Models.Face;
                 if (args.NewValue != null && data == null)
                 {
                    throw new global::System.ArgumentException("Incorrect type passed into template. Based on the x:DataType global::CaregiverIKNOWU.Models.Face was expected.");
                 }
                 this.SetDataRoot(data);
                 this.Update();
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                switch(args.Phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(args.Item as global::CaregiverIKNOWU.Models.Face);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            ((global::Windows.UI.Xaml.Controls.StackPanel)args.ItemContainer.ContentTemplateRoot).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::CaregiverIKNOWU.Models.Face) args.Item, 1 << (int)args.Phase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            // MainPage_obj2_Bindings

            public void SetDataRoot(global::CaregiverIKNOWU.Models.Face newDataRoot)
            {
                this.dataRoot = newDataRoot;
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::CaregiverIKNOWU.Models.Face obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Image(obj.Image, phase);
                        this.Update_UpdatedAt(obj.UpdatedAt, phase);
                    }
                }
            }
            private void Update_Image(global::Windows.UI.Xaml.Media.Imaging.BitmapImage obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Image_Source(this.obj3, obj, null);
                }
            }
            private void Update_UpdatedAt(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj4, obj, null);
                }
            }
        }

        private class MainPage_obj5_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::CaregiverIKNOWU.Models.Person dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Image obj6;
            private global::Windows.UI.Xaml.Controls.TextBlock obj7;
            private global::Windows.UI.Xaml.Controls.TextBlock obj8;

            public MainPage_obj5_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 6:
                        this.obj6 = (global::Windows.UI.Xaml.Controls.Image)target;
                        break;
                    case 7:
                        this.obj7 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 8:
                        this.obj8 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 global::CaregiverIKNOWU.Models.Person data = args.NewValue as global::CaregiverIKNOWU.Models.Person;
                 if (args.NewValue != null && data == null)
                 {
                    throw new global::System.ArgumentException("Incorrect type passed into template. Based on the x:DataType global::CaregiverIKNOWU.Models.Person was expected.");
                 }
                 this.SetDataRoot(data);
                 this.Update();
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                switch(args.Phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(args.Item as global::CaregiverIKNOWU.Models.Person);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            ((global::Windows.UI.Xaml.Controls.StackPanel)args.ItemContainer.ContentTemplateRoot).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::CaregiverIKNOWU.Models.Person) args.Item, 1 << (int)args.Phase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            // MainPage_obj5_Bindings

            public void SetDataRoot(global::CaregiverIKNOWU.Models.Person newDataRoot)
            {
                this.dataRoot = newDataRoot;
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::CaregiverIKNOWU.Models.Person obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_DefaultIcon(obj.DefaultIcon, phase);
                        this.Update_Relation(obj.Relation, phase);
                        this.Update_Name(obj.Name, phase);
                    }
                }
            }
            private void Update_DefaultIcon(global::Windows.UI.Xaml.Media.Imaging.BitmapImage obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Image_Source(this.obj6, obj, null);
                }
            }
            private void Update_Relation(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj7, obj, null);
                }
            }
            private void Update_Name(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj8, obj, null);
                }
            }
        }

        private class MainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::CaregiverIKNOWU.MainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Grid obj11;
            private global::Windows.UI.Xaml.Controls.ToggleSwitch obj18;
            private global::Windows.UI.Xaml.Controls.GridView obj32;
            private global::Windows.UI.Xaml.Controls.GridView obj46;

            private MainPage_obj1_BindingsTracking bindingsTracking;

            public MainPage_obj1_Bindings()
            {
                this.bindingsTracking = new MainPage_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 11:
                        this.obj11 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 18:
                        this.obj18 = (global::Windows.UI.Xaml.Controls.ToggleSwitch)target;
                        (this.obj18).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.ToggleSwitch.IsOnProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.AppSettings.UseBingImageOfTheDay = (this.obj18).IsOn;
                                }
                            });
                        break;
                    case 32:
                        this.obj32 = (global::Windows.UI.Xaml.Controls.GridView)target;
                        break;
                    case 46:
                        this.obj46 = (global::Windows.UI.Xaml.Controls.GridView)target;
                        break;
                    default:
                        break;
                }
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // MainPage_obj1_Bindings

            public void SetDataRoot(global::CaregiverIKNOWU.MainPage newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::CaregiverIKNOWU.MainPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_AppSettings(obj.AppSettings, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Faces(obj.Faces, phase);
                        this.Update_Persons(obj.Persons, phase);
                    }
                }
            }
            private void Update_AppSettings(global::CaregiverIKNOWU.Controls.Settings obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_AppSettings(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_AppSettings_WallPaper(obj.WallPaper, phase);
                        this.Update_AppSettings_UseBingImageOfTheDay(obj.UseBingImageOfTheDay, phase);
                    }
                }
            }
            private void Update_AppSettings_WallPaper(global::Windows.UI.Xaml.Media.Imaging.BitmapImage obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Panel_Background(this.obj11, (global::Windows.UI.Xaml.Media.Brush)this.LookupConverter("BitmapToBrushConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.Brush), null, null), null);
                }
            }
            private void Update_AppSettings_UseBingImageOfTheDay(global::System.Boolean obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(this.obj18, obj);
                }
            }
            private void Update_Faces(global::System.Collections.ObjectModel.ObservableCollection<global::CaregiverIKNOWU.Models.Face> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj32, obj, null);
                }
            }
            private void Update_Persons(global::System.Collections.ObjectModel.ObservableCollection<global::CaregiverIKNOWU.Models.Person> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj46, obj, null);
                }
            }

            private class MainPage_obj1_BindingsTracking
            {
                global::System.WeakReference<MainPage_obj1_Bindings> WeakRefToBindingObj; 

                public MainPage_obj1_BindingsTracking(MainPage_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<MainPage_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_AppSettings(null);
                }

                public void PropertyChanged_AppSettings(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::CaregiverIKNOWU.Controls.Settings obj = sender as global::CaregiverIKNOWU.Controls.Settings;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_AppSettings_WallPaper(obj.WallPaper, DATA_CHANGED);
                                    bindings.Update_AppSettings_UseBingImageOfTheDay(obj.UseBingImageOfTheDay, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "WallPaper":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_AppSettings_WallPaper(obj.WallPaper, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "UseBingImageOfTheDay":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_AppSettings_UseBingImageOfTheDay(obj.UseBingImageOfTheDay, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::CaregiverIKNOWU.Controls.Settings cache_AppSettings = null;
                public void UpdateChildListeners_AppSettings(global::CaregiverIKNOWU.Controls.Settings obj)
                {
                    if (obj != cache_AppSettings)
                    {
                        if (cache_AppSettings != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_AppSettings).PropertyChanged -= PropertyChanged_AppSettings;
                            cache_AppSettings = null;
                        }
                        if (obj != null)
                        {
                            cache_AppSettings = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_AppSettings;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 9:
                {
                    this.Add = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                }
                break;
            case 10:
                {
                    this.SettingsButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 78 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.SettingsButton).Tapped += this.SettingsButton_Tapped;
                    #line default
                }
                break;
            case 11:
                {
                    this.rootGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 12:
                {
                    this.PersonDialog = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                }
                break;
            case 13:
                {
                    this.SettingsPopup = (global::Windows.UI.Xaml.Controls.Primitives.Popup)(target);
                }
                break;
            case 14:
                {
                    this.testButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 286 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.testButton).Click += this.testButton_Click;
                    #line default
                }
                break;
            case 15:
                {
                    this.testImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 16:
                {
                    this.rootPopupBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 17:
                {
                    this.patientNameTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 18:
                {
                    this.BingWallpaperToggle = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                }
                break;
            case 19:
                {
                    this.PersonDialogPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 20:
                {
                    this.BasicInfoGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 21:
                {
                    this.RiskGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 22:
                {
                    this.FaceViewGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 23:
                {
                    this.ImagePreviewGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 24:
                {
                    this.SetDefaultButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 230 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SetDefaultButton).Click += this.SetDefaultButton_Click;
                    #line default
                }
                break;
            case 25:
                {
                    this.VideoStackPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 26:
                {
                    this.CameraStreamingButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 233 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.CameraStreamingButton).Click += this.CameraStreamingButton_Click;
                    #line default
                }
                break;
            case 27:
                {
                    this.CamPreview = (global::Windows.UI.Xaml.Controls.CaptureElement)(target);
                }
                break;
            case 28:
                {
                    this.VisualizationCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 29:
                {
                    this.TimerCounterBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 30:
                {
                    this.FaceImagePreview = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 31:
                {
                    this.deleteFaceButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 228 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.deleteFaceButton).Click += this.deleteFaceButton_Click;
                    #line default
                }
                break;
            case 32:
                {
                    this.faceGridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 209 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.faceGridView).ItemClick += this.faceGridView_ItemClick;
                    #line 209 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.faceGridView).SelectionChanged += this.faceGridView_SelectionChanged;
                    #line default
                }
                break;
            case 33:
                {
                    this.AddNewFaceFromCamera = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 219 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddNewFaceFromCamera).Click += this.AddNewFaceFromCamera_Click;
                    #line default
                }
                break;
            case 34:
                {
                    this.AddNewFaceFromFile = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 223 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddNewFaceFromFile).Click += this.AddNewFaceFromFile_Click;
                    #line default
                }
                break;
            case 35:
                {
                    this.riskSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 187 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.riskSlider).ValueChanged += this.riskSlider_ValueChanged;
                    #line default
                }
                break;
            case 36:
                {
                    this.sendWarningButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 188 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.sendWarningButton).Click += this.sendWarningButton_Click;
                    #line default
                }
                break;
            case 37:
                {
                    this.sendWarningTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 38:
                {
                    this.defaultImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 39:
                {
                    this.PersonNameInput = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 40:
                {
                    this.PersonRelationInput = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 41:
                {
                    this.familiarRadioButton = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 178 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.familiarRadioButton).Checked += this.FamiliarRadioButton_Checked;
                    #line default
                }
                break;
            case 42:
                {
                    this.strangeRadioButton = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 179 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.strangeRadioButton).Checked += this.FamiliarRadioButton_Checked;
                    #line default
                }
                break;
            case 43:
                {
                    this.faceDatabase = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 44:
                {
                    this.WarningImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 138 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.WarningImage).Tapped += this.WarningImage_Tapped;
                    #line default
                }
                break;
            case 45:
                {
                    this.statusTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 46:
                {
                    this.personGridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 119 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.personGridView).ItemClick += this.personGridView_ItemClick;
                    #line 119 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.personGridView).SelectionChanged += this.personGridView_SelectionChanged;
                    #line default
                }
                break;
            case 47:
                {
                    this.addPersonButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 125 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.addPersonButton).PointerPressed += this.addPersonButton_PointerPressed;
                    #line 125 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.addPersonButton).PointerReleased += this.addPersonButton_PointerReleased;
                    #line 125 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.addPersonButton).Click += this.addPersonButton_Click;
                    #line default
                }
                break;
            case 48:
                {
                    this.addPersonButton_imageBrush = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    MainPage_obj1_Bindings bindings = new MainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    bindings.SetConverterLookupRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.StackPanel element2 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                    MainPage_obj2_Bindings bindings = new MainPage_obj2_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot((global::CaregiverIKNOWU.Models.Face) element2.DataContext);
                    element2.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element2, bindings);
                }
                break;
            case 5:
                {
                    global::Windows.UI.Xaml.Controls.StackPanel element5 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                    MainPage_obj5_Bindings bindings = new MainPage_obj5_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot((global::CaregiverIKNOWU.Models.Person) element5.DataContext);
                    element5.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element5, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}
