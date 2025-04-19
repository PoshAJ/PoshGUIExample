// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;

namespace PoshGUIExample {
    public class GreetingArgumentException : Exception {
        public GreetingArgumentException (string Message) : base(Message) { }
    }
}
