using System;
using Xamarin.Forms;

namespace DnB_Xamarin_V2.ThemesAndStyle
{
    internal sealed class ToggleButton : Button
    {
        public event EventHandler<ToggledEventArgs> Toggled;

        public static BindableProperty IsToggledProperty = BindableProperty
                            .Create("IsToggled", typeof(bool), typeof(ToggleButton), false, propertyChanged: OnIsToggledChanged);

        public ToggleButton()
        {
            Clicked += (sender, args) => IsToggled ^= true;
        }

        public bool IsToggled
        {
            set { SetValue(IsToggledProperty, value); }
            get { return (bool)GetValue(IsToggledProperty); }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            VisualStateManager.GoToState(this, "ToggledOff");
        }

        static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton toggleButton = (ToggleButton)bindable;
            bool isToggled = (bool)newValue;

            toggleButton.Toggled?.Invoke(toggleButton, new ToggledEventArgs(isToggled));

            VisualStateManager.GoToState(toggleButton, isToggled ? "ToggledOn" : "ToggledOff");
        }
    }
}