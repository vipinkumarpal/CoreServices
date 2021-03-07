using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Utility.Faults
{
    public class Faultkey
    {
        public string Key { get; }
        public string Description { get; }
        public Faultkey() { }
        private Faultkey(string key, string description)
        {
            Key = key;
            Description = description;
        }

        public static Faultkey Create(string key, string description) => new Faultkey(key, description);

        public static Faultkey MsgNoContentFound => new Faultkey("MsgNoContentFound","No Content Found");

        public static Faultkey MsgGenericException => Create("MsgGenericException", "The application was unable to process your request due to internal error");
    }
}
