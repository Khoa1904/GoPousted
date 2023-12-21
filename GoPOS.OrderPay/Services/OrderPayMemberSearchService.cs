using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface IOrderPayMemberSearchViewModel
{
    public  void SearchMember(string telNo, string cardNo, string name);
}