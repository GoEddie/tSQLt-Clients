﻿using System.Xml.Serialization;

namespace tSQLt.Client.Net
{
    public class Failure
    {
        [XmlAttribute("message")]
        public string Message;

    }
}