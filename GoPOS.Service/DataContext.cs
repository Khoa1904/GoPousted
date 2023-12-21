using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Config;
using GoPOS.Service.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Policy;

namespace GoPOS.Service
{
    [DbConfigurationType(typeof(DbConfig))]
    public class DataContext : DbContext
    {
        public DbSet<POS_STATUS> pOS_STATUSes { get; set; }
        public DbSet<SETT_POSACCOUNT> sETT_POSACCOUNTs { get; set; }

        public DbSet<MST_INFO_JOINCARD> mST_INFO_JOINCARDs { get; set; }

        public DbSet<MST_INFO_PRD_JOINCARD> mST_INFO_PRD_JOINCARDs { get; set; }

        public DbSet<POS_KEY_MANG> pOS_KEY_MANGs { get; set; }

        public DbSet<POS_MST_MANG> pOS_MST_MANGs { get; set; }
        public DbSet<POS_POST_MANG> pOS_POST_MANGs { get; set; }

        public DbSet<MST_COMM_CODE> mST_COMM_CODEs { get; set; }
        public DbSet<MST_COMM_CODE_SHOP> mST_COMM_CODE_SHOPs { get; set; }
        public DbSet<MST_CNFG_CODE> mST_CNFG_CODEs { get; set; }
        public DbSet<MST_CNFG_DETAIL> mST_CNFG_DETAILs { get; set; }

        public DbSet<MST_CNFG_SHOP> mST_CNFG_SHOPs { get; set; }
        public DbSet<MST_CNFG_POS> mST_CNFG_POs { get; set; }
        public DbSet<MST_INFO_KDS> mST_INFO_KDSs { get; set; }
        public DbSet<MST_INFO_KDS_PRD> mST_INFO_KDS_PRDs { get; set; }

        public DbSet<MST_INFO_SHOP> mST_INFO_SHOPs { get; set; }
        public DbSet<MST_INFO_POS> mST_INFO_POs { get; set; }
        public DbSet<MST_INFO_VAN_POS> mST_INFO_VAN_POs { get; set; }
        public DbSet<MST_INFO_CARD> mST_INFO_CARDs { get; set; }
        public DbSet<MST_INFO_VAN_CARDMAP> mST_INFO_VAN_CARDMAPs { get; set; }
        public DbSet<MST_INFO_EASYPAY> mST_INFO_EASYPAYs { get; set; }
        public DbSet<MST_INFO_VAN_EASYPAYMAP> mST_INFO_VAN_EASYPAYMAPs { get; set; }
        public DbSet<MST_INFO_EMP> mST_INFO_EMPs { get; set; }
        public DbSet<MST_INFO_PRODUCT> mST_INFO_PRODUCTs { get; set; }
        public DbSet<MST_INFO_LCLASS> mST_INFO_LCLASSes { get; set; }
        public DbSet<MST_INFO_MCLASS> mST_INFO_MCLASSes { get; set; }
        public DbSet<MST_INFO_SLCASS> mST_INFO_SLCASSes { get; set; }
        public DbSet<MST_TUCH_CLASS> mST_TUCH_CLASSes { get; set; }
        public DbSet<MST_TUCH_PRODUCT> mST_TUCH_PRODUCTs { get; set; }
        public DbSet<MST_SHOP_FUNC_KEY> mST_SHOP_FUNC_KEYs { get; set; }
        public DbSet<MST_POS_FUNC_KEY> mST_POS_FUNC_KEYs { get; set; }
        public DbSet<MST_EMP_FUNC_KEY> mST_EMP_FUNC_KEYs { get; set; }
        public DbSet<MST_SIDE_DEPT_CLASS> mST_SIDE_DEPT_CLASSes { get; set; }
        public DbSet<MST_SIDE_DETP_CODE> mST_SIDE_DETP_CODEs { get; set; }
        public DbSet<MST_SIDE_SEL_GROUP> mST_SIDE_SEL_GROUPs { get; set; }
        public DbSet<MST_SIDE_SEL_CLASS> mST_SIDE_SEL_CLASSes { get; set; }
        public DbSet<MST_SIDE_SEL_CODE> mST_SIDE_SEL_CODEs { get; set; }
        public DbSet<MST_INFO_FIX_DC> mST_INFO_FIX_DCs { get; set; }
        public DbSet<MST_INFO_TICKET_CLASS> mST_INFO_TICKET_CLASSes { get; set; }
        public DbSet<MST_INFO_TICKET> mST_INFO_TICKETs { get; set; }
        public DbSet<MST_INFO_COUPON> mST_INFO_COUPONs { get; set; }
        public DbSet<MST_INFO_PRD_COUPON> mST_INFO_PRD_COUPONs { get; set; }
        public DbSet<MST_FORM_PRINTER> mST_FORM_PRINTERs { get; set; }
        public DbSet<MST_INFO_ACCOUNT> mST_INFO_ACCOUNTs { get; set; }
        public DbSet<MST_INFO_EXRATE> mST_INFO_EXRATEs { get; set; }
        public DbSet<MST_INFO_RTN_REASON> mST_INFO_RTN_REASONs { get; set; }
        public DbSet<MST_GLOB_PRODUCT_NAME> mST_GLOB_PRODUCT_NAMEs { get; set; }
        public DbSet<MST_GLOB_TOUH_CLASS> mST_GLOB_TOUH_CLASSes { get; set; }
        public DbSet<MST_GLOB_SIDE_MENU> mST_GLOB_SIDE_MENUs { get; set; }
        public DbSet<TRN_HEADER> tRN_HEADERs { get; set; }
        public DbSet<TRN_PRDT> tRN_PRDTs { get; set; }
        public DbSet<TRN_TENDERSEQ> tRN_TENDERSEQs { get; set; }
        public DbSet<TRN_CASH> tRN_CASHes { get; set; }
        public DbSet<TRN_CASHREC> tRN_CASHRECs { get; set; }
        public DbSet<TRN_CARD> tRN_CARDs { get; set; }
        public DbSet<TRN_PARTCARD> tRN_PARTCARDs { get; set; }
        public DbSet<TRN_GIFT> tRN_GIFTs { get; set; }
        public DbSet<TRN_FOODCPN> tRN_FOODCPNs { get; set; }
        public DbSet<TRN_EASYPAY> tRN_EASYPAYs { get; set; }
        public DbSet<TRN_POINTUSE> tRN_POINTUSEs { get; set; }
        public DbSet<TRN_POINTSAVE> tRN_POINTSAVEs { get; set; }
        public DbSet<TRN_PPCARD> tRN_PPCARDs { get; set; }

        public DbSet<ORD_HEADER> oRD_HEADERs { get; set; }
        public DbSet<ORD_PRDT> oRD_PRDTs { get; set; }
        public DbSet<ORD_TENDERSEQ> oRD_TENDERSEQs { get; set; }
        public DbSet<ORD_CASH> oRD_CASHes { get; set; }
        public DbSet<ORD_CARD> oRD_CARDs { get; set; }
        public DbSet<ORD_PARTCARD> oRD_PARTCARDs { get; set; }
        public DbSet<ORD_GIFT> oRD_GIFTs { get; set; }
        public DbSet<ORD_FOODCPN> oRD_FOODCPNs { get; set; }
        public DbSet<ORD_EASYPAY> oRD_EASYPAYs { get; set; }
        public DbSet<ORD_POINTUSE> oRD_POINTUSEs { get; set; }
        public DbSet<ORD_POINTSAVE> oRD_POINTSAVEs { get; set; }
        public DbSet<ORD_PPCARD> oRD_PPCARDs { get; set; }

        public DbSet<NTRN_PRECHARGE_HEADER> nTRN_PRECHARGE_HEADERs { get; set; }

        public DbSet<NTRN_PRECHARGE_CARD> nTRN_PRECHARGE_CARDs { get; set; }

        private string _connectString;

        public DataContext() : base(GetDbConnection(DapperORM.Firebird()), true)
        {
            _connectString = DapperORM.Firebird();
        }

        public void InitializeDatabase()
        {
            //System.Data.Entity.Database.SetInitializer<DataContext>(new MigrateDatabaseToLatestVersion<DataContext, GoPOS.Service.Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<MST_INFO_EMP>().ToTable("MST_INFO_EMP").HasKey(p => new { p.EMP_NO, p.SHOP_CODE });
            modelBuilder.Entity<MST_INFO_SHOP>().ToTable("MST_INFO_SHOP").HasKey(p => new { p.HD_SHOP_CODE, p.SHOP_CODE });
            modelBuilder.Entity<POS_STATUS>().ToTable("POS_STATUS").HasKey(p => new { p.SHOP_CODE, p.POS_NO });
            modelBuilder.Entity<MST_CNFG_POS>().ToTable("MST_CNFG_POS").HasKey(p => new { p.SHOP_CODE, p.POS_NO, p.ENV_SET_CODE });
            modelBuilder.Entity<SETT_POSACCOUNT>().ToTable("SETT_POSACCOUNT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.REGI_SEQ });
            modelBuilder.Entity<POS_KEY_MANG>().ToTable("POS_KEY_MANG").HasKey(p => new { p.HD_SHOP_CODE, p.SHOP_CODE, p.POS_NO });
            modelBuilder.Entity<POS_MST_MANG>().ToTable("POS_MST_MANG").HasKey(p => new { p.RECV_SEQ, p.MST_ID });
            modelBuilder.Entity<ORD_CARD>().ToTable("ORD_CARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_PARTCARD>().ToTable("ORD_PARTCARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_EASYPAY>().ToTable("ORD_EASYPAY").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_GIFT>().ToTable("ORD_GIFT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_PRDT>().ToTable("TRN_PRDT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_CASH>().ToTable("ORD_CASH").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_CASHREC>().ToTable("TRN_CASHREC").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<POS_POST_MANG>()
                .ToTable("POS_POST_MANG").HasKey(p => new
                {
                    p.SHOP_CODE,
                    p.SALE_DATE,
                    p.POS_NO,
                    p.REGI_SEQ,
                    p.POST_FLAG,
                    p.BILL_FLAG,
                    p.BILL_NO
                });
            modelBuilder.Entity<MST_INFO_PRD_JOINCARD>().ToTable("MST_INFO_PRD_JOINCARD").HasKey(p => new { p.SHOP_CODE, p.JCD_CODE, p.STYLE_PRD_CODE });
            modelBuilder.Entity<MST_COMM_CODE>().ToTable("MST_COMM_CODE").HasKey(p => new { p.COM_CODE_FLAG, p.COM_CODE });
            modelBuilder.Entity<MST_COMM_CODE_SHOP>().ToTable("MST_COMM_CODE_SHOP").HasKey(p => new { p.COM_CODE_FLAG, p.COM_CODE, p.SHOP_CODE });
            modelBuilder.Entity<MST_CNFG_DETAIL>().ToTable("MST_CNFG_DETAIL").HasKey(p => new { p.ENV_SET_CODE, p.ENV_VALUE_CODE });
            modelBuilder.Entity<MST_CNFG_SHOP>().ToTable("MST_CNFG_SHOP").HasKey(p => new { p.ENV_SET_CODE, p.SHOP_CODE });
            modelBuilder.Entity<MST_INFO_POS>().ToTable("MST_INFO_POS").HasKey(p => new { p.SHOP_CODE, p.POS_NO });
            modelBuilder.Entity<MST_INFO_VAN_POS>().ToTable("MST_INFO_VAN_POS").HasKey(p => new { p.SHOP_CODE, p.POS_NO });
            modelBuilder.Entity<MST_INFO_VAN_CARDMAP>().ToTable("MST_INFO_VAN_CARDMAP").HasKey(p => new { p.VAN_CODE, p.VAN_CRDCP_CODE });
            modelBuilder.Entity<MST_INFO_VAN_EASYPAYMAP>().ToTable("MST_INFO_VAN_EASYPAYMAP").HasKey(p => new { p.VAN_CODE, p.VAN_PAYCP_CODE });
            modelBuilder.Entity<MST_INFO_LCLASS>().ToTable("MST_INFO_LCLASS").HasKey(p => new { p.SHOP_CODE, p.LCLASS_CODE });
            modelBuilder.Entity<MST_INFO_MCLASS>().ToTable("MST_INFO_MCLASS").HasKey(p => new { p.SHOP_CODE, p.MCLASS_CODE });
            modelBuilder.Entity<MST_INFO_SLCASS>().ToTable("MST_INFO_SLCASS").HasKey(p => new { p.SHOP_CODE, p.SCLASS_CODE });
            modelBuilder.Entity<MST_TUCH_CLASS>().ToTable("MST_TUCH_CLASS").HasKey(p => new { p.SHOP_CODE, p.TU_FLAG, p.TU_CLASS_CODE });
            modelBuilder.Entity<MST_TUCH_PRODUCT>().ToTable("MST_TUCH_PRODUCT").HasKey(p => new { p.SHOP_CODE, p.TU_FLAG, p.TU_CLASS_CODE, p.TU_KEY_CODE, p.TU_PAGE });
            modelBuilder.Entity<MST_SHOP_FUNC_KEY>().ToTable("MST_SHOP_FUNC_KEY").HasKey(p => new { p.SHOP_CODE, p.FK_NO });
            modelBuilder.Entity<MST_POS_FUNC_KEY>().ToTable("MST_POS_FUNC_KEY").HasKey(p => new { p.SHOP_CODE, p.FN_NO });
            modelBuilder.Entity<MST_EMP_FUNC_KEY>().ToTable("MST_EMP_FUNC_KEY").HasKey(p => new { p.SHOP_CODE, p.EMP_FLAG, p.FK_NO });
            modelBuilder.Entity<MST_SIDE_DEPT_CLASS>().ToTable("MST_SIDE_DEPT_CLASS").HasKey(p => new { p.SHOP_CODE, p.SDA_CLASS_CODE });
            modelBuilder.Entity<MST_SIDE_DETP_CODE>().ToTable("MST_SIDE_DETP_CODE").HasKey(p => new { p.SHOP_CODE, p.SDA_CODE });
            modelBuilder.Entity<MST_SIDE_SEL_GROUP>().ToTable("MST_SIDE_SEL_GROUP").HasKey(p => new { p.SHOP_CODE, p.SDS_GROUP_CODE });
            modelBuilder.Entity<MST_SIDE_SEL_CLASS>().ToTable("MST_SIDE_SEL_CLASS").HasKey(p => new { p.SHOP_CODE, p.SDS_CLASS_CODE });
            modelBuilder.Entity<MST_SIDE_SEL_CODE>().ToTable("MST_SIDE_SEL_CODE").HasKey(p => new { p.SHOP_CODE, p.SDS_CODE });
            modelBuilder.Entity<MST_INFO_FIX_DC>().ToTable("MST_INFO_FIX_DC").HasKey(p => new { p.SHOP_CODE, p.DC_NO });
            modelBuilder.Entity<MST_INFO_TICKET_CLASS>().ToTable("MST_INFO_TICKET_CLASS").HasKey(p => new { p.SHOP_CODE, p.TK_CLASS_FLAG, p.TK_CLASS_CODE });
            modelBuilder.Entity<MST_INFO_TICKET>().ToTable("MST_INFO_TICKET").HasKey(p => new { p.SHOP_CODE, p.TK_CLASS_FLAG, p.TK_GFT_CODE });
            modelBuilder.Entity<MST_INFO_JOINCARD>().ToTable("MST_INFO_JOINCARD").HasKey(p => new { p.SHOP_CODE, p.JCD_CODE });
            modelBuilder.Entity<MST_INFO_COUPON>().ToTable("MST_INFO_COUPON").HasKey(p => new { p.SHOP_CODE, p.TK_CPN_CODE });
            modelBuilder.Entity<MST_INFO_PRD_COUPON>().ToTable("MST_INFO_PRD_COUPON").HasKey(p => new { p.SHOP_CODE, p.TK_CPN_CODE, p.PRD_CODE });
            modelBuilder.Entity<MST_FORM_PRINTER>().ToTable("MST_FORM_PRINTER").HasKey(p => new { p.SHOP_CODE, p.PRT_CLASS_CODE });
            modelBuilder.Entity<MST_INFO_ACCOUNT>().ToTable("MST_INFO_ACCOUNT").HasKey(p => new { p.SHOP_CODE, p.ACCNT_CODE });
            modelBuilder.Entity<MST_INFO_EXRATE>().ToTable("MST_INFO_EXRATE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.EX_CODE });
            modelBuilder.Entity<MST_INFO_RTN_REASON>().ToTable("MST_INFO_RTN_REASON").HasKey(p => new { p.SHOP_CODE, p.MSG_FLAG, p.MSG_CODE });
            modelBuilder.Entity<MST_GLOB_PRODUCT_NAME>().ToTable("MST_GLOB_PRODUCT_NAME").HasKey(p => new { p.SHOP_CODE, p.PRD_CODE, p.NATION_CODE });
            modelBuilder.Entity<MST_GLOB_TOUH_CLASS>().ToTable("MST_GLOB_TOUH_CLASS").HasKey(p => new { p.SHOP_CODE, p.TU_CLASS_CODE, p.NATION_CODE, p.POS_NO });
            modelBuilder.Entity<MST_GLOB_SIDE_MENU>().ToTable("MST_GLOB_SIDE_MENU").HasKey(p => new { p.SHOP_CODE, p.SDS_CLASS_CODE, p.NATION_CODE });
            modelBuilder.Entity<TRN_HEADER>().ToTable("TRN_HEADER").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO });
            modelBuilder.Entity<TRN_TENDERSEQ>().ToTable("TRN_TENDERSEQ").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.PAY_SEQ_NO });
            modelBuilder.Entity<TRN_CASH>().ToTable("TRN_CASH").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_CASHREC>().ToTable("TRN_CASHREC").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_CARD>().ToTable("TRN_CARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_PARTCARD>().ToTable("TRN_PARTCARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_GIFT>().ToTable("TRN_GIFT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_FOODCPN>().ToTable("TRN_FOODCPN").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_EASYPAY>().ToTable("TRN_EASYPAY").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_HEADER>().ToTable("ORD_HEADER").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO });
            modelBuilder.Entity<ORD_PRDT>().ToTable("ORD_PRDT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_TENDERSEQ>().ToTable("ORD_TENDERSEQ").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.PAY_SEQ_NO });
            modelBuilder.Entity<ORD_CASH>().ToTable("ORD_CASH").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_CARD>().ToTable("ORD_CARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_PARTCARD>().ToTable("ORD_PARTCARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_GIFT>().ToTable("ORD_GIFT").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_FOODCPN>().ToTable("ORD_FOODCPN").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_EASYPAY>().ToTable("ORD_EASYPAY").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<MST_INFO_KDS_PRD>().ToTable("MST_INFO_KDS_PRD").HasKey(p => new { p.SHOP_CODE, p.PRD_CODE, p.KDS_NO });
            modelBuilder.Entity<MST_INFO_KDS>().ToTable("MST_INFO_KDS").HasKey(p => new { p.SHOP_CODE, p.KDS_NO });
            modelBuilder.Entity<ORD_POINTSAVE>().ToTable("ORD_POINTSAVE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_POINTSAVE>().ToTable("TRN_POINTSAVE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<ORD_POINTUSE>().ToTable("ORD_POINTUSE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.ORDER_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_POINTUSE>().ToTable("TRN_POINTUSE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });

            modelBuilder.Entity<MST_TABLE_DEPT>().ToTable("MST_TABLE_DEPT").HasKey(p => new { p.SHOP_CODE, p.TABLE_FLAG, p.PROPERTY_CODE});
            modelBuilder.Entity<MST_TABLE_GROUP>().ToTable("MST_TABLE_GROUP").HasKey(p => new { p.SHOP_CODE,p.TG_CODE });
            modelBuilder.Entity<MST_TABLE_INFO>().ToTable("MST_TABLE_INFO").HasKey(p => new { p.SHOP_CODE, p.TABLE_CODE });

            modelBuilder.Entity<TRN_POINTUSE>().ToTable("TRN_POINTUSE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });
            modelBuilder.Entity<TRN_POINTUSE>().ToTable("TRN_POINTUSE").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.BILL_NO, p.SEQ_NO });


            modelBuilder.Entity<NTRN_PRECHARGE_HEADER>().ToTable("NTRN_PRECHARGE_HEADER").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.SALE_NO, p.CST_NO });
            modelBuilder.Entity<NTRN_PRECHARGE_CARD>().ToTable("NTRN_PRECHARGE_CARD").HasKey(p => new { p.SHOP_CODE, p.SALE_DATE, p.POS_NO, p.SALE_NO, p.SEQ_NO });

        }



        public static DbConnection GetDbConnection(string connect)
        {
            var connectx = new FbConnection(connect);
            return connectx;
        }

        #region ExceMethod

        public IEnumerable<T> ExecuteQuery<T>(string tsql)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query<T>(tsql);
            }
        }
        public IEnumerable<T> ExecuteQuery<T>(string tsql, DynamicParameters parameters)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query<T>(tsql, parameters);
            }
        }
        public IEnumerable<T> ExecuteQuery<T>(string tsql, DynamicParameters parameters, CommandType type)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query<T>(tsql, parameters, commandType: type);
            }
        }
        public IEnumerable<dynamic> ExecuteQuery(string tsql)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query(tsql);
            }
        }
        public IEnumerable<dynamic> ExecuteQuery(string tsql, DynamicParameters parameters)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query(tsql, parameters);
            }
        }
        public IEnumerable<dynamic> ExecuteQuery(string tsql, DynamicParameters parameters, CommandType command)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.Query(tsql, parameters, commandType: command);
            }
        }

        public void ExecuteNonQuery(string tsql)
        {
            using (var connect = new FbConnection(_connectString))
            {
                connect.Execute(tsql);
            }
        }

        public T FirstOrDefault<T>(string tsql)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.QueryFirstOrDefault<T>(tsql);
            }
        }
        public T FirstOrDefault<T>(string tsql, DynamicParameters parameters)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.QueryFirstOrDefault<T>(tsql, parameters);
            }
        }
        public dynamic FirstOrDefault(string tsql)
        {
            using (var connect = new SqlConnection(_connectString))
            {
                return connect.QueryFirstOrDefault(tsql);
            }
        }

        #endregion

        public IReadOnlyList<KeyPropertyInfo> GetPrimaryKeyProperties(Type clrEntityType)
        {

            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var metadata = objectContext.MetadataWorkspace;
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));
            var entityType = metadata.GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == clrEntityType);
            return entityType.KeyProperties
                .Select(p => new KeyPropertyInfo
                {
                    Name = p.Name,
                    ClrType = p.PrimitiveType.ClrEquivalentType
                })
                .ToList();
        }

        public Expression<Func<T, bool>> BuildKeyPredicate<T>(object[] id)
        {
            var keyProperties = GetPrimaryKeyProperties(typeof(T));
            var parameter = Expression.Parameter(typeof(T), "e");
            var body = keyProperties
                // e => e.PK[i] == id[i]
                .Select((p, i) => Expression.Equal(
                    Expression.Property(parameter, p.Name),
                    Expression.Convert(
                        Expression.PropertyOrField(Expression.Constant(new { id = id[i] }), "id"),
                        p.ClrType)))
                .Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }


    }


    public static class DbContextExts
    {
        public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(this DbSet<TEntity> dbSet, DataContext context, object[] id)
            where TEntity : class
        {
            return dbSet.Where(context.BuildKeyPredicate<TEntity>(id));
        }
    }

    public struct KeyPropertyInfo
    {
        public string Name;
        public Type ClrType;
    }

}
