using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-02-12 김형석 생성
 */
public interface IOrderPayCompPayService
{
    public Task<List<MST_INFO_CARD>> GetCreditCardList();

}

