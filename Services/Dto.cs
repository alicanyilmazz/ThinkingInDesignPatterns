using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public sealed class DoPinChangeRequest
    {
        public string CardNumber { get; set; }
    }

    public sealed class DoPinChangeResponse
    {
        public bool Success { get; set; }
    }

    public sealed class DoCashWithDrawalRequest
    {
        public decimal Amount { get; set; }
    }

    public sealed class DoCashWithDrawalResponse
    {
        public bool Success { get; set; }
    }
}
