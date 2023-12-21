using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 멤버조회
/// </summary>

public class OrderPayMemberSearchService : IOrderPayMemberSearchViewModel
{
    public CheckBox iCheck1 => throw new NotImplementedException();

    public CheckBox iCheck2 => throw new NotImplementedException();

    public CheckBox iLuna => throw new NotImplementedException();

    public CheckBox iSolar => throw new NotImplementedException();

    public Point ButtonPost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void SearchMember(string Telno, string Cardno, string name)
    {
        throw new NotImplementedException();
    }
}