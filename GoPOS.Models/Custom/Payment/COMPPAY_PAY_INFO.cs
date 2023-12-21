using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.Payment
{
    /// <summary>
    /// Complex Pay Item  in Grid
    /// </summary>
    public class COMPPAY_PAY_INFO : INotifyPropertyChanged
    {
        private string pAY_SEQ;
        private string pAY_TYPE;
        private string aPPR_IDT_NO;
        private decimal? aPPR_AMT;
        private string aPPR_NO;
        private string aPPR_PROC_FLAG;
        private string pROC_FLAG;
        private string pROC_STATUS = "N";
        private string cANC_INPUT_CARDNO;
        private string hANDLE_AFF_CARD_NO;
        private string hANDLE_TEMP_NO;
        private string hANDLE_CPN_NO;

        public COMPPAY_PAY_INFO()
        {
        }

        public string PAY_SEQ
        {
            get => pAY_SEQ; set
            {
                pAY_SEQ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PAY_SEQ)));
            }
        }
        public string PAY_TYPE_CODE
        {
            get => pAY_TYPE; set
            {
                pAY_TYPE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PAY_TYPE_CODE)));
            }
        }

        public string PAY_TYPE_NAME
        {
            get
            {
                return OrderPayConsts.PayTypeFlagNameByCode(PAY_TYPE_CODE, PAY_TYPE_CODE_FG);
            }
        }

        public string PAY_TYPE_CODE_FG
        {
            get;set;
        }

        public string PAY_CLASS_NAME { get; set; }
        public string PAY_VM_NANE { get; set; }
        public string APPR_IDT_NO
        {
            get => aPPR_IDT_NO; set
            {
                aPPR_IDT_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(APPR_IDT_NO)));
            }
        }

        /// <summary>
        /// 승인금액
        /// </summary>
        public decimal? APPR_AMT
        {
            get => aPPR_AMT; set
            {
                aPPR_AMT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(APPR_AMT)));
            }
        }

        /// <summary>
        /// 승인번호
        /// </summary>
        public string APPR_NO
        {
            get => aPPR_NO; set
            {
                aPPR_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(APPR_NO)));
            }
        }

        /// <summary>
        /// 1: 정상, 0: 취소
        /// </summary>
        public string APPR_PROC_FLAG
        {
            get => aPPR_PROC_FLAG; set
            {
                aPPR_PROC_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(APPR_PROC_FLAG)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(APPR_PROC_FLAG_NM)));
            }
        }


        /// <summary>
        /// 승인처리상태
        /// </summary>
        public string APPR_PROC_FLAG_NM
        {
            get
            {
                return "1".Equals(APPR_PROC_FLAG) ? "정상" : "취소";
            }
        }

        /// <summary>
        /// A: auto: M: manual
        /// 처리요건
        /// </summary>
        public string PROC_METHOD
        {
            get => pROC_FLAG; set
            {
                pROC_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PROC_METHOD)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PROC_METHOD_NM)));
            }
        }


        /// <summary>
        /// 처리요건
        /// </summary>
        public string PROC_METHOD_NM
        {
            get
            {
                return "A".Equals(PROC_METHOD) ? "자동취소" : "수동취소";
            }
        }

        /// <summary>
        /// 반품처리상태
        /// N: NOT, C: DONE, E: ERROR
        /// * "", 취소완료, 오류
        /// </summary>
        public string PROC_STATUS
        {
            get => pROC_STATUS; set
            {
                pROC_STATUS = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PROC_STATUS)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PROC_STATUS_NM)));
            }
        }

        /// <summary>
        /// 반품처리상태
        /// </summary>
        public string PROC_STATUS_NM
        {
            get
            {
                return "N".Equals(PROC_STATUS) ? string.Empty : ("C".Equals(PROC_STATUS) ? "취소완료" : "오류");
            }
        }

        public object[] PayDatas { get; set; }

        public string CANC_INPUT_CARDNO
        {
            get => cANC_INPUT_CARDNO; set
            {
                cANC_INPUT_CARDNO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CANC_INPUT_CARDNO));
            }
        }

        public string HANDLE_AFF_CARD_NO
        {
            get => hANDLE_AFF_CARD_NO; set
            {
                hANDLE_AFF_CARD_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(HANDLE_AFF_CARD_NO));
            }
        }
        public string HANDLE_TEMP_NO
        {
            get => hANDLE_TEMP_NO; set
            {
                hANDLE_TEMP_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(HANDLE_TEMP_NO));
            }
        }
        public string HANDLE_CPN_NO
        {
            get => hANDLE_CPN_NO; set
            {
                hANDLE_CPN_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(HANDLE_CPN_NO));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
