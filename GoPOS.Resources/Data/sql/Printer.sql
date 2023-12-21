<GetPrintForms>

SELECT	SHOP_CODE
		,PRT_CLASS_CODE
		,PRT_FORM
		,INSERT_DT
		,UPDATE_DT

FROM MST_FORM_PRINTER
WHERE	SHOP_CODE = @SHOP_CODE
AND		PRT_CLASS_CODE = @PRT_CLASS_CODE

</GetPrintForms>

<GetPrintDatas>
SELECT 
		 O.SHOP_CODE                         
		,O.SALE_DATE                         
		,PRINT_SEQ                         
		,O.ORDER_NO                          
		,K.PRT_NO                            
		,'TEST DATA' AS CONTENTS                         -- test phase 
		,PRINT_YN                          
		,O.INSERT_DT                         
		,getdate() as PRT_DT                            
		,ORDER_PRT_SEQ                     
		,O.ORDER_SEQ_NO                      
FROM	ORD_HEADER O INNER JOIN MST_KICN_PRINTER K ON O.SHOP_CODE = K.SHOP_CODE AND O.POS_NO = K.POS_NO

</GetPrintDatas>

<GetPrintM>
SELECT 
			 SHOP_CODE                     
			,PRT_NO                        
			,PRT_NAME                      
			,POS_NO                        
			,PRT_TYPE_FLAG                 
			,PRT_PORT                      
			,PRT_SPEED                     
			,USE_YN                        
			,INSERT_DT                     
			,UPDATE_DT                     
			,PRT_PAPER_QTY                 
			,FLOOR_NO                      
			,FLOOR_FLAG                    
			,PRT_TCP_IP                    
			,PRT_TCP_PORT                  
			,PRT_BELL_YN    				
FROM		MST_KICN_PRINTER 
WHERE		SHOP_CODE = @SHOP_CODE AND PRT_NO = @PRT_NO
</GetPrintM>

<GetProductPrinterMap>
SELECT 
	    SHOP_CODE       
	   ,PRD_CODE    
	   ,PRT_NO      
	   ,USE_YN      
	   ,INSERT_DT   
	   ,UPDATE_DT       
FROM	MST_PRD_PRNT_MAP
WHERE	SHOP_CODE = @SHOP_CODE AND PRT_NO = @PRT_NO
</GetProductPrinterMap>

<GetProducts>

SELECT  SHOP_CODE,PRD_CODE,PRD_NAME FROM MST_INFO_PRODUCT m
UNION
SELECT 
         msdc.SHOP_CODE, MSDC3.SDA_CLASS_CODE || msdc.SDA_CODE AS PRD_CODE,MSDC.SDA_NAME AS PRD_NAME
         FROM MST_SIDE_DETP_CODE msdc 
            LEFT JOIN MST_SIDE_DEPT_CLASS msdc2 
               ON MSDC.SHOP_CODE = MSDC2.SHOP_CODE
               AND MSDC.SDA_CLASS_CODE = MSDC2.SDA_CLASS_CODE
            LEFT JOIN MST_SIDE_DEPT_CLASS msdc3
               ON MSDC.SHOP_CODE = MSDC3.SHOP_CODE
               AND msdc2.SDA_CLASS_CODE = MSDC3.SDA_CLASS_CODE  
         WHERE MSDC.USE_YN = 'Y'

</GetProducts>
<GetShopConfigValue>
SELECT mcd.ENV_SET_CODE, mcd.ENV_VALUE_CODE, mcd.ENV_VALUE_NAME
FROM MST_CNFG_SHOP mcs 
	LEFT JOIN MST_CNFG_DETAIL mcd ON mcs.ENV_SET_CODE = mcd.ENV_SET_CODE 
							AND mcs.ENV_SET_VALUE = mcd.ENV_VALUE_CODE 
	LEFT JOIN MST_CNFG_CODE mcc ON mcd.ENV_SET_CODE = mcc.ENV_SET_CODE
WHERE mcs.ENV_SET_CODE = @SET_CODE
</GetShopConfigValue>
<GetPosConfigValue>
SELECT mcd.ENV_SET_CODE, mcd.ENV_VALUE_CODE, mcd.ENV_VALUE_NAME
FROM MST_CNFG_POS mcs 
	LEFT JOIN MST_CNFG_DETAIL mcd ON mcs.ENV_SET_CODE = mcd.ENV_SET_CODE 
							AND mcs.ENV_SET_VALUE = mcd.ENV_VALUE_CODE 
	LEFT JOIN MST_CNFG_CODE mcc ON mcd.ENV_SET_CODE = mcc.ENV_SET_CODE
WHERE mcc.ENV_SET_CODE = @SET_CODE
</GetPosConfigValue>