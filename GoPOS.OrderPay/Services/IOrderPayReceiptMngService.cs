using Dapper;
using GoPOS.Models;
using GoPOS.Models.Custom.OrderMng;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-02-13 김형석 생성
 */
public interface IOrderPayReceiptMngService
{
    public Task<List<RECEIPT_MANAGER_ITEM>> GetTrnTenderSeqList(string saleDate, string posNo, string billNo);
}

