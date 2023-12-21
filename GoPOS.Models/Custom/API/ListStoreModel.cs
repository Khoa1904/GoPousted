using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.API
{
    public class ListStoreModel : INotifyPropertyChanged
    {
        public int no { get; set; } = 1;
        public string posNo { get; set; } = "";
        public string storeNm { get; set; }
        public string localPosAt { get; set; }
        public string fchqCode { get; set; }
        public string storeCode { get; set; } = "";
        public string posGb { get; set; }

        public string PosNo
        {
            get => posNo;
            set
            {
                posNo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PosNo)));
            }
        }
        public int NO
        {
            get => no;
            set
            {
                no = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NO)));
            }
        }

        public string StoreCode
        {
            get => storeCode;
            set
            {
                storeCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StoreCode)));
            }
        }

        public string StoreNm
        {
            get => storeNm;
            set
            {
                storeNm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StoreNm)));
            }
        }


        public string LocalPosAt
        {
            get => localPosAt;
            set
            {
                localPosAt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocalPosAt)));
            }
        }

        public string FchqCode
        {
            get => fchqCode;
            set
            {
                fchqCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FchqCode)));
            }
        }


        private string _mainPosId;
        public string MainPosId
        {
            get => _mainPosId;
            set
            {
                _mainPosId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainPosId)));
            }
        }

        public string PosGb
        {
            get => posGb;
            set
            {
                posGb = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PosGb)));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
