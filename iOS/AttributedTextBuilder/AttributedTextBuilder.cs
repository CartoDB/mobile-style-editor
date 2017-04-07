
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace mobile_style_editor.iOS
{
	public class AttributedTextBuilder
	{
		public string Text { get; private set; }

		public List<AttributeSection> Attributes { get; private set; }

		public AttributedTextBuilder()
		{
			Text = "";
			Attributes = new List<AttributeSection>();
		}

		public AttributedTextBuilder Append(string text, UIColor color, float size)
		{
			Attributes.Add(new AttributeSection { Dictionary = GetColorAttributes(color), Range = new NSRange(Text.Length, text.Length) });
           	Text += text;
			return this;
		}

		public NSMutableAttributedString Build()
		{
			var result = new NSMutableAttributedString(Text);

			foreach (AttributeSection item in Attributes)
			{
				result.SetAttributes(item.Dictionary, item.Range);	
			}

			return result;
		}

		NSDictionary GetColorAttributes(UIColor color)
		{
			return new UIStringAttributes { ForegroundColor = color }.Dictionary;
		}
	}

	public class AttributeSection
	{
		public NSDictionary Dictionary { get; set; }

		public NSRange Range { get; set; }
	}
}
