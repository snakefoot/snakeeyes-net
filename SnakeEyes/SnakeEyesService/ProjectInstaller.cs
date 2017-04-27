using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Configuration.Install;

namespace SnakeEyes
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected virtual string AppendPathParameter(string path, string parameter)
        {
            if (path.Length > 0 && path[0] != '"')
            {
                path = "\"" + path + "\"";
            }
            path += " " + parameter;
            return path;
        }

        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            // Apply the ServiceDependencies specified in the app.config file
            // http://raquila.com/software/configure-app-config-application-settings-during-msi-install/
            string assemblyPath = Context.Parameters["assemblypath"];
            Configuration config = ConfigurationManager.OpenExeConfiguration(assemblyPath);

            KeyValueConfigurationElement dependencies = config.AppSettings.Settings["ServiceDependencies"];
            if (dependencies != null && !string.IsNullOrEmpty(dependencies.Value))
            {
                serviceInstaller1.ServicesDependedOn = dependencies.Value.Split(',');
            }

            Context.Parameters["assemblypath"] = AppendPathParameter(assemblyPath, "/service");
            base.OnBeforeInstall(savedState);
        }

        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            string assemblyPath = Context.Parameters["assemblypath"];
            Context.Parameters["assemblypath"] = AppendPathParameter(assemblyPath, "/service");
            base.OnBeforeUninstall(savedState);
        }
    }
}