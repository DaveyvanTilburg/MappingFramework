﻿using XPathSerialization;

namespace XPathObjects
{
    public class RoomStay : Adaptable
    {
        public string Code { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string RateCode { get; set; } = string.Empty;
    }
}