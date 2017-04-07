using System;
using Android.Text;

namespace mobile_style_editor.Droid
{
	public class SpanSection
	{
		string text;
		int startIndex;
		IParcelableSpan[] spans;

		public SpanSection(string text, int startIndex, params IParcelableSpan[] spans)
		{
			this.spans = spans;
			this.text = text;
			this.startIndex = startIndex;
		}

		public void Apply(SpannableStringBuilder spanStringBuilder)
		{
			if (spanStringBuilder == null)
			{
				return;
			}

			foreach (IParcelableSpan span in spans)
			{

				spanStringBuilder.SetSpan((Java.Lang.Object)span, startIndex, startIndex + text.Length, SpanTypes.InclusiveInclusive);
			}
		}

	}
}