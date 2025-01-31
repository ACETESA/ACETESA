﻿using System.Collections;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Transversal
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> MessageList { get; private set; }

        public OperationResult()
        {
            MessageList = new List<string>();
            Success = true;
        }

        public void AddMessage(string message)
        {
            MessageList.Add(message);
        }
    }
}
