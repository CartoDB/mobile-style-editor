
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace mobile_style_editor.iOS
{
	public class AttributedTextBuilder
	{
		public string Text { get; private set; }

		public Dictionary<NSDictionary, NSRange> Attributes { get; private set; }

		public AttributedTextBuilder()
		{
			Text = "";
			Attributes = new Dictionary<NSDictionary, NSRange>();
		}

		public AttributedTextBuilder Append(string text, UIColor color, float size)
		{
			Text += text;
			//Attributes.Add(GetColorAttributes(color), new NSRange(0
			return this;	
		}

		public NSMutableAttributedString Build()
		{
			var result = new NSMutableAttributedString(Text);

			foreach (KeyValuePair<NSDictionary, NSRange> item in Attributes)
			{
				result.SetAttributes(item.Key, item.Value);	
			}

			return result;
		}

		NSDictionary GetColorAttributes(UIColor color)
		{
			return new UIStringAttributes { ForegroundColor = color }.Dictionary;
		}
	}
}
