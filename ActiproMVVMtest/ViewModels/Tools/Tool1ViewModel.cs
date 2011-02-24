﻿using System;
using System.Windows.Media.Imaging;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a tool view-model for the sample.
	/// </summary>
	public class Tool1ViewModel : ToolItemViewModel {

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="Tool1ViewModel"/> class.
		/// </summary>
		public Tool1ViewModel() {
			this.ImageSource = new BitmapImage(new Uri("/Resources/Images/Toolbox16.png", UriKind.Relative));
			this.Name = "toolWindow1";
			this.Title = "Tool 1";
		}

	}
}
