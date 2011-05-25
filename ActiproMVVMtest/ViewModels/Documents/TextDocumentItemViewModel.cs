using System;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a text view-model for the sample.
	/// </summary>
	public class TextDocumentItemViewModel : DocumentItemViewModel {

		private static int counter = 1;
		private string text;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="TextDocumentItemViewModel"/> class.
		/// </summary>
		public TextDocumentItemViewModel() {
			this.Title = string.Format("Document {0}", counter++);
			this.Text = string.Format("Dynamically created at {0}", DateTime.Now);
			this.Description = "Text document";
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets or sets the text associated with the view-model.
		/// </summary>
		/// <value>The text associated with the view-model.</value>
		public string Text {
			get {
				return this.text;
			}
			set {
				if (this.text != value) {
					this.text = value;
					this.NotifyPropertyChanged("Text");
				}
			}
		}

	}
}
