// Copyright (c) 2025 Anthony J. Raymond, MIT License

using System;

namespace PoshGUIExample {
    public class GreetingArgumentException : Exception {
        public GreetingArgumentException (string message) : base(message) { }
    }
}
