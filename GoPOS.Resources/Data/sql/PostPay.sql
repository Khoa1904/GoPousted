<GetTables>
SELECT *
FROM	MST_TABLE_INFO                          
WHERE	USE_YN    = 'Y'
	and	SHOP_CODE = @SHOP_CODE
ORDER BY TABLE_CODE 
</GetTables>


