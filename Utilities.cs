using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NDesk.Options;

namespace EditReg {
    public static class Utilities {
        public static Dictionary<string, RegistryKey> hiveMap = new Dictionary<string, RegistryKey> {
            {"HKCR", Registry.ClassesRoot},
            {"HKEY_CLASSES_ROOT", Registry.ClassesRoot},
            {"HKCU", Registry.CurrentUser},
            {"HKEY_CURRENT_USER", Registry.CurrentUser},
            {"HKLM", Registry.LocalMachine},
            {"HKEY_LOCAL_MACHINE", Registry.LocalMachine},
            {"HKU", Registry.Users},
            {"HKEY_USERS", Registry.Users},
            {"HKPD", Registry.PerformanceData},
            {"HKEY_PERFORMANCE_DATA", Registry.PerformanceData},
        };
        public static Dictionary<RegistryValueKind, string> kindName = new Dictionary<RegistryValueKind, string> {
            {RegistryValueKind.Binary, "REG_BINARY"},
            {RegistryValueKind.DWord, "REG_DWORD"},
            {RegistryValueKind.ExpandString, "REG_EXPAND_SZ"},
            {RegistryValueKind.MultiString, "REG_MULTI_SZ"},
            {RegistryValueKind.None, "(None)"},
            {RegistryValueKind.QWord, "REG_QWORD"},
            {RegistryValueKind.String, "REG_SZ"},
            {RegistryValueKind.Unknown, "(Unknown)"},
        };
        public static Dictionary<RegistryValueKind, char> kindId = new Dictionary<RegistryValueKind, char> {
            {RegistryValueKind.Binary, 'b'},
            {RegistryValueKind.DWord, 'd'},
            {RegistryValueKind.ExpandString, 'e'},
            {RegistryValueKind.MultiString, 'm'},
            {RegistryValueKind.None, 'n'},
            {RegistryValueKind.QWord, 'q'},
            {RegistryValueKind.String, 's'},
            {RegistryValueKind.Unknown, 'b'}, // Assumed binary
        };

        public static RegistryKey ParseRegistryKey(string name, string argName = null, bool create = false) {
            string[] names = name.Split(new char[] {'/', '\\'}, 2);
            name = names[1];
            RegistryKey hive;
            
            if (!hiveMap.TryGetValue(names[0], out hive))
                throw new OptionException(string.Format("Invalid registry hive: {0}", hive), argName);
            if (create)
                return hive.CreateSubKey(name);
            else {
                RegistryKey key = hive.OpenSubKey(name);
                if (key == null)
                    throw new OptionException(string.Format("Invalid subkey under {0}: {1}", hive, name), argName);
                return key;
            }
        }

        public static void printKeys(this RegistryKey key) {
            key.printKeys(key.GetValueNames());
        }

        public static void printKeys(this RegistryKey key, IEnumerable<string> keys) {
            List<Tuple<string, string, string>> result = new List<Tuple<string, string, string>>();
            int maxName = 4, maxType = 4;
            foreach (string name in keys) {
                string type = Utilities.kindName[key.GetValueKind(name)];
                string value = key.GetValue(name).ToString();

                if (name.Length > maxName)
                    maxName = name.Length;
                if (type.Length > maxType)
                    maxType = type.Length;
                result.Add(Tuple.Create(name, type, value));
            }

            ++maxName;
            ++maxType;
            string format = string.Format("{{0,{0}}}|{{1,{1}}}|{{2}}", maxName, maxType);
            Console.WriteLine(string.Format(format, "Name", "Type", "Value"));
            Console.WriteLine(new string('-', maxName + maxType + 7));
            foreach (var value in result)
                Console.WriteLine(string.Format(format, value.Item1, value.Item2, value.Item3));
        }
    }
}
