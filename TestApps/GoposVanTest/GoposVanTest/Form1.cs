using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;

using System.Text;

namespace GoposVanTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //ResultMsg.Text = "";
            //VanAPI vanAPI = new();
            //VanRequestMsg msg = new KocesRequestMsg()
            //{
            //    MER_DATA = "KOCES POS TEST",
            //    TRAN_AMT = "1000",
            //    TAX_AMT = "100",
            //    SVC_AMT = "0",
            //    DUTY_AMT = "0",
            //    INS_MON = "00"              
            //};

            //try
            //{
            //    VanResponseMsg result = vanAPI.ApprovalCreditCard(msg);
            //    string? resultTxt = result.ToString();
            //    string? APPR_VER = result.APPR_VER;                                                                                         //   전문버전
            //    ResultMsg.Text = resultTxt;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            VanRequestMsg msg = new VanRequestMsg()
            {
                VAN_CODE = "12",  // Smartro 밴코드
                AGENT_IP = "127.0.0.1",
                AGENT_PORT = "13855",
                TRAN_AMT = "1000",
                TAX_AMT = "101",
                SVC_AMT = "0",
                INS_MON = "00",
                SIGN_AT = "1",
                WORKING_KEY = "",
                WORKING_KEY_INDEX = ""
            };

            VanAPI vanAPI = new VanAPI();
            VanResponseMsg result = vanAPI.ApprovalCreditCard(msg);
        }
    }
}