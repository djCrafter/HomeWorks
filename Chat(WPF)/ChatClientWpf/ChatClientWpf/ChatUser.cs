﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatClientWpf
{
    public class ChatUser
    {
        private readonly string name;
        private readonly string id;

        public ChatUser(string name, string id)
        {
            this.name = name;
            this.id = id;
        }

        public string Name
        {
            get { return name; }
        }

        public string Id
        {
            get { return id; }
        }

        public static explicit operator ChatUser(ListBoxItem v)
        {
            throw new NotImplementedException();
        }
    }
}
