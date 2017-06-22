

using lw.Data;
namespace lw.Products.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {
        
        public Settings() {
			this.SettingsLoaded += new System.Configuration.SettingsLoadedEventHandler(Settings_SettingsLoaded);

        }

		void Settings_SettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
		{
			DirectorBase db = new DirectorBase(cte.lib);
			this["jcproshopConnectionString"] = db.GetConnection().ConnectionString;
			this["freshveryConnectionString"] = db.GetConnection().ConnectionString;
			
		}
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
        }
    }
}
