using System.Collections.Generic;

using WinFormsBlazor.Model;

namespace WinFormsBlazor.Service
{
    public class OptionService
    {
        public static List<Option> SelectOption { get; set; } = new();

        public static Task<List<Option>> OptionAsync(string gubun)
        {
            return Task.FromResult(SelectOption.FindAll(x => x.OptionGubun == gubun).OrderBy(x => x.OptionNum).ToList());
        }

        public static void AddOption(int optNum, string gubun, string option)
        {
            bool isNew = true;
            if (SelectOption.Count == 0)
            {
                isNew = true;
            }
            else
            {
                for (int i = 0; i < SelectOption.Count; i++)
                {
                    if (SelectOption[i].OptionGubun == gubun && SelectOption[i].OptionNum == optNum)
                    {
                        SelectOption[i].OptionValue = option;
                        isNew = false;
                    }
                }
            }

            if (isNew)
            {
                var newOption = new Option
                {
                    OptionGubun = gubun,
                    OptionNum = optNum,
                    OptionValue = option
                };
                SelectOption.Add(newOption);
            }
        }
    }
}
