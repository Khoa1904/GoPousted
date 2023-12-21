 -- region CardPayment
<OrderPayCoprtnDscnt_JoinProducts>
SELECT JP.*, P.PRD_NAME AS STYLE_PRD_NAME
FROM MST_INFO_PRD_JOINCARD JP
	INNER JOIN MST_INFO_PRODUCT P ON JP.SHOP_CODE = P.SHOP_CODE
					AND JP.STYLE_PRD_CODE = P.PRD_CODE
WHERE JP.SHOP_CODE = @SHOP_CODE AND JP.JCD_CODE = @JCD_CODE;
</OrderPayCoprtnDscnt_JoinProducts>
<OrderPayCoprtnDscnt_JoinCardsSelect>
SELECT JC.*
FROM MST_INFO_JOINCARD JC
WHERE JC.SHOP_CODE = @SHOP_CODE AND 
	CAST (JC.VALID_F_DATE AS INTEGER) < @TODAY
	AND CAST (JC.VALID_T_DATE  AS INTEGER) > @TODAY
ORDER BY JC.JCD_CODE ASC;
</OrderPayCoprtnDscnt_JoinCardsSelect>

<GetProdList>
SELECT  
                                P.SHOP_CODE
                                ,P.SALE_DATE
                                ,P.ORDER_NO 
                       --         ,ORDER_DTL_NO 
                                ,P.PRD_CODE 
                                ,M.PRD_NAME 
                                ,P.ORDER_SEQ_NO
                                ,T.NORMAL_UPRC 
                                ,P.SALE_QTY 
                                ,P.DC_AMT 
                                ,P.DCM_SALE_AMT
                                ,T.SALE_MG_CODE
                       --         ,SALE_MG_NAME
                                ,F.DC_TYPE_FLAG
                                ,P.DLV_PACK_FLAG 
                                ,P.TAX_YN 
                                ,P.CORNER_CODE 
                                ,P.ORG_SALE_MG_CODE
                                ,M.TIP_MENU_YN 
                                ,P.SVC_TIP_AMT 
                                ,P.VAT_AMT 
                                ,P.SALE_UPRC 
                                ,M.BRAND_CODE 
                                ,P.PRD_TYPE_FLAG  
                                ,P.DC_AMT_GEN 
                                ,P.DC_AMT_SVC 
                                ,P.DC_AMT_JCD 
                                ,P.DC_AMT_CPN 
                                ,P.DC_AMT_CST 
                                ,P.DC_AMT_FOD 
                                ,P.DC_AMT_CRD 
                                ,P.DC_AMT_PRM 
                                ,P.DC_AMT_PACK
                                ,T.DC_AMT_LYT 
                                ,T.DC_AMT_TAX 
                                ,T.CST_SALE_POINT
                                ,T.CST_USE_POINT 
                                ,T.SDA_CODE 
                                ,T.SDS_ORG_DTL_NO
                                ,T.ORG_SALE_UPRC 
                                ,T.ETC_AMT 
                                ,T.SVC_CODE 
                                ,T.TK_CPN_CODE 
                                ,T.DC_SEQ_TYPE 
                                ,T.DC_SEQ_AMT 
                                ,T.DC_SEQ_FLAG 
                                ,T.DC_SEQ_RATE 
                                ,M.REMARK 
                                ,M.SIDE_MENU_YN
                                ,M.TOGO_CHARGE 
                                ,T.AFFILIATE_UPRC 
                                ,T.MCP_BAR_CODE 
                          --    ,STAMP_SALE_QTY    
                                ,M.STAMP_USE_YN 
                                ,M.STAMP_USE_QTY 
                                ,S.SDS_CLASS_CODE 
                                ,T.DOUBLE_CODE
                                ,T.DOUBLE_AMT 
                                ,T.PRD_WEIGHT 
                                ,T.TAX_RFND_AMT
                                ,T.TAX_RFND_FEE
                                ,M.PRD_DC_FLAG 
                                ,T.IF_CPN_CODE 
                                ,T.DC_REASON_CODE 
                                ,T.DC_REASON_NAME 
                                ,T.SDS_PARENT_CODE 
                                ,M.DC_RULE_FLAG 
                                ,M.DEPOSIT_USE_YN
                                ,M.DEPOSIT_TYPE 
FROM    TRN_PRDT T INNER JOIN ORD_PRDT P            ON T.SHOP_CODE = P.SHOP_CODE AND T.SALE_DATE = P.SALE_DATE
                   INNER JOIN MST_INFO_PRODUCT M    ON P.PRD_CODE  = M.PRD_CODE                    
                   INNER JOIN MST_INFO_FIX_DC  F    ON P.SHOP_CODE = F.SHOP_CODE AND F.USE_YN   = '1'
                   INNER JOIN MST_SIDE_SEL_CLASS S  ON P.SHOP_CODE = S.SHOP_CODE AND S.USE_YN   = '1' 
             --    INNER JOIN MST_INFO_EMP E        ON T.SALE_MG_CODE = E.EMP_NO
WHERE   P.SHOP_CODE = @SHOP_CODE
AND     (@SHOP_CODE IS NULL OR P.SHOP_CODE = @SHOP_CODE) -- ?
AND     (@ODER_NO IS NULL OR P.SHOP_CODE = @ORDER_NO)    -- ?
</GetProdList>

<ApprReqUnionPay>
SELECT 
                T.SHOP_CODE 
                ,T.SALE_DATE 
                ,T.POS_NO 
                ,T.BILL_NO  
                ,T.LINE_NO  
                ,T.SEQ_NO  
                ,T.REGI_SEQ 
                ,T.SALE_YN  
                ,T.VAN_CODE 
                ,T.CORNER_CODE 
                ,T.APPR_PROC_FLAG                  
                ,T.CRD_CARD_NO         
                ,T.APPR_REQ_AMT        
                ,T.SVC_TIP_AMT         
                ,T.VAT_AMT             
                ,T.INST_MM_FLAG        
                ,T.INST_MM_CNT         
                ,T.VALID_TERM          
                ,T.SIGN_PAD_YN         
                ,T.CARD_IN_FLAG        
                ,T.APPR_DATE           
                ,T.APPR_TIME           
                ,T.APPR_NO             
                ,T.CRDCP_CODE          
                ,T.ISS_CRDCP_CODE      
                ,T.ISS_CRDCP_NAME      
                ,T.PUR_CRDCP_CODE      
                ,T.PUR_CRDCP_NAME      
                ,T.APPR_AMT            
                ,T.APPR_DC_AMT         
                ,T.APPR_MSG            
                ,T.VAN_TERM_NO         
                ,T.VAN_SLIP_NO         
                ,T.CRDCP_TERM_NO       
                ,T.ORG_APPR_DATE       
                ,T.ORG_APPR_NO         
                ,T.APPR_LOG_NO         
                ,T.INSERT_DT           
                ,T.EMP_NO              
                ,T.PRE_PAY_FLAG        
                ,T.LYT_APPR_FLAG       
                ,T.LYT_APPR_INFO       
                ,T.SPECIAL_CARD_FLA
                ,T.CSN_BANK_NO         
                ,T.JCD_SAVE_FLAG       
                ,T.CNMK_CODE           
                ,T.PRINT_MSG           
                ,T.NO_VAT_AMT          
                ,T.TRAN_TYPE_FLAG      
                ,T.TRAN_DC_NAME        
                ,T.TRAN_DC_AMT         
                ,T.VAN_SER_NO          
                ,T.SLIP_TYPE_FLAG      
                ,T.TRAN_UNIQUE_NO      
                ,T.TRAN_ORDER_NO       
                ,T.DCC_EX_RATE_CODE
                ,T.DCC_RCV_TEXT        
                ,T.DEPOSIT_AMT
FROM    TRN_CARD       
WHERE   SHOP_CODE= @SHOP_CODE 
    AND SALE_DATE= @SALE_DATE 
    AND POS_NO   = @POS_NO    
    AND BILL_NO  = @BILL_NO   
    AND LINE_NO  = @LINE_NO   
    AND SEQ_NO   = @SEQ_NO    

<ApprReqUnionPay/>

-- region CashPayment

<UpdateScdTableLock>
BEGIN TRY 
    UPDATE  MST_TABLE_DEPT
    
    SET
                LOCK_FLAG         =  @LOCK_FLAG     
                OUTPUT inserted.LOCK_POS_NO INTO @POSNO
        FROM    MST_TABLE_DEPT 
        WHERE   SHOP_CODE   =@SHOP_CODE
          AND   TABLE_CODE  =@TABLE_CODE
          AND   TG_CODE     =@TG_CODE

    SET @CODE = 1;
END TRY

BEGIN CATCH
    SET @CODE =0;
END CATCH

SELECT @CODE AS RESULT_CODE, @POSNO AS R_LOCK_POS_NO;
<UpdateScdTableLock/>

<UpdateScdTableStatus>
BEGIN TRY 
    UPDATE  MST_TABLE_DEPT
    
    SET
                STATUS_FLAG         =  @STATUS_FLAG     
                OUTPUT inserted.LOCK_POS_NO INTO @POSNO
        FROM    MST_TABLE_DEPT 
        WHERE   SHOP_CODE   =@SHOP_CODE
          AND   TABLE_CODE  =@TABLE_CODE
          AND   TG_CODE     =@TG_CODE

    SET @CODE = 1;
END TRY

BEGIN CATCH
    SET @CODE =0;
END CATCH

SELECT @CODE AS RESULT_CODE, @POSNO AS R_LOCK_POS_NO;
<UpdateScdTableStatus/>

<InsertProdOrderHdr>
        -- table ORD_HEADER
INSERT INTO ORD_HEADER (
             SHOP_CODE
            ,SALE_DATE
            ,POS_NO
            ,ORDER_NO
            ,DLV_ORDER_FLAG
            ,ORDER_END_FLAG
            ,FD_TBL_CODE
            ,TOT_SALE_AMT
            ,TOT_DC_AMT
            ,SVC_TIP_AMT
            ,TOT_ETC_AMT
            ,DCM_SALE_AMT
            ,VAT_SALE_AMT
            ,VAT_AMT
            ,NO_VAT_SALE_AMT
            ,NO_TAX_SALE_AMT
            ,EXP_PAY_AMT
            ,GST_PAY_AMT
            ,RET_PAY_AMT
            ,CASH_BILL_AMT
            ,ORG_BILL_NO
            ,CST_NO
            ,DC_GEN_AMT
            ,DC_SVC_AMT
            ,DC_PCD_AMT
            ,DC_CPN_AMT
            ,DC_CST_AMT
            ,DC_TFD_AMT
            ,DC_PRM_AMT
            ,DC_CRD_AMT
            ,DC_PACK_AMT
            ,FD_GST_FLAG_YN
            ,FD_GST_FLAG_1
            ,FD_GST_FLAG_2
            ,EMP_NO
            ,SEND_FLAG
            ,SEND_DT
            ,INSERT_DT)

VALUES(
             @SHOP_CODE                 --SHOP_CODE   
            ,@SALE_DATE                 --SALE_DATE   
            ,@POS_NO                    --POS_NO   
            ,@ORDER_NO                  --ORDER_NO
            ,@DLV_ORDER_FLAG            --DLV_ORDER_FLAG   
            ,@ORDER_END_FLAG            --ORDER_END_FLAG   
            ,@FD_TBL_CODE               --FD_TBL_CODE   
            ,@TOT_SALE_AMT              --TOT_SALE_AMT   
            ,@TOT_DC_AMT                --TOT_DC_AMT   
            ,@SVC_TIP_AMT               --SVC_TIP_AMT   
            ,@TOT_ETC_AMT               --TOT_ETC_AMT   
            ,@DCM_SALE_AMT              --DCM_SALE_AMT   
            ,@VAT_SALE_AMT              --VAT_SALE_AMT   
            ,@VAT_AMT                   --VAT_AMT   
            ,@NO_VAT_SALE_AMT           --NO_VAT_SALE_AMT   
            ,@NO_TAX_SALE_AMT           --NO_TAX_SALE_AMT   
            ,@EXP_PAY_AMT               --EXP_PAY_AMT   
            ,@GST_PAY_AMT               --GST_PAY_AMT   
            ,@RET_PAY_AMT               --RET_PAY_AMT
            ,@CASH_BILL_AMT             --CASH_BILL_AMT
            ,CASE WHEN @BILL_NO         IS NOT NULL THEN @BILL_NO           ELSE ''    END      --ORG_BILL_NO                                               
            ,CASE WHEN @CST_NO          IS NOT NULL THEN @CST_NO            ELSE ''    END      --CST_NO        
            ,@DC_GEN_AMT                --DC_GEN_AMT   
            ,@DC_SVC_AMT                --DC_SVC_AMT   
            ,@DC_PCD_AMT                --DC_PCD_AMT   
            ,@DC_CPN_AMT                --DC_CPN_AMT   
            ,@DC_CST_AMT                --DC_CST_AMT   
            ,@DC_TFD_AMT                --DC_TFD_AMT   
            ,@DC_PRM_AMT                --DC_PRM_AMT   
            ,@DC_CRD_AMT                --DC_CRD_AMT   
            ,@DC_PACK_AMT               --DC_PACK_AMT         
            ,@FD_GST_FLAG_YN            --FD_GST_FLAG_YN
            ,@FD_GST_FLAG_1             --FD_GST_FLAG_1
            ,@FD_GST_FLAG_2             --FD_GST_FLAG_2
            ,@EMP_NO                    --EMP_NO
            ,@SEND_FLAG                 --SEND_FLAG
            ,@SEND_DT                   --SEND_DT
            ,getdate()                  --INSERT_DT)
            )

                            -- table TRN_DELIVERY

    INSERT INTO TRN_DELIVERY (SHOP_CODE
                             ,SALE_DATE
                             ,POS_NO
                             ,ORDER_NO
                             ,DLV_PAY_TYPE_FLAG
                             ,DLV_IDT_NO       
                             ,DLV_REMARK       
                             ,PAGER_BELL_NO    
                             ,ORG_FD_TBL_CODE  
                             ,DLV_START_DT     
                             ,DLV_EMP_NO  )         
    VALUES(      @SHOP_CODE
                ,@SALE_DATE
                ,@POS_NO
                ,@ORDER_NO
                ,@DLV_PAY_TYPE_FLAG                                                                               
                ,CASE WHEN @DLV_IDT_NO              IS NOT NULL THEN @DLV_IDT_NO               ELSE ''    END                       
                ,CASE WHEN @DLV_REMARK              IS NOT NULL THEN @DLV_REMARK               ELSE ''    END                                                                                                              
                ,CASE WHEN @PAGER_BELL_NO           IS NOT NULL THEN @PAGER_BELL_NO            ELSE ''    END      
                ,CASE WHEN @ORG_FD_TBL_CODE         IS NOT NULL THEN @ORG_FD_TBL_CODE          ELSE ''    END     
                ,CASE WHEN @DLV_START_DT            IS NOT NULL THEN @DLV_START_DT             ELSE ''    END     
                ,CASE WHEN @DLV_EMP_NO              IS NOT NULL THEN @DLV_EMP_NO               ELSE ''    END     
    )


                            -- table ORDER_DELIVERY

    INSERT INTO ORD_DELIVERY (SHOP_CODE
                             ,SALE_DATE
                             ,POS_NO
                             ,ORDER_NO
                             ,DLV_PAY_TYPE_FLAG
                             ,DLV_IDT_NO       
                             ,DLV_REMARK       
                             ,PAGER_BELL_NO    
                             ,ORG_FD_TBL_CODE  
                             ,DLV_EMP_NO
                             ,INSERT_DT)
    VALUES (     @SHOP_CODE
                ,@SALE_DATE
                ,@POS_NO
                ,@ORDER_NO
                ,@DLV_PAY_TYPE_FLAG                                                                                
                ,CASE WHEN @DLV_IDT_NO              IS NOT NULL THEN @DLV_IDT_NO               ELSE ''    END                        
                ,CASE WHEN @DLV_REMARK              IS NOT NULL THEN @DLV_REMARK               ELSE ''    END                                                                                                               
                ,CASE WHEN @PAGER_BELL_NO           IS NOT NULL THEN @PAGER_BELL_NO            ELSE ''    END       
                ,CASE WHEN @ORG_FD_TBL_CODE         IS NOT NULL THEN @ORG_FD_TBL_CODE          ELSE ''    END      
                ,CASE WHEN @DLV_EMP_NO              IS NOT NULL THEN @DLV_EMP_NO               ELSE ''    END      
    )



                           -- table TRN_HEADER  
INSERT INTO TRN_HEADER (
                        CASH_AMT
                        ,CRD_CARD_AMT
                        ,WES_AMT
                        ,TK_GFT_AMT
                        ,TK_FOD_AMT
                        ,CST_POINT_AMT
                        ,JCD_CARD_AMT
                        ,RFC_AMT
                        ,CST_NO
                        ,CST_SALE_POINT     
                        ,REPAY_CASH_AMT
                        ,REPAY_TK_GFT_AMT
                        ,CASH_BILL_AMT
                        ,FD_TBL_CODE
                        ,FD_GST_CNT_T
                        ,FD_GST_CNT_1
                        ,FD_GST_CNT_2
                        ,FD_GST_CNT_3
                        ,FD_GST_CNT_4
                        ,ORDER_NO
                        ,INSERT_DT
                        ,CST_NAME
                        ,DLV_EMP_NO
                        ,DLV_START_DT
                        ,DLV_PAYIN_EMP_NO
                        ,DLV_PAYIN_DT
                        ,NEW_DLV_ADDR_YN
                        ,DLV_ADDR
                        ,DLV_ADDR_DTL
                        ,NEW_DLV_TEL_NO_YN
                        ,DLV_CL_CODE
                        ,DLV_CM_CODE
                        ,TRAVEL_CODE
                        ,RSV_NO
                        ,PRE_PAY_CASH
                        ,PRE_PAY_CARD
                        ,RSV_USER_NAME
                        ,RSV_USER_TEL_NO
                        ,CST_AVL_POINT  
                        ,AFFILIATE_CODE
                        ,LOCAL_POINT_YN
                        ,MCP_AMT
                        ,PCD_CARD_AMT
                        ,CST_ANN_POINT          
                        ,CST_NEW_POINT
                        ,CST_USE_POINT
                        ,CST_DATA
                        ,DLV_BOWLIN_YN
                        ,DLV_BOWLIN_EMP_NO
                        ,DLV_BOWLIN_DT
                        ,PPC_CARD_AMT
                        ,EGIFT_AMT
                        ,MOBILE_ORDER_FLAG
                        ,MOBILE_ORDER_NO
                        ,RFND_TYPE_CODE
                        ,TAX_RFND_AMT
                        ,TAX_RFND_FEE
                        ,DC_TAX_AMT
                        ,DUTCH_ORDER_NO
                        ,O2O_AMT
                        ,DLV_SAFE_TEL_NO
                        ,DLV_EXPIRE_TIME
                        ,EXTERN_GUEST_FLAG
                        ,DCC_EX_RATE_CODE
                        ,DCC_RCV_TEXT
                        ,SP_PAY_AMT
                        ,HOOK_ORDER_CHANNEL_TYPE
                        ,DEPOSIT_AMT)
VALUES(
             CASE WHEN @CASH_AMT                IS NOT NULL THEN @CASH_AMT                 ELSE 0     END       --CASH_AMT
            ,CASE WHEN @CRD_CARD_AMT            IS NOT NULL THEN @CRD_CARD_AMT             ELSE 0     END       --CRD_CARD_AMT
            ,CASE WHEN @WES_AMT                 IS NOT NULL THEN @WES_AMT                  ELSE 0     END       --WES_AMT
            ,CASE WHEN @TK_GFT_AMT              IS NOT NULL THEN @TK_GFT_AMT               ELSE 0     END       --TK_GFT_AMT
            ,CASE WHEN @TK_FOD_AMT              IS NOT NULL THEN @TK_FOD_AMT               ELSE 0     END       --TK_FOD_AMT
            ,CASE WHEN @CST_POINT_AMT           IS NOT NULL THEN @CST_POINT_AMT            ELSE 0     END       --CST_POINT_AMT
            ,CASE WHEN @JCD_CARD_AMT            IS NOT NULL THEN @JCD_CARD_AMT             ELSE 0     END       --JCD_CARD_AMT
            ,CASE WHEN @RFC_AMT                 IS NOT NULL THEN @RFC_AMT                  ELSE 0     END       --RFC_AMT
            ,CASE WHEN @CST_NO                  IS NOT NULL THEN @CST_NO                   ELSE ''    END       --CST_NO
            ,CASE WHEN @CST_SALE_POINT          IS NOT NULL THEN @CST_SALE_POINT           ELSE 0     END       --CST_SALE_POINT     
            ,CASE WHEN @REPAY_CASH_AMT          IS NOT NULL THEN @REPAY_CASH_AMT           ELSE 0     END       --REPAY_CASH_AMT
            ,CASE WHEN @REPAY_TK_GFT_AMT        IS NOT NULL THEN @REPAY_TK_GFT_AMT         ELSE 0     END       --REPAY_TK_GFT_AMT
            ,CASE WHEN @CASH_BILL_AMT           IS NOT NULL THEN @CASH_BILL_AMT            ELSE 0     END       --CASH_BILL_AMT
            ,CASE WHEN @FD_TBL_CODE     IS NOT NULL THEN @FD_TBL_CODE                      ELSE '000' END       --FD_TBL_CODE
            ,CASE WHEN @FD_GST_CNT_T            IS NOT NULL THEN @FD_GST_CNT_T             ELSE 0     END       --FD_GST_CNT_T
            ,CASE WHEN @FD_GST_CNT_1            IS NOT NULL THEN @FD_GST_CNT_1             ELSE 0     END       --FD_GST_CNT_1
            ,CASE WHEN @FD_GST_CNT_2            IS NOT NULL THEN @FD_GST_CNT_2             ELSE 0     END       --FD_GST_CNT_2
            ,CASE WHEN @FD_GST_CNT_3            IS NOT NULL THEN @FD_GST_CNT_3             ELSE 0     END       --FD_GST_CNT_3
            ,CASE WHEN @FD_GST_CNT_4            IS NOT NULL THEN @FD_GST_CNT_4             ELSE 0     END       --FD_GST_CNT_4
            ,CASE WHEN @ORDER_NO                IS NOT NULL THEN @ORDER_NO                 ELSE ''    END       --ORDER_NO
            ,getdate()                                                                                          --INSERT_DT
            ,CASE WHEN @CST_NAME                IS NOT NULL THEN @CST_NAME                 ELSE ''    END       --CST_NAME
            ,CASE WHEN @DLV_EMP_NO              IS NOT NULL THEN @DLV_EMP_NO               ELSE ''    END       --DLV_EMP_NO
            ,CASE WHEN @DLV_START_DT            IS NOT NULL THEN @DLV_START_DT             ELSE ''    END       --DLV_START_DT
            ,CASE WHEN @DLV_PAYIN_EMP_NO        IS NOT NULL THEN @DLV_PAYIN_EMP_NO         ELSE ''    END       --DLV_PAYIN_EMP_NO
            ,CASE WHEN @DLV_PAYIN_DT            IS NOT NULL THEN @DLV_PAYIN_DT             ELSE ''    END       --DLV_PAYIN_DT
            ,CASE WHEN @NEW_DLV_ADDR_YN         IS NOT NULL THEN @NEW_DLV_ADDR_YN          ELSE 'N'   END  -- ? --NEW_DLV_ADDR_YN
            ,CASE WHEN @DLV_ADDR                IS NOT NULL THEN @DLV_ADDR                 ELSE ''    END       --DLV_ADDR
            ,CASE WHEN @DLV_ADDR_DTL            IS NOT NULL THEN @DLV_ADDR_DTL             ELSE ''    END       --DLV_ADDR_DTL
            ,CASE WHEN @NEW_DLV_TEL_NO_YN       IS NOT NULL THEN @NEW_DLV_TEL_NO_YN        ELSE 'N'   END       --NEW_DLV_TEL_NO_YN
            ,CASE WHEN @DLV_CL_CODE             IS NOT NULL THEN @DLV_CL_CODE              ELSE ''    END -- ?  --DLV_CL_CODE
            ,CASE WHEN @DLV_CM_CODE             IS NOT NULL THEN @DLV_CM_CODE              ELSE ''    END -- ?  --DLV_CM_CODE
            ,CASE WHEN @TRAVEL_CODE             IS NOT NULL THEN @TRAVEL_CODE              ELSE ''    END       --TRAVEL_CODE
            ,CASE WHEN @RSV_NO                  IS NOT NULL THEN @RSV_NO                   ELSE ''    END       --RSV_NO
            ,CASE WHEN @PRE_PAY_CASH            IS NOT NULL THEN @PRE_PAY_CASH             ELSE '0'   END       --PRE_PAY_CASH
            ,CASE WHEN @PRE_PAY_CARD            IS NOT NULL THEN @PRE_PAY_CARD             ELSE '0'   END       --PRE_PAY_CARD
            ,CASE WHEN @RSV_USER_NAME           IS NOT NULL THEN @RSV_USER_NAME            ELSE ''    END       --RSV_USER_NAME
            ,CASE WHEN @RSV_USER_TEL_NO         IS NOT NULL THEN @RSV_USER_TEL_NO          ELSE ''    END       --RSV_USER_TEL_NO
            ,CASE WHEN @CST_AVL_POINT           IS NOT NULL THEN @CST_AVL_POINT            ELSE '0'   END       --CST_AVL_POINT  
            ,CASE WHEN @AFFILIATE_CODE          IS NOT NULL THEN @AFFILIATE_CODE           ELSE '0'   END       --AFFILIATE_CODE
            ,CASE WHEN @LOCAL_POINT_YN          IS NOT NULL THEN @LOCAL_POINT_YN           ELSE '0'   END       --LOCAL_POINT_YN
            ,CASE WHEN @MCP_AMT                 IS NOT NULL THEN @MCP_AMT                  ELSE '0'   END       --MCP_AMT
            ,CASE WHEN @PCD_CARD_AMT            IS NOT NULL THEN @PCD_CARD_AMT             ELSE '0'   END --    --PCD_CARD_AMT
            ,CASE WHEN @CST_ANN_POINT           IS NOT NULL THEN @CST_ANN_POINT            ELSE 0     END       --CST_ANN_POINT                                                                              
            ,CASE WHEN @CST_NEW_POINT           IS NOT NULL THEN @CST_NEW_POINT            ELSE 0     END       --CST_NEW_POINT
            ,CASE WHEN @CST_USE_POINT           IS NOT NULL THEN @CST_USE_POINT            ELSE 0     END       --CST_USE_POINT
            ,CASE WHEN @CST_DATA                IS NOT NULL THEN @CST_DATA                 ELSE ''    END       --CST_DATA
            ,CASE WHEN @DLV_BOWLIN_YN           IS NOT NULL THEN @DLV_BOWLIN_YN            ELSE 'N'   END       --DLV_BOWLIN_YN
            ,CASE WHEN @DLV_BOWLIN_EMP_NO       IS NOT NULL THEN @DLV_BOWLIN_EMP_NO        ELSE ''    END       --DLV_BOWLIN_EMP_NO
            ,CASE WHEN @DLV_BOWLIN_DT           IS NOT NULL THEN @DLV_BOWLIN_DT            ELSE ''    END       --DLV_BOWLIN_DT
            ,CASE WHEN @PPC_CARD_AMT            IS NOT NULL THEN @PPC_CARD_AMT             ELSE 0     END       --PPC_CARD_AMT
            ,CASE WHEN @EGIFT_AMT               IS NOT NULL THEN @EGIFT_AMT                ELSE 0     END       --EGIFT_AMT
            ,CASE WHEN @MOBILE_ORDER_FLAG       IS NOT NULL THEN @MOBILE_ORDER_FLAG        ELSE '00'  END       --MOBILE_ORDER_FLAG
            ,CASE WHEN @MOBILE_ORDER_NO         IS NOT NULL THEN @MOBILE_ORDER_NO          ELSE ''    END       --MOBILE_ORDER_NO
            ,CASE WHEN @RFND_TYPE_CODE          IS NOT NULL THEN @RFND_TYPE_CODE           ELSE ''    END       --RFND_TYPE_CODE
            ,CASE WHEN @TAX_RFND_AMT            IS NOT NULL THEN @TAX_RFND_AMT             ELSE 0     END       --TAX_RFND_AMT
            ,CASE WHEN @TAX_RFND_FEE            IS NOT NULL THEN @TAX_RFND_FEE             ELSE 0     END       --TAX_RFND_FEE
            ,CASE WHEN @DC_TAX_AMT              IS NOT NULL THEN @DC_TAX_AMT               ELSE 0     END       --DC_TAX_AMT
            ,CASE WHEN @DUTCH_ORDER_NO          IS NOT NULL THEN @DUTCH_ORDER_NO           ELSE ''    END       --DUTCH_ORDER_NO
            ,CASE WHEN @O2O_AMT                 IS NOT NULL THEN @O2O_AMT                  ELSE 0     END       --O2O_AMT
            ,CASE WHEN @DLV_SAFE_TEL_NO         IS NOT NULL THEN @DLV_SAFE_TEL_NO          ELSE '0'   END       --DLV_SAFE_TEL_NO
            ,CASE WHEN @DLV_EXPIRE_TIME         IS NOT NULL THEN @DLV_EXPIRE_TIME          ELSE '0'   END       --DLV_EXPIRE_TIME
            ,CASE WHEN @EXTERN_GUEST_FLAG       IS NOT NULL THEN @EXTERN_GUEST_FLAG        ELSE '0'   END       --EXTERN_GUEST_FLAG
            ,CASE WHEN @DCC_EX_RATE_CODE        IS NOT NULL THEN @DCC_EX_RATE_CODE         ELSE '0'   END       --DCC_EX_RATE_CODE
            ,CASE WHEN @DCC_RCV_TEXT            IS NOT NULL THEN @DCC_RCV_TEXT             ELSE '0'   END       --DCC_RCV_TEXT
            ,CASE WHEN @SP_PAY_AMT              IS NOT NULL THEN @SP_PAY_AMT               ELSE 0     END       --SP_PAY_AMT
            ,CASE WHEN @HOOK_ORDER_CHANNEL_TYPE IS NOT NULL THEN @HOOK_ORDER_CHANNEL_TYPE  ELSE ''    END       --HOOK_ORDER_CHANNEL_TYPE
            ,CASE WHEN @DEPOSIT_AMT             IS NOT NULL THEN @DEPOSIT_AMT              ELSE 0     END       --DEPOSIT_AMT
)
                -- not found or undefined
    --      ,CASE WHEN @CST_CARD_NO             IS NOT NULL THEN @CST_CARD_NO              ELSE ''    END
    --      ,CASE WHEN @DC_YAP_AMT              IS NOT NULL THEN @DC_YAP_AMT               ELSE 0     END
    --      ,CASE WHEN @MOBILE_REG_NO           IS NOT NULL THEN @MOBILE_REG_NO            ELSE ''    END
    --      ,CASE WHEN @LG_SEND_FLAG            IS NOT NULL THEN @LG_SEND_FLAG             ELSE '0'   END
    --      ,CASE WHEN @LG_SEND_DT              IS NOT NULL THEN @LG_SEND_DT               ELSE ''    END
    --      ,CASE WHEN @MOBILE_PACK_AMT         IS NOT NULL THEN @MOBILE_PACK_AMT          ELSE 0     END
    --      ,CASE WHEN @ORDER_END_DT            IS NOT NULL THEN @ORDER_END_DT             ELSE ''    END
    --      ,CASE WHEN @DELIVER_NO              IS NOT NULL THEN @DELIVER_NO               ELSE ''    END      
    --      ,CASE WHEN @RESERVED_NO             IS NOT NULL THEN @RESERVED_NO              ELSE ''    END
    --      ,CASE WHEN @ORG_ORDER_NO            IS NOT NULL THEN @ORG_ORDER_NO             ELSE ''    END      --
    --      ,CASE WHEN @KITCHEN_MEMO            IS NOT NULL THEN @KITCHEN_MEMO             ELSE ''    END      -- MST_KTCN_MEMO

<InsertProdOrderHdr/> 

<InsertProdOrderDetail>
    
                            -- table ORD_PRDT
SELECT 
    @ORDER_NO = COALESCE(MAX(CAST(ORDER_NO AS INT)) + 1, '1'),
    @SEQ_NO1 =  COALESCE(MAX(CAST(SEQ_NO AS INT)) + 1, '1')
FROM ORD_PRDT
WHERE CONVERT(DATE, SALE_DATE) = CONVERT(DATE, GETDATE())
    AND ORDER_NO = '1'
    AND SEQ_NO = '1';

INSERT  INTO  ORD_PRDT (
             SHOP_CODE
            ,SALE_DATE
            ,POS_NO
            ,ORDER_NO
            ,SEQ_NO
            ,ORDER_SEQ_NO
            ,PRD_FLAG
            ,PRD_CODE
            ,PRD_TYPE_FLAG
            ,CORNER_CODE
            ,SALE_QTY
            ,SALE_UPRC
            ,SALE_AMT
            ,DC_AMT
            ,ETC_AMT
            ,SVC_TIP_AMT
            ,VAT_AMT
            ,DCM_SALE_AMT
            ,CHG_BILL_NO
            ,TAX_YN
            ,DLV_PACK_FLAG
            ,SDS_PARENT_CODE
            ,DC_AMT_GEN
            ,DC_AMT_SVC
            ,DC_AMT_JCD
            ,DC_AMT_CPN
            ,DC_AMT_CST
            ,DC_AMT_FOD
            ,DC_AMT_PRM
            ,DC_AMT_CRD
            ,DC_AMT_PACK
            ,ORDER_SCN_FLAG
            ,ORDER_SCN_DT
            ,ORDER_SCN_POS_NO
            ,ORDER_CANCEL_FLAG
            ,INSERT_DT
            ,SALE_YN
            )
        VALUES(
             @SHOP_CODE                                                                                  --SHOP_CODE
            ,@SALE_DATE                                                                                  --SALE_DATE
            ,@POS_NO                                                                                     --POS_NO
            ,@ORDER_NO                                                                                   --ORDER_NO
            ,@SEQ_NO1                                                                                    --SEQ_NO
            ,CASE WHEN @ORDER_SEQ_NO        IS NOT NULL THEN @ORDER_SEQ_NO      ELSE    '01'       END        --ORDER_SEQ_NO    
            ,'0'                                                                                         --PRD_FLAG
            ,@PRD_CODE                                                                                   --PRD_CODE
            ,CASE WHEN @PRD_TYPE_FLAG       IS NOT NULL THEN @PRD_TYPE_FLAG     ELSE    '0'        END      --PRD_TYPE_FLAG
            ,CASE WHEN @CORNER_CODE         IS NOT NULL THEN @CORNER_CODE       ELSE    '00'       END      --CORNER_CODE
            ,CASE WHEN @SALE_QTY            IS NOT NULL THEN @SALE_QTY          ELSE    SALE_QTY   END      --SALE_QTY
            ,CASE WHEN @SALE_UPRC           IS NOT NULL THEN @SALE_UPRC         ELSE    'Y'        END      --SALE_UPRC
            ,CASE WHEN @SALE_AMT            IS NOT NULL THEN @SALE_AMT          ELSE    '0'        END      --SALE_AMT
            ,CASE WHEN @DC_AMT              IS NOT NULL THEN @DC_AMT            ELSE    DC_AMT     END      --DC_AMT
            ,CASE WHEN @ETC_AMT             IS NOT NULL THEN @ETC_AMT           ELSE    0          END      --ETC_AMT
            ,CASE WHEN @SVC_TIP_AMT         IS NOT NULL THEN @SVC_TIP_AMT       ELSE    0          END      --SVC_TIP_AMT
            ,CASE WHEN @VAT_AMT             IS NOT NULL THEN @VAT_AMT           ELSE    VAT_ATM    END      --VAT_AMT
            ,CASE WHEN @DCM_SALE_AMT        IS NOT NULL THEN @DCM_SALE_AMT      ELSE    0          END      --DCM_SALE_AMT
            ,CASE WHEN @CHG_BILL_NO         IS NOT NULL THEN @CHG_BILL_NO       ELSE    0          END      --CHG_BILL_NO
            ,CASE WHEN @TAX_YN              IS NOT NULL THEN @TAX_YN            ELSE    0          END      --TAX_YN
            ,CASE WHEN @DLV_PACK_FLAG       IS NOT NULL THEN @DLV_PACK_FLAG     ELSE    0          END      --DLV_PACK_FLAG
            ,CASE WHEN @SDS_PARENT_CODE     IS NOT NULL THEN @SDS_PARENT_CODE   ELSE    0          END      --SDS_PARENT_CODE
            ,CASE WHEN @DC_AMT_GEN          IS NOT NULL THEN @DC_AMT_GEN        ELSE    0          END      --DC_AMT_GEN
            ,CASE WHEN @DC_AMT_SVC          IS NOT NULL THEN @DC_AMT_SVC        ELSE    0          END      --DC_AMT_SVC
            ,CASE WHEN @DC_AMT_JCD          IS NOT NULL THEN @DC_AMT_JCD        ELSE    0          END      --DC_AMT_JCD
            ,CASE WHEN @DC_AMT_CPN          IS NOT NULL THEN @DC_AMT_CPN        ELSE    DC_AMT_CPN END      --DC_AMT_CPN
            ,CASE WHEN @DC_AMT_CST          IS NOT NULL THEN @DC_AMT_CST        ELSE    DC_AMT_CST END      --DC_AMT_CST
            ,CASE WHEN @DC_AMT_FOD          IS NOT NULL THEN @DC_AMT_FOD        ELSE    '0'        END      --DC_AMT_FOD
            ,CASE WHEN @DC_AMT_PRM          IS NOT NULL THEN @DC_AMT_PRM        ELSE    0          END      --DC_AMT_PRM
            ,CASE WHEN @DC_AMT_CRD          IS NOT NULL THEN @DC_AMT_CRD        ELSE    0          END      --DC_AMT_CRD
            ,CASE WHEN @DC_AMT_PACK         IS NOT NULL THEN @DC_AMT_PACK       ELSE    0          END      --DC_AMT_PACK
            ,CASE WHEN @ORDER_SCN_FLAG      IS NOT NULL THEN @ORDER_SCN_FLAG    ELSE    0          END      --ORDER_SCN_FLAG
            ,CASE WHEN @ORDER_SCN_DT        IS NOT NULL THEN @ORDER_SCN_DT      ELSE    0          END      --ORDER_SCN_DT
            ,CASE WHEN @ORDER_SCN_POS_NO    IS NOT NULL THEN @ORDER_SCN_POS_NO  ELSE    0          END      --ORDER_SCN_POS_NO
            ,CASE WHEN @ORDER_CANCEL_FLAG   IS NOT NULL THEN @ORDER_CANCEL_FLAG ELSE    0          END      --ORDER_CANCEL_FLAG
            ,@INSERT_DT                                                                                  --INSERT_DT
            ,'Y'
)

SELECT 
    @BILL_NO = COALESCE(MAX(CAST(BILL_NO AS INT)) + 1, '1'),
    @SEQ_NO2 = COALESCE(MAX(CAST(SEQ_NO AS INT)) + 1, '1')
FROM YourTable
WHERE CONVERT(DATE, SALE_DATE) = CONVERT(DATE, GETDATE())
    AND BILL_NO = '1'
    AND SEQ_NO = '1';

            -- TABLE TRN_PRDT
INSERT INTO TRN_PRDT (
            SHOP_CODE
           ,SALE_DATE
           ,POS_NO
           ,BILL_NO
           ,SEQ_NO
           ,REGI_SEQ
           ,SALE_YN
           ,PRD_CODE
           ,PRD_TYPE_FLAG
           ,CORNER_CODE
           ,SALE_QTY
           ,SALE_UPRC
           ,SALE_AMT
           ,DC_AMT
           ,ETC_AMT
           ,SVC_TIP_AMT
           ,VAT_AMT
           ,DCM_SALE_AMT
           ,CHG_BILL_NO
           ,TAX_YN
           ,DLV_PACK_FLAG
           ,SDS_CLASS_CODE
           ,DC_AMT_GEN
           ,DC_AMT_SVC
           ,DC_AMT_JCD
           ,DC_AMT_CPN
           ,DC_AMT_CST
           ,DC_AMT_FOD
           ,DC_AMT_PRM
           ,DC_AMT_CRD
           ,DC_AMT_PACK
           ,INSERT_DT
           ,ORG_SALE_MG_CODE
           ,ORG_SALE_UPRC
           ,NORMAL_UPRC
           ,SALE_MG_CODE
           ,SVC_CODE
           ,TK_CPN_CODE
           ,DC_AMT_LYT
           ,CST_SALE_POINT
           ,CST_USE_POINT
           ,PRM_PROC_YN
           ,PRM_CODE
           ,PRM_SEQ
           ,SDA_CODE
           ,SDS_ORG_DTL_NO
           ,EMP_NO
           ,AFFILIATE_UPRC
           ,MCP_BAR_CODE
           ,DOUBLE_CODE
           ,DOUBLE_AMT
           ,PRD_WEIGHT
           ,IF_CPN_CODE
           ,TAX_RFND_AMT
           ,TAX_RFND_FEE
           ,DC_AMT_TAX
           ,DC_REASON_CODE
           ,DC_REASON_NAME
           ,SDS_PARENT_CODE
           ,DC_SEQ_TYPE
           ,DC_SEQ_AMT
           ,DC_SEQ_FLAG
           ,DC_SEQ_RATE
  
        )
VALUES     (
            @SHOP_CODE
           ,@SALE_DATE
           ,@POS_NO
           ,@BILL_NO     
           ,@SEQ_NO2       
           ,CASE WHEN @REGI_SEQ             IS NOT NULL THEN @REGI_SEQ          ELSE    '01'            END
           ,CASE WHEN @SALE_YN              IS NOT NULL THEN @SALE_YN           ELSE    'Y'             END
           ,@PRD_CODE
           ,CASE WHEN @PRD_TYPE_FLAG        IS NOT NULL THEN @PRD_TYPE_FLAG     ELSE    '0'             END
           ,CASE WHEN @CORNER_CODE          IS NOT NULL THEN @CORNER_CODE       ELSE    '0'             END
           ,CASE WHEN @SALE_QTY             IS NOT NULL THEN @SALE_QTY          ELSE    0               END
           ,CASE WHEN @SALE_UPRC            IS NOT NULL THEN @SALE_UPRC         ELSE    0               END
           ,CASE WHEN @SALE_AMT             IS NOT NULL THEN @SALE_AMT          ELSE    0               END
           ,CASE WHEN @DC_AMT               IS NOT NULL THEN @DC_AMT            ELSE    0               END
           ,CASE WHEN @ETC_AMT              IS NOT NULL THEN @ETC_AMT           ELSE    0               END
           ,CASE WHEN @SVC_TIP_AMT          IS NOT NULL THEN @SVC_TIP_AMT       ELSE    0               END
           ,CASE WHEN @VAT_AMT              IS NOT NULL THEN @VAT_AMT           ELSE    0               END
           ,CASE WHEN @DCM_SALE_AMT         IS NOT NULL THEN @DCM_SALE_AMT      ELSE    0               END
           ,CASE WHEN @CHG_BILL_NO          IS NOT NULL THEN @CHG_BILL_NO       ELSE    CHG_BILL_NO     END
           ,CASE WHEN @TAX_YN               IS NOT NULL THEN @TAX_YN            ELSE    'Y'             END
           ,CASE WHEN @DLV_PACK_FLAG        IS NOT NULL THEN @DLV_PACK_FLAG     ELSE    'N'             END
           ,CASE WHEN @SDS_CLASS_CODE       IS NOT NULL THEN @SDS_CLASS_CODE    ELSE    SDS_CLASS_CODE  END
           ,CASE WHEN @DC_AMT_GEN           IS NOT NULL THEN @DC_AMT_GEN        ELSE    0               END
           ,CASE WHEN @DC_AMT_SVC           IS NOT NULL THEN @DC_AMT_SVC        ELSE    0               END
           ,CASE WHEN @DC_AMT_JCD           IS NOT NULL THEN @DC_AMT_JCD        ELSE    0               END
           ,CASE WHEN @DC_AMT_CPN           IS NOT NULL THEN @DC_AMT_CPN        ELSE    0               END
           ,CASE WHEN @DC_AMT_CST           IS NOT NULL THEN @DC_AMT_CST        ELSE    0               END
           ,CASE WHEN @DC_AMT_FOD           IS NOT NULL THEN @DC_AMT_FOD        ELSE    0               END
           ,CASE WHEN @DC_AMT_PRM           IS NOT NULL THEN @DC_AMT_PRM        ELSE    0               END
           ,CASE WHEN @DC_AMT_CRD           IS NOT NULL THEN @DC_AMT_CRD        ELSE    0               END
           ,CASE WHEN @DC_AMT_PACK          IS NOT NULL THEN @DC_AMT_PACK       ELSE    0               END
           ,@INSERT_DT
           ,CASE WHEN @ORG_SALE_MG_CODE     IS NOT NULL THEN @ORG_SALE_MG_CODE  ELSE   ORG_SALE_MG_CODE	END
           ,CASE WHEN @ORG_SALE_UPRC        IS NOT NULL THEN @ORG_SALE_UPRC     ELSE    0               END
           ,CASE WHEN @NORMAL_UPRC          IS NOT NULL THEN @NORMAL_UPRC       ELSE    0               END
           ,CASE WHEN @SALE_MG_CODE         IS NOT NULL THEN @SALE_MG_CODE      ELSE    SALE_MG_CODE	END
           ,CASE WHEN @SVC_CODE             IS NOT NULL THEN @SVC_CODE          ELSE    SVC_CODE   	    END
           ,CASE WHEN @TK_CPN_CODE          IS NOT NULL THEN @TK_CPN_CODE       ELSE    TK_CPN_CODE	    END
           ,CASE WHEN @DC_AMT_LYT           IS NOT NULL THEN @DC_AMT_LYT        ELSE    0               END
           ,CASE WHEN @CST_SALE_POINT       IS NOT NULL THEN @CST_SALE_POINT    ELSE    0               END
           ,CASE WHEN @CST_USE_POINT        IS NOT NULL THEN @CST_USE_POINT     ELSE    0               END
           ,CASE WHEN @PRM_PROC_YN          IS NOT NULL THEN @PRM_PROC_YN       ELSE    'N'             END
           ,CASE WHEN @PRM_CODE             IS NOT NULL THEN @PRM_CODE          ELSE    PRM_CODE      	END
           ,CASE WHEN @PRM_SEQ              IS NOT NULL THEN @PRM_SEQ           ELSE    PRM_SEQ       	END
           ,CASE WHEN @SDA_CODE             IS NOT NULL THEN @SDA_CODE          ELSE    SDA_CODE      	END
           ,CASE WHEN @SDS_ORG_DTL_NO       IS NOT NULL THEN @SDS_ORG_DTL_NO    ELSE    SDS_ORG_DTL_NO	END
           ,@EMP_NO
           ,CASE WHEN @AFFILIATE_UPRC       IS NOT NULL THEN @AFFILIATE_UPRC    ELSE    AFFILIATE_UPRC	END
           ,CASE WHEN @MCP_BAR_CODE         IS NOT NULL THEN @MCP_BAR_CODE      ELSE    MCP_BAR_CODE  	END
           ,CASE WHEN @DOUBLE_CODE          IS NOT NULL THEN @DOUBLE_CODE       ELSE    DOUBLE_CODE   	END
           ,CASE WHEN @DOUBLE_AMT           IS NOT NULL THEN @DOUBLE_AMT        ELSE    0               END
           ,CASE WHEN @PRD_WEIGHT           IS NOT NULL THEN @PRD_WEIGHT        ELSE    0               END
           ,CASE WHEN @IF_CPN_CODE          IS NOT NULL THEN @IF_CPN_CODE       ELSE    IF_CPN_CODE	    END
           ,CASE WHEN @TAX_RFND_AMT         IS NOT NULL THEN @TAX_RFND_AMT      ELSE    0               END
           ,CASE WHEN @TAX_RFND_FEE         IS NOT NULL THEN @TAX_RFND_FEE      ELSE    0               END
           ,CASE WHEN @DC_AMT_TAX           IS NOT NULL THEN @DC_AMT_TAX        ELSE    0               END
           ,CASE WHEN @DC_REASON_CODE       IS NOT NULL THEN @DC_REASON_CODE    ELSE    DC_REASON_CODE 	END
           ,CASE WHEN @DC_REASON_NAME       IS NOT NULL THEN @DC_REASON_NAME    ELSE    DC_REASON_NAME 	END
           ,CASE WHEN @SDS_PARENT_CODE      IS NOT NULL THEN @SDS_PARENT_CODE   ELSE    SDS_PARENT_CODE	END
           ,CASE WHEN @DC_SEQ_TYPE          IS NOT NULL THEN @DC_SEQ_TYPE       ELSE    DC_SEQ_TYPE    	END
           ,CASE WHEN @DC_SEQ_AMT           IS NOT NULL THEN @DC_SEQ_AMT        ELSE    DC_SEQ_AMT     	END
           ,CASE WHEN @DC_SEQ_FLAG          IS NOT NULL THEN @DC_SEQ_FLAG       ELSE    DC_SEQ_FLAG    	END
           ,CASE WHEN @DC_SEQ_RATE          IS NOT NULL THEN @DC_SEQ_RATE       ELSE    DC_SEQ_RATE    	END
  )          
            
      
       
<InsertProdOrderDetail/>      