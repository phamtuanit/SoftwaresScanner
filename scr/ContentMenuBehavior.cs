#region Reference

using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

#endregion

namespace SoftwaresAnalyze
{
    public class DropDownButtonBehavior : Behavior<ButtonBase>
    {
        private long attachedCount;

        private bool isContextMenuOpen;

        /// <summary>
        ///     Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.AssociatedObject_Click), true);
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            var source = sender as ButtonBase;
            if (source != null && source.ContextMenu != null)
            {
                // Only open the ContextMenu when it is not already open. If it is already open,
                // when the button is pressed the ContextMenu will lose focus and automatically close.
                if (!this.isContextMenuOpen)
                {
                    source.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(this.ContextMenu_Closed),
                        true);
                    Interlocked.Increment(ref this.attachedCount);
                    // If there is a drop-down assigned to this button, then position and display it 
                    source.ContextMenu.PlacementTarget = source;
                    source.ContextMenu.Placement = PlacementMode.Bottom;
                    source.ContextMenu.IsOpen = true;
                    this.isContextMenuOpen = true;
                }
            }
        }

        /// <summary>
        ///     Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.AssociatedObject_Click));
        }

        /// <summary>
        ///     Handles the Closed event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            this.isContextMenuOpen = false;
            var contextMenu = sender as ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(this.ContextMenu_Closed));
                Interlocked.Decrement(ref this.attachedCount);
            }
        }
    }
}