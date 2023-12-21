﻿using GoPOS.Models.Custom.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.ConfigSetup.Interface.View
{
    public interface IAuthenticProcessingView
    {
        TextBlock PosIPLabel { get; }
        TextBox PosIP { get; }
    }
}
