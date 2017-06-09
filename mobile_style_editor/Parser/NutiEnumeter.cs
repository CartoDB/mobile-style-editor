
using System;
using System.Collections.Generic;

namespace mobile_style_editor
{
    public enum NutiEnum
    {
        texts3d,
        markerd3d,
        shields3d,
        buildings3d,
        _pixelscale,
        _fontscale,
        lang
    }

    public class NutiParameter
    {
        public NutiEnum Type { get; set; }

        public bool IsBoolean { get { return Key.Contains("3d"); } }

        public bool IsScale { get { return Key.Contains("scale"); } }

        public bool IsLanguage { get { return Key.Contains("lang"); } }

        public string Key { get { return Type.ToString(); } }

		public object DefaultValue { get; set; }

        public Dictionary<string, object> Values { get; set; }

        public NutiParameter()
        {
            Values = new Dictionary<string, object>();
        }
	}
}
