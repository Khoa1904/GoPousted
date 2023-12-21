using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-02-25 김형석 생성

 */
public interface IPostPayTableManagementService
{
    Task<(List<MST_TABLE_INFO>, SpResult)> GetTableInfo();

    Task<(List<MST_TABLE_DEPT>, SpResult)> GetTableDept();

}
