// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;

namespace Example {
    public class FunctionException : Exception {
        public FunctionException (string Message) : base(Message) { }
    }
}
