using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditReg.Command {
    public interface Command {
        void program(string name);
        void command(string name);
        void args(IEnumerable<string> args);
        void run();
    }

    public abstract class AbstractCommand : Command {
        protected string _program = "", _command = "";

        public void program(string name) {
            _program = name;
        }

        public void command(string name) {
            _command = name;
        }

        abstract public void args(IEnumerable<string> args);
        abstract public void run();
    }
}
