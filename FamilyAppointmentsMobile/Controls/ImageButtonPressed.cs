using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace FamilyAppointmentsMobile.Controls  
{
    public class ImageButtonPressed : ContentView
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButtonPressed));

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageButtonPressed));

        public static readonly BindableProperty AnimateProperty =
            BindableProperty.Create(nameof(Animate), typeof(bool), typeof(ImageButtonPressed), false);

        public static readonly BindableProperty EnabledProperty =
            BindableProperty.Create(nameof(Enabled), typeof(bool), typeof(ImageButtonPressed), true);

        public static readonly BindableProperty SideHitAreaProperty =
            BindableProperty.Create(nameof(SideHitArea), typeof(Thickness), typeof(ImageButtonPressed));

        public static readonly BindableProperty ImageSourceProperty =
           BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ImageButtonPressed), null, BindingMode.TwoWay);

        private ICommand animatePressCommand;
        private Image resourceImage;
        private Grid grid;

        public ImageButtonPressed()
        {
            Initialize();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public bool Animate
        {
            get { return (bool)GetValue(AnimateProperty); }
            set { SetValue(AnimateProperty, value); }
        }

        public Thickness SideHitArea
        {
            get { return (Thickness)GetValue(SideHitAreaProperty); }
            set { SetValue(SideHitAreaProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public ICommand AnimatePressCommand =>
            animatePressCommand
            ?? (animatePressCommand = new RelayCommand(() => ToggleAction()));

        private void ToggleAction()
        {
            if (!Enabled) { return; }

            if (Animate)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await grid.ScaleTo(0.6, 70, Easing.Linear);
                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                    }
                    await Task.Delay(40);
                    await grid.ScaleTo(1, 70, Easing.Linear);
                });
            }
            else
            {
                if (Command != null)
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        private void Initialize()
        {
            resourceImage = new Image()
            {
                Margin = new Thickness(4),
            };

            Animate = true;


            grid = new Grid();
            grid.Padding = SideHitArea;
            grid.GestureRecognizers.Add(new TapGestureRecognizer { Command = AnimatePressCommand });
            grid.Children.Add(resourceImage);

            Content = grid;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            (Content as Grid).Padding = SideHitArea;
            HeightRequest = HeightRequest + SideHitArea.VerticalThickness;
            WidthRequest = WidthRequest + SideHitArea.HorizontalThickness;

            SetImageView();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged();
            if (propertyName == null) return;
            if (Equals(propertyName, nameof(ImageSource)))
            {
                SetImageView();
            }               
        }

        private void SetImageView()
        {
            if (ImageSource != null)
            {
                resourceImage.HeightRequest = this.Height;
                resourceImage.WidthRequest = this.Width;
                resourceImage.Source = ImageSource;      
            }
        }
    }
}