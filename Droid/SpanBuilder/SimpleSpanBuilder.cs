using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Java.Lang;

namespace mobile_style_editor.Droid
{
	public class SimpleSpanBuilder
	{
		List<SpanSection> spanSections;
		StringBuilder stringBuilder;

		public SimpleSpanBuilder()
		{
			spanSections = new List<SpanSection>();
			stringBuilder = new StringBuilder();
		}

		public SimpleSpanBuilder Append(string text, params IParcelableSpan[] spans)
		{
			if (spans != null && spans.Length > 0)
			{
				spanSections.Add(new SpanSection(text, stringBuilder.Length(), spans));
			}
			stringBuilder.Append(text);
			return this;
		}

		public SimpleSpanBuilder Append(string text, Color color, float size)
		{
			return Append(text, new ForegroundColorSpan(color), new RelativeSizeSpan(size));
		}

		public SpannableStringBuilder Build()
		{
			SpannableStringBuilder builder = new SpannableStringBuilder(stringBuilder.ToString());

			foreach (SpanSection section in spanSections)
			{
				section.Apply(builder);
			}

			return builder;
		}

		public override string ToString()
		{
			return stringBuilder.ToString();
		}
	}
}
