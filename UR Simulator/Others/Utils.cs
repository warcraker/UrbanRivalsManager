using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using UrbanRivalsCore.Model;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Linq;

namespace UrbanRivalsManager.Utils
{
    // TODO: Deprecated

    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = (Enum)e, DisplayName = e.ToString() });
        }
    }

    public static class WPFThreadingExtensions
    {
        public static void InvokeIfRequired(this DispatcherObject control, Action methodcall, DispatcherPriority priorityForCall)
        {
            //see if we need to Invoke call to Dispatcher thread
            if (control.Dispatcher.Thread != Thread.CurrentThread)
                control.Dispatcher.Invoke(priorityForCall, methodcall);
            else
                methodcall();
        }

        public static void Invoke(this DispatcherObject control, Action methodcall, DispatcherPriority priorityForCall)
        {
            control.Dispatcher.Invoke(methodcall, priorityForCall);
        }
    }
   
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    public class PortableSettingsProvider : SettingsProvider
    {
        const string SETTINGSROOT = "Settings";

        public override void Initialize(string name, NameValueCollection col)
        {
            base.Initialize(this.ApplicationName, col);
        }

        public override string ApplicationName
        {
            get
            {
                if (System.Windows.Forms.Application.ProductName.Trim().Length > 0)
                {
                    return System.Windows.Forms.Application.ProductName;
                }
                else
                {
                    FileInfo fi = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
                    return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                }
            }
            set { } //Do nothing
        }

        public override string Name
        {
            get { return "PortableSettingsProvider"; }
        }
        public virtual string GetAppSettingsPath()
        {
            //Used to determine where to store the settings
            System.IO.FileInfo fi = new System.IO.FileInfo(System.Windows.Forms.Application.ExecutablePath);
            return fi.DirectoryName;
        }

        public virtual string GetAppSettingsFilename()
        {
            //Used to determine the filename to store the settings
            return ApplicationName + ".settings";
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
        {
            //Iterate through the settings to be stored
            //Only dirty settings are included in propvals, and only ones relevant to this provider
            foreach (SettingsPropertyValue propval in propvals)
            {
                SetValue(propval);
            }

            try
            {
                SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
            }
            catch (Exception)
            {
                ; //Ignore if cant save, device been ejected
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
        {
            //Create new collection of values
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            //Iterate through the settings to be retrieved
            foreach (SettingsProperty setting in props)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                value.SerializedValue = GetValue(setting);
                values.Add(value);
            }
            return values;
        }

        private XmlDocument _settingsXML = null;

        private XmlDocument SettingsXML
        {
            get
            {
                //If we dont hold an xml document, try opening one.  
                //If it doesnt exist then create a new one ready.
                if (_settingsXML == null)
                {
                    _settingsXML = new XmlDocument();

                    try
                    {
                        _settingsXML.Load(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
                    }
                    catch (Exception)
                    {
                        //Create new document
                        XmlDeclaration dec = _settingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
                        _settingsXML.AppendChild(dec);

                        XmlNode nodeRoot = default(XmlNode);

                        nodeRoot = _settingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
                        _settingsXML.AppendChild(nodeRoot);
                    }
                }

                return _settingsXML;
            }
        }

        private string GetValue(SettingsProperty setting)
        {
            string ret = "";

            try
            {
                if (IsRoaming(setting))
                {
                    ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + setting.Name).InnerText;
                }
                else
                {
                    ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + setting.Name).InnerText;
                }
            }

            catch (Exception)
            {
                if ((setting.DefaultValue != null))
                {
                    ret = setting.DefaultValue.ToString();
                }
                else
                {
                    ret = "";
                }
            }

            return ret;
        }

        private void SetValue(SettingsPropertyValue propVal)
        {

            XmlElement MachineNode = default(XmlElement);
            XmlElement SettingNode = default(XmlElement);

            //Determine if the setting is roaming.
            //If roaming then the value is stored as an element under the root
            //Otherwise it is stored under a machine name node 
            try
            {
                if (IsRoaming(propVal.Property))
                {
                    SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name);
                }
                else
                {
                    SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + propVal.Name);
                }
            }
            catch (Exception)
            {
                SettingNode = null;
            }

            //Check to see if the node exists, if so then set its new value
            if ((SettingNode != null))
            {
                SettingNode.InnerText = propVal.SerializedValue.ToString();
            }
            else
            {
                if (IsRoaming(propVal.Property))
                {
                    //Store the value as an element of the Settings Root Node
                    SettingNode = SettingsXML.CreateElement(propVal.Name);
                    SettingNode.InnerText = propVal.SerializedValue.ToString();
                    SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(SettingNode);
                }
                else
                {
                    //Its machine specific, store as an element of the machine name node,
                    //creating a new machine name node if one doesnt exist.
                    try
                    {

                        MachineNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName);
                    }
                    catch (Exception)
                    {
                        MachineNode = SettingsXML.CreateElement(Environment.MachineName);
                        SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
                    }

                    if (MachineNode == null)
                    {
                        MachineNode = SettingsXML.CreateElement(Environment.MachineName);
                        SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
                    }

                    SettingNode = SettingsXML.CreateElement(propVal.Name);
                    SettingNode.InnerText = propVal.SerializedValue.ToString();
                    MachineNode.AppendChild(SettingNode);
                }
            }
        }

        private bool IsRoaming(SettingsProperty prop)
        {
            //Determine if the setting is marked as Roaming
            foreach (DictionaryEntry d in prop.Attributes)
            {
                Attribute a = (Attribute)d.Value;
                if (a is System.Configuration.SettingsManageabilityAttribute)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // XXX QQQ Not used, Stored here in case I'll use them. Otherwise is safe to remove them.

    //public static class UnixTimeConverter
    //{
    //    private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
    //
    //    public static double ToUnixSeconds(DateTime date)
    //    {
    //        TimeSpan span = (date.ToLocalTime() - epoch);
    //        return span.TotalSeconds;
    //    }
    //
    //    public static DateTime ToDate(double unixSeconds)
    //    {
    //        return epoch.AddSeconds(unixSeconds);
    //    }
    //}

    //public static class Randomizer
    //{
    //    private static Random randomizer;
    //
    //    static Randomizer() { randomizer = new Random(); }
    //
    //    public static int Next()
    //    {
    //        return randomizer.Next();
    //    }
    //
    //    public static int Next(int max)
    //    {
    //        return randomizer.Next(max);
    //    }
    //
    //    public static int Next(int min, int max)
    //    {
    //        return randomizer.Next(min, max);
    //    }
    //}

    //public static class CloneHelper
    //{
    //    public static T Copy<T>(T source)
    //    {
    //        if (!typeof(T).IsSerializable)
    //            throw new ArgumentException("The class " + typeof(T).ToString() + " is not serializable");
    //
    //        if (Object.ReferenceEquals(source, null))
    //            return default(T);
    //
    //        IFormatter formatter = new BinaryFormatter();
    //        Stream stream = new MemoryStream();
    //        using (stream)
    //        {
    //            try
    //            {
    //                formatter.Serialize(stream, source);
    //                stream.Seek(0, SeekOrigin.Begin);
    //                return (T)formatter.Deserialize(stream);
    //            }
    //            catch (SerializationException ex)
    //            { throw new ArgumentException(ex.Message, ex); }
    //            catch { throw; }
    //        }
    //    }
    //}
}
