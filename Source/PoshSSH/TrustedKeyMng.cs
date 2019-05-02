using System.Collections.Generic;
using Microsoft.Win32;
using System;
using System.IO;
using Newtonsoft.Json;

namespace SSH
{
    // Class for managing the keys 
    public class TrustedKeyMng
    {

        private string GetConfigFileName()
        {
            string filename = "keys.json";
            string foldername = ".sshconf";
            string userhomepath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            userhomepath = System.IO.Path.Combine(userhomepath, foldername);
            if (!Directory.Exists(userhomepath))
            {
                Directory.CreateDirectory(userhomepath);
            }
            filename = System.IO.Path.Combine(userhomepath, filename);
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }
            return filename;
        }

        public Dictionary<string, string> GetKeys()
        {
            var hostkeys = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            string filename = GetConfigFileName();
            string json = File.ReadAllText(filename);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    hostkeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
                catch
                {

                }
            }
            return hostkeys;
        }

        public bool SetKey(string host, string fingerprint)
        {
            var hostkeys = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            string filename = GetConfigFileName();
            string json = string.Empty;
            json = File.ReadAllText(filename);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    hostkeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
                catch
                {

                }
            }
            hostkeys.Add(host, fingerprint);
            json = JsonConvert.SerializeObject(hostkeys, Formatting.Indented);
            File.WriteAllText(filename, json);
            return true;
        }
    }
}
