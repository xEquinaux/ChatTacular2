using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Chat_tacular.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool authFlag
	{
		get
		{
			return (bool)this["authFlag"];
		}
		set
		{
			this["authFlag"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string authCode
	{
		get
		{
			return (string)this["authCode"];
		}
		set
		{
			this["authCode"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string programAuth
	{
		get
		{
			return (string)this["programAuth"];
		}
		set
		{
			this["programAuth"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("640, 480")]
	public Point chatSize
	{
		get
		{
			return (Point)this["chatSize"];
		}
		set
		{
			this["chatSize"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	public Color chatBgColor
	{
		get
		{
			return (Color)this["chatBgColor"];
		}
		set
		{
			this["chatBgColor"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string chatFontName
	{
		get
		{
			return (string)this["chatFontName"];
		}
		set
		{
			this["chatFontName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int chatFontSize
	{
		get
		{
			return (int)this["chatFontSize"];
		}
		set
		{
			this["chatFontSize"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string loginChannel
	{
		get
		{
			return (string)this["loginChannel"];
		}
		set
		{
			this["loginChannel"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string loginName
	{
		get
		{
			return (string)this["loginName"];
		}
		set
		{
			this["loginName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string bgFontColor
	{
		get
		{
			return (string)this["bgFontColor"];
		}
		set
		{
			this["bgFontColor"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string fgFontColor
	{
		get
		{
			return (string)this["fgFontColor"];
		}
		set
		{
			this["fgFontColor"] = value;
		}
	}
}
