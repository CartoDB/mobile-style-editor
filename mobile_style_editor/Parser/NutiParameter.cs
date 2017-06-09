
using System;
using System.Collections.Generic;
using System.Linq;
using Carto.Core;

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
        lang,
        source
    }

    public class NutiConst
    {
        public const string Parameters = "nutiparameters";

        public const string DefaultValue = "default";

        public const string Values = "values";

        public const string NullValue = "null";

        public static readonly List<NutiEnum> ParameterList = Enum.GetValues(typeof(NutiEnum)).Cast<NutiEnum>().ToList();
    }

    public class NutiParameter
    {
        public NutiEnum Type { get; set; }

		public object DefaultValue { get; set; }

		public Dictionary<string, object> Values { get; set; }

		public string Key { get { return Type.ToString(); } }

		public bool IsBoolean { get { return Key.Contains("3d"); } }

        public bool IsScale { get { return Key.Contains("scale"); } }

        public bool IsLanguage { get { return Key.Contains("lang"); } }

        public bool IsSource { get { return Key.Contains("source"); } }

        public NutiParameter()
        {
            Values = new Dictionary<string, object>();
        }

        public Variant ValueJson
        {
            get {
				var parameters = new StringVariantMap();

				if (DefaultValue is string)
				{
					parameters.Add(NutiConst.DefaultValue, new Variant((string)DefaultValue));
				}
				else if (DefaultValue is double)
				{
					parameters.Add(NutiConst.DefaultValue, new Variant((double)DefaultValue));
				}
				else if (DefaultValue is bool)
				{
					parameters.Add(NutiConst.DefaultValue, new Variant((bool)DefaultValue));
				}

				if (!IsScale)
				{
					var values = new StringVariantMap();

					foreach (var item in Values)
					{
						// There are currently no double-valued nutiparameters with a values array
						if (item.Value is string)
						{
							values.Add(item.Key, new Variant((string)item.Value));
						}
						else if (item.Value is bool)
						{
							values.Add(item.Key, new Variant((bool)item.Value));
						}
					}

					parameters.Add(NutiConst.Values, new Variant(values));
				}

                return new Variant(parameters);
            }
        }

	}
}
