using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FamilyAppointmentsMobile.Helpers;

namespace FamilyAppointmentsMobile.Controls
{
    public class DefaultEffectButton : ContentView
    {
        public event EventHandler Clicked;

        #region Bindable propertys

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(DefaultEffectButton), null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(DefaultEffectButton), null);

        public static readonly BindableProperty ButtonTextProperty =
           BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(DefaultEffectButton), string.Empty);

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(DefaultEffectButton), null);

        public static readonly BindableProperty ButtonBackgroundProperty = BindableProperty.Create(nameof(ButtonBackground), typeof(Color), typeof(DefaultEffectButton), ResourceHelper.GetResource<Color>("PinButtonBackgroundColor"));

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

        public Color ButtonBackground
        {
            get { return (Color) GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        #endregion 

        private Border mainBorder;
        private Label label;
        private Image image;
        private TouchBehavior touchBehavior;

        public DefaultEffectButton()
        {
            InitContent();
           
        }

        

        private void InitContent()
        {
            mainBorder = new Border
            {
                StrokeThickness = 1,
                //HeightRequest = 56,
                Stroke = ResourceHelper.GetResource<Color>("PinButtonBorderColor"),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) },
            };
            var grid = new Grid
            {

            };

            label = new Label
            {
                BackgroundColor = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation,
                TextColor = Colors.White,
            };

            image = new Image
            {
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(6)
            };

            grid.Children.Add(label);
            grid.Children.Add(image);

            touchBehavior = new TouchBehavior
            {
                PressedOpacity = 0.6,
                PressedScale = 0.97,
                PressedBackgroundColor = Colors.LightGray
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => OnClicked();
            mainBorder.GestureRecognizers.Add(tapGestureRecognizer);

            mainBorder.Content = grid;
            mainBorder.Behaviors.Add(touchBehavior);
            Content = mainBorder;
        }

        protected virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == null) return;
           
            if (propertyName == nameof(Command))
            {
                touchBehavior.Command = this.Command;
            }
            if (propertyName == nameof(CommandParameter))
            {
                touchBehavior.CommandParameter = this.CommandParameter;
            }
            if (propertyName == nameof(ButtonText))
            {
                label.Text = this.ButtonText;
                HandleButtonTextVisibility();
            }
            if (propertyName == nameof(ImageSource))
            {
                image.Source = ImageSource;
                HandleImageVisibility();
            }
            if (propertyName == nameof(ButtonBackground))
            {
                mainBorder.Background = ButtonBackground;
            }
        }

        private void HandleImageVisibility()
        {
            if (string.IsNullOrEmpty(ButtonText))
            {
                image.IsVisible = true;
                label.IsVisible = false;
            }
            else
            {
                image.IsVisible = false;
                label.IsVisible = true;
            }
        }

        private void HandleButtonTextVisibility()
        {
            if (ImageSource == null)
            {
                label.IsVisible = true;
                image.IsVisible = false;
            }
            else
            {
                label.IsVisible = false;
                image.IsVisible = true;
            }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            touchBehavior.Command = this.Command;
            touchBehavior.CommandParameter = this.CommandParameter;
            mainBorder.Background = ButtonBackground;
            label.Text = this.ButtonText;
            label.TextColor = ResourceHelper.GetResource<Color>("PinTextColor");
            image.Source = ImageSource;
            HandleButtonTextVisibility();
            HandleImageVisibility();
        }
    }
}
