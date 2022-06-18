/*

    Handles everything related to the Purity Asyncronous Language.

*/

using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace PurityEngine.AL {
    [Serializable]
    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public class PurityALException : System.Exception
    {
        public PurityALException() { }
        public PurityALException(string message) : base(message) { }
        public PurityALException(string message, System.Exception inner) : base(message, inner) { }
        protected PurityALException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public class Compiler {
        public delegate void AsyncReciever(byte b);
        public List<string> memLocs = new List<string>();

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public Operation Compile(string op) {
            try {
                string[] keywords = splitIntoKeywords(op);
                if (keywords[0] != "if") {
                    throw new Exception();
                }
                Operation output = new Operation();
                output.opcode = Opcode.If;
                output.condition = Compile(keywords[1]);
                output.ifTrue = Compile(keywords[2]);
                return output;
            } catch {}
            try {
                string[] keywords = splitIntoKeywords(op);
                if (keywords[0] != "noop") {
                    throw new Exception();
                }
                Operation output = new Operation();
                output.opcode = Opcode.Noop;
                return output;
            } catch {}
            throw new PurityALException("Could not compile block: " + op);
        }

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public string[] splitIntoKeywords(string s) {
            string x = s;
            while (x.EndsWith(' ')) {
                x = x.Remove(x.Length-1);
            }
            while (x.StartsWith(' ')) {
                x = x.Remove(0,1);
            }
            List<string> keywords = new List<string>();
            int depth = 0;
            string token = "";
            bool inString = false;
            for (int i = 0; i < x.Length; i++) {
                if (x[i] == '"') {
                    inString=!inString;
                } else if (inString) {
                    // Do nothing...
                } else if (x[i] == '{' || x[i] == '(') {
                    depth++;
                } else if (x[i] == '}' || x[i] == ')') {
                    depth--;
                    if (token != "") {
                        keywords.Add(token);
                    }
                } else if (x[i] == ' ' && depth == 0) {
                    if (token != "") {
                        keywords.Add(token);
                    }                    
                    token = "";
                } else {
                    token += x[i];
                }
            }
            if (token != "") {
                keywords.Add(token);
            }
            return keywords.ToArray();
        }

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public int ReserveMemLoc(string varName) {
            if (!memLocs.Contains(varName)) {
                memLocs.Add(varName);
            }
            return memLocs.IndexOf(varName);
        }
    }

    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public enum Opcode {
        If,
        Noop
    }

    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public class Operation {
        public Opcode opcode;
        public Operation ifTrue;
        public Operation condition;
        
        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public string ToString() {
            switch (opcode) {
                case Opcode.If:
                    return "if (" + condition.ToString() + ") {" + ifTrue.ToString() + "}";
                case Opcode.Noop:
                    return "noop";
            }
            throw new NotImplementedException("Handling for " + opcode.ToString() + " in ToString() is not implemented yet.");
        }
    }

    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public interface Runtime {
        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public void Execute(Operation op, Compiler.AsyncReciever reciever);
    }

    [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
    public class CPUVM : Runtime {
        public Stack<long> stack = new Stack<long>();
        public List<long> memory = new List<long>();

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public void Execute(Operation op, Compiler.AsyncReciever reciever) {
            switch (op.opcode) {
                case Opcode.If:
                    Execute(op.condition,reciever);
                    if (stack.Pop() == 1) {
                        Execute(op.ifTrue,reciever);
                    }
                    break;
                case Opcode.Noop:
                    // It's a noop, whadd'ya expect
                    break;
                default:
                    throw new NotImplementedException("Handling for opcode " + op.opcode.ToString() + " in the CPU VM has not been implemented yet.");
            }
        }

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public long Read(int index) {
            while (memory.Count - 1 < index) {
                memory.Add(0);
            }
            return memory[index];
        }

        [Obsolete("Purity AL is currently not supported. We may pick it back up in the future, but at the moment, it is not supported.")]
        public void Write(int index, long value) {
            while (memory.Count - 1 < index) {
                memory.Add(0);
            }
            memory[index] = value;
        }
    }
}