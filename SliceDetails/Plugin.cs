using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Logging;
using SiraUtil.Zenject;
using SliceDetails.Installers;
using SliceDetails.Settings;

namespace SliceDetails;

[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
public class Plugin
{
	internal static SettingsStore Settings { get; private set; } = null!;

	[Init]
	public Plugin(Logger logger, Config config, Zenjector zenjector) 
	{
		Settings = config.Generated<SettingsStore>();

		zenjector.UseLogger(logger);
		zenjector.Install<SDAppInstaller>(Location.App);
		zenjector.Install<SDMenuInstaller>(Location.Menu);
		zenjector.Install<SDGameInstaller>(Location.StandardPlayer);
	}
}