#region Reference

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#endregion

namespace SoftwaresAnalyze
{
    public class TextBoxDoubleClickBehavior : Behavior<TextBox>
    {
        /// <summary>
        ///     Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.AddHandler(Control.PreviewMouseDoubleClickEvent,
                new RoutedEventHandler((sender, args) => { this.AssociatedObject.IsReadOnly = !this.AssociatedObject.IsReadOnly; }),
                true);
        }
    }
}