using Caliburn.Micro;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.ViewModels;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using System.Xml.Linq;
using log4net.Core;
using Google.Protobuf.Collections;
using GoPOS.Service;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using GoPOS.Service.Service.POS;

namespace GoPOS.ViewModels
{
    public class OrderPayMemberPointSaveViewModel : OrderPayChildViewModel
    {
        private TRN_POINTSAVE pointTRN;
        private ORD_POINTSAVE pointORD;
        private readonly IPOSTMangService pOSTMangService;
        public OrderPayMemberPointSaveViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IPOSTMangService pOSTMangService) 
            : base(windowManager, eventAggregator)
        {
            this.pOSTMangService = pOSTMangService;
        }
      
    }
}
