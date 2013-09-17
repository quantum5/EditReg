using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditReg {
    class Program {
        static int help() {
            return 2;
        }

        static Dictionary<string, Type> commandMap = new Dictionary<string, Type> {
            {"query", typeof(Command.QueryCommand)},
        };

        static int Main(string[] args) {
            if (args.Length < 1)
                return help();

            string command = args[0].ToLower();

            if (commandMap.ContainsKey(command)) {
                if (!typeof(Command.Command).IsAssignableFrom(commandMap[command])) {
                    throw new InvalidOperationException(
                        string.Format("Type {0} is not subclass of Command",
                                      commandMap[command].FullName));
                }
                object @object = Activator.CreateInstance(commandMap[command]);
                Command.Command cmd = (Command.Command) @object;
                cmd.program(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                cmd.command(command);
                cmd.args(args.Skip(1).ToArray());
                cmd.run();
            }

            return 1;
        }
    }
}
