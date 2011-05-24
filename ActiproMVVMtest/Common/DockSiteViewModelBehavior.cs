using System;
using System.Windows;
using ActiproSoftware.Windows.Controls.Docking;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.Common
{

	/// <summary>
	/// Provides attached behaviors for <see cref="DockSite"/> that properly initializes/opens windows associated with view-models.
	/// </summary>
	public static class DockSiteViewModelBehavior {

		#region Dependency Properties

		/// <summary>
		/// Identifies the <c>IsManaged</c> attached dependency property.  This field is read-only.
		/// </summary>
		/// <value>The identifier for the <c>IsManaged</c> attached dependency property.</value>
		public static readonly DependencyProperty IsManagedProperty = DependencyProperty.RegisterAttached("IsManaged",
			typeof(bool), typeof(DockSiteViewModelBehavior), new FrameworkPropertyMetadata(false, OnIsManagedPropertyValueChanged));

		#endregion // Dependency Properties

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// NON-PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets the first <see cref="ToolWindow"/> associated with the specified dock group.
		/// </summary>
		/// <param name="dockSite">The dock site to search.</param>
		/// <param name="dockGroup">The dock group.</param>
		/// <returns>
		/// A <see cref="ToolWindow"/>; otherwise, <see langword="null"/>.
		/// </returns>
		private static ToolWindow GetToolWindow(DockSite dockSite, string dockGroup) {
			if (dockSite != null && !string.IsNullOrEmpty(dockGroup)) {
				foreach (ToolWindow toolWindow in dockSite.ToolWindows) {
					ToolItemViewModel toolItemViewModel = toolWindow.DataContext as ToolItemViewModel;
					if (toolItemViewModel != null && toolItemViewModel.DockGroup == dockGroup)
						return toolWindow;
				}
			}

			return null;
		}

		/// <summary>
		/// Handles the <c>WindowRegistered</c> event of the <c>DockSite</c> control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="DockingWindowEventArgs"/> instance containing the event data.</param>
		private static void OnDockSiteWindowRegistered(object sender, DockingWindowEventArgs e) {
			DockSite dockSite = sender as DockSite;
			if (dockSite == null)
				return;

			// Ensure the DockingWindow exists and is generated for an item
			DockingWindow dockingWindow = e.Window;
			if (dockingWindow == null || !dockingWindow.IsContainerForItem)
				return;

			// Pass down the name, if any as this cannot be done via a Style
			if (string.IsNullOrEmpty(dockingWindow.Name)) {
				ViewModelBase viewModel = dockingWindow.DataContext as ViewModelBase;
				if (viewModel != null && !string.IsNullOrEmpty(viewModel.Name))
					dockingWindow.Name = viewModel.Name;
			}

			// Open the DockingWindow, if it's not already open
			if (!dockingWindow.IsOpen) {
				if (dockingWindow is DocumentWindow)
					dockingWindow.Open();
				else {
					ToolWindow toolWindow = dockingWindow as ToolWindow;
					ToolItemViewModel toolItemViewModel = dockingWindow.DataContext as ToolItemViewModel;
					if (toolWindow != null && toolItemViewModel != null) {
						// Look for a ToolWindow within the same group, if found then dock to that group, otherwise either dock or auto-hide the window
						ToolWindow targetToolWindow = GetToolWindow(dockSite, toolItemViewModel.DockGroup);
						if (targetToolWindow != null && targetToolWindow != toolWindow)
							toolWindow.Dock(targetToolWindow, Direction.Content);
						else if (toolItemViewModel.IsInitiallyAutoHidden)
							toolWindow.AutoHide(toolItemViewModel.DefaultDock);
						else
							toolWindow.Dock(dockSite, toolItemViewModel.DefaultDock);
					}
					else {
						dockingWindow.Open();
					}
				}
			}
		}

        /// <summary>
        /// Handles the <c>WindowActivated</c> event of the <c>DockSite</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DockingWindowEventArgs"/> instance containing the event data.</param>
        private static void OnDockSiteWindowActivated(object sender, DockingWindowEventArgs e)
        {
            DockSite dockSite = sender as DockSite;
            if (dockSite == null)
                return;

            // Ensure the DockingWindow exists and is generated for an item
            DockingWindow dockingWindow = e.Window;
            if (dockingWindow == null || !dockingWindow.IsContainerForItem)
                return;

            ToolWindowCollection toolWindows = dockSite.ToolWindows;
            foreach (ToolWindow tw in toolWindows)
            {
                if (tw.Name == "toolWindow2")
                {
                    if (dockingWindow is DocumentWindow)
                        tw.Title = dockingWindow.Title;
                    else if (dockingWindow is ToolWindow)
                        tw.Title = dockingWindow.Name;
                    else
                        tw.Title = "problem";
                }
            }

        }

        /// <summary>
		/// Called when <see cref="IsManagedProperty"/> is changed.
		/// </summary>
		/// <param name="d">The dependency object that was changed.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnIsManagedPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			DockSite dockSite = d as DockSite;
			if (dockSite == null)
				return;

			// Add handler for WindowRegistered event, which will allow us to open/position generated windows
            if ((bool)e.NewValue)
            {
                dockSite.WindowRegistered += DockSiteViewModelBehavior.OnDockSiteWindowRegistered;
                // dockSite.WindowActivated += DockSiteViewModelBehavior.OnDockSiteWindowActivated;
            }
            else
            {
                dockSite.WindowRegistered -= DockSiteViewModelBehavior.OnDockSiteWindowRegistered;
                // dockSite.WindowActivated -= DockSiteViewModelBehavior.OnDockSiteWindowActivated;
            }
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets the value of the <see cref="IsManagedProperty"/> attached property for a specified <see cref="DockSite"/>.
		/// </summary>
		/// <param name="obj">The object to which the attached property is retrieved.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="DockSite"/> is being managed; otherwise <c>false</c>.
		/// </returns>
		public static bool GetIsManaged(DockSite obj) {
			if (null == obj) throw new ArgumentNullException("obj");
			return (bool)obj.GetValue(DockSiteViewModelBehavior.IsManagedProperty);
		}
		/// <summary>
		/// Sets the value of the <see cref="IsManagedProperty"/> attached property to a specified <see cref="DockSite"/>.
		/// </summary>
		/// <param name="obj">The object to which the attached property is written.</param>
		/// <param name="value">
		/// A value indicating whether the specified <see cref="DockSite"/> is being managed.
		/// </param>
		public static void SetIsManaged(DockSite obj, bool value) {
			if (null == obj) throw new ArgumentNullException("obj");
			obj.SetValue(DockSiteViewModelBehavior.IsManagedProperty, value);
		}

	}
}
