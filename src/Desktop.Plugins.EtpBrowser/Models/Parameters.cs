using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.Models
{
    public class Parameters
    {
        [JsonIgnore]
        public BindableCollection<string> Name { get; set; } = new BindableCollection<string>()
        { 
            //Defined Parameters
            "TestName", 
            "TestID", 
            "Interval",
            "AllChannels",
            "DelayedStart", 
            "DelayedStartInterval",
            "DelayedMnemonicCount",
            "TimeoutOnNoDataRecieved",
            "loadDataDensity", 
            "RangeValue", 
            "Mnemonics", 
            "RandomMnemonicCount", 
            "isAllChannels",
            "enableOffset", 
            "isActive", 
            "useCurvesUrisForDescribe", 
            "startFromCurrentTime",
            "enabled",
            "testType",
            "enableOffset"
        };
        
        public string Value { get; set; }
        public string SelectedName { get; set; } = "";
    }
}
