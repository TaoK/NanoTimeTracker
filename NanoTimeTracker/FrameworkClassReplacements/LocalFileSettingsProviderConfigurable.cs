/*
Nano TimeTracker - a small free windows time-tracking utility
Copyright (C) 2011 Tao Klerks

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.IO;
using System.Xml;

namespace NanoTimeTracker.FrameworkClassReplacements
{
    class LocalFileSettingsProviderConfigurable : SettingsProvider, IApplicationSettingsProvider
    {
        //This is a very basic replacement for LocalFileSettingsProvider, giving some small measure of control
        // over the location of the configuration file, placing it explicitly in "UserAppDataPath" (so that you 
        // can actually store other data in the same place if you want!). It also supports storing the data in
        // the executing assembly's folder, using that if already present. It does have a few significant 
        // limitations, primarily to limit scope/complexity as we don't currently use these features:
        // - Ignores "IsRoaming" property
        // - Ignores "UserScopedSettingAttribute" vs "ApplicationScopedSettingAttribute"
        // - Ignores SettingsContext, so no groups
        //
        //Upgrading settings from previous versions is supported, although it doesn't make sense (or have any 
        // effect) when the settings are stored in the assembly path.
        //
        //Additional utility methods are made available, for example for deleting old version paths (presumably
        // after checking with the user!).
        //
        //Please note, if you are using auto-numbering with an explicitly defined "AssemblyFileVersion" attribute,
        // then you will get "Invalid Characters in Path"-style errors when this code attempts to access the 
        // standard Windows Forms Application "UserAppDataPath" property.
        // (see http://all-things-pure.blogspot.com/2009/09/assembly-version-file-version-product.html)
        //
        //See the following articles for more information on creating custom providers:
        // - http://msdn.microsoft.com/en-us/library/8eyb2ct1.aspx
        // - http://www.sellsbrothers.com/writing/dotnet2customsettingsprovider.htm
        // - http://www.codeproject.com/KB/vb/CustomSettingsProvider.aspx
        // - http://blog.joachim.at/?p=30

        public override void Initialize(string name, NameValueCollection config) 
        {
            base.Initialize(this.ApplicationName, config);
        }

        public override string ApplicationName
        {
            get
            {
                return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            }
            set
            {
                //Do nothing
            }
        }

        private const string SETTINGS_FILENAME = "UserSettings.xml";
        private static string AssemblyLocalFilePath
        {
            get
            {
                FileInfo assemblyLocation = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return Path.Combine(assemblyLocation.DirectoryName, assemblyLocation.Name.Substring(0, assemblyLocation.Name.Length - assemblyLocation.Extension.Length + 1) + SETTINGS_FILENAME);
            }
        }

        private static bool SettingsFileIsVersioned
        {
            get
            {
                if (File.Exists(AssemblyLocalFilePath))
                    return false;
                else
                    return true;
            }
        }

        private static string CurrentSettingsFilePath
        {
            get
            {
                if (SettingsFileIsVersioned)
                    return Path.Combine(System.Windows.Forms.Application.UserAppDataPath, SETTINGS_FILENAME);
                else
                    return AssemblyLocalFilePath;
            }
        }

        private XmlDocument _currentSettingsDoc;
        private XmlDocument CurrentSettingsDocument
        {
            get
            {
                lock (this)
                {
                    if (_currentSettingsDoc == null)
                    {
                        _currentSettingsDoc = GetSettingsDocument(CurrentSettingsFilePath);
                        if (_currentSettingsDoc == null)
                        {
                            _currentSettingsDoc = new XmlDocument();
                            _currentSettingsDoc.AppendChild(_currentSettingsDoc.CreateElement("Settings"));
                        }
                    }
                    return _currentSettingsDoc;
                }
            }
        }

        private XmlDocument GetVersionedSettingsDocument(Version targetVersion)
        {
            DirectoryInfo appVersionContainerFolder = new DirectoryInfo(System.Windows.Forms.Application.UserAppDataPath).Parent;
            return GetSettingsDocument(Path.Combine(Path.Combine(appVersionContainerFolder.FullName, targetVersion.ToString()), SETTINGS_FILENAME));
        }

        private XmlDocument GetSettingsDocument(string targetPath)
        {
            try
            {
                XmlDocument settingsDoc = new XmlDocument();
                settingsDoc.Load(targetPath);
                return settingsDoc;
            }
            catch 
            {
                return null;
            }
        }



        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
            {
                SetPropertyValue(CurrentSettingsDocument, propertyValue);
            }
            CurrentSettingsDocument.Save(CurrentSettingsFilePath);
        }

        private static void SetPropertyValue(XmlDocument settingsDocument, SettingsPropertyValue propertyValue)
        {
            //try to get the element
            XmlElement propertyNode = (XmlElement)settingsDocument.SelectSingleNode(string.Format("//Setting[@name = '{0}']", propertyValue.Name.Replace("'", "&apos;")));

            //if not exists, then create
            if (propertyNode == null)
            {
                propertyNode = settingsDocument.CreateElement("Setting");
                propertyNode.SetAttribute("name", propertyValue.Name);
                settingsDocument.DocumentElement.AppendChild(propertyNode);
            }

            //set the value!
            propertyNode.InnerText = propertyValue.SerializedValue.ToString();
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection outputValues = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                outputValues.Add(GetPropertyValue(CurrentSettingsDocument, property));
            }

            return outputValues;
        }

        private static SettingsPropertyValue GetPropertyValue(XmlDocument sourceDoc, SettingsProperty property)
        {
            SettingsPropertyValue value = new SettingsPropertyValue(property);
            value.IsDirty = false;

            XmlElement propertyNode = (XmlElement)sourceDoc.SelectSingleNode(string.Format("//Setting[@name = '{0}']", property.Name.Replace("'", "&apos;")));
            if (propertyNode != null)
                value.SerializedValue = propertyNode.InnerText;
            else
                value.SerializedValue = property.DefaultValue;

            return value;
        }

        public Version GetCurrentVersionNumber()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public Version GetPreviousVersionNumber()
        {
            Version currentVersion = GetCurrentVersionNumber();
            Version bestCandidateVersion = null;

            if (SettingsFileIsVersioned)
            {
                DirectoryInfo appVersionContainerFolder = new DirectoryInfo(System.Windows.Forms.Application.UserAppDataPath).Parent;
                foreach (DirectoryInfo versionFolder in appVersionContainerFolder.GetDirectories())
                {
                    try
                    {
                        Version candidateVersion = new Version(versionFolder.Name);
                        if (candidateVersion < currentVersion && (bestCandidateVersion == null || candidateVersion > bestCandidateVersion)) 
                        {
                            bestCandidateVersion = candidateVersion; 
                        }
                    }
                    catch
                    {
                        //who cares?
                    }
                }
            }

            return bestCandidateVersion;
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            SettingsPropertyValue outValue = null;
            Version previousVersion = GetPreviousVersionNumber();
            if (previousVersion != null)
            {
                XmlDocument previousSettingsDoc = GetVersionedSettingsDocument(previousVersion);
                if (previousSettingsDoc != null)
                {
                    outValue = GetPropertyValue(previousSettingsDoc, property);
                }
            }
            else
            {
                outValue = new SettingsPropertyValue(property);
                outValue.PropertyValue = null;
            }
            return outValue;
        }

        public void Reset(SettingsContext context)
        {
            CurrentSettingsDocument.DocumentElement.RemoveAll();
            CurrentSettingsDocument.Save(CurrentSettingsFilePath);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            Version previousVersion = GetPreviousVersionNumber();
            if (previousVersion != null)
            {
                Reset(context);

                XmlDocument previousSettingsDoc = GetVersionedSettingsDocument(previousVersion);
                if (previousSettingsDoc != null)
                {
                    foreach (SettingsProperty property in properties)
                    {
                        SettingsPropertyValue oldValue = GetPropertyValue(previousSettingsDoc, property);
                        if (oldValue.PropertyValue != null)
                            SetPropertyValue(CurrentSettingsDocument, oldValue);
                    }
                }
            }
        }

        public static void MoveToAssemblyLocalStorage()
        {
            if (!CurrentSettingsFilePath.Equals(AssemblyLocalFilePath) && File.Exists(CurrentSettingsFilePath))
                File.Copy(CurrentSettingsFilePath, AssemblyLocalFilePath);
            //TODO: handle any other files, besides settings files
        }


        //TODO: Implement additional static methods for version-mgmt (deleting old versiuons' settings, migratiung other data, etc)

    }
}
