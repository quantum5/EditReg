using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using Microsoft.Win32;

namespace EditReg.Command {
    class QueryCommand : AbstractCommand {
        protected RegistryKey key;
        protected List<string> subkeys = new List<string>();

        public override void run() {
            if (key == null)
                return;

            key.printKeys();
        }

        public override void args(IEnumerable<string> args) {
            OptionSet options = new OptionSet {
                {"v|value=", "Value name to query", x => subkeys.Add(x)},
            };
            List<string> extra;
            try {
                extra = options.Parse(args);
                if (extra.Count != 1)
                    throw new OptionException("You must specify one and only one key to query", "key");
                key = Utilities.ParseRegistryKey(extra[0], "key");
            } catch (OptionException e) {
                string name = string.Format("{0} {1}", _program, _command).TrimStart();
                Console.Write(name + ": ");
                Console.WriteLine(e.Message);
                Console.WriteLine(string.Format("Try `{0} --help` for more information", name));
            }
        }
    }
}
