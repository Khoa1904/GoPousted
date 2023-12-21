﻿using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.Model
{
    public interface IShellViewModel
    {
        string CurrentPage { get; set; }
        Task TryCloseAsync(bool? dialogResult = null);
    }
}