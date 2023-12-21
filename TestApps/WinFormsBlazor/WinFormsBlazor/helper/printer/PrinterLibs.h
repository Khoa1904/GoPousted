#ifndef _PRINTERLIBS_H_
#define _PRINTERLIBS_H_

#define CSNPRINTERLLIBS_EXPORTS
#ifdef CSNPRINTERLLIBS_EXPORTS
#define CSNPRINTERLLIBS __declspec(dllexport)
#else
#define CSNPRINTERLLIBS __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

	/****************************************Port operation function*****************************************/
	// Enumerate serial port
	// buffer
	//     The buffer to save enumerated port list.
	// length
	//     The buffer size
	// return
	//     Enumerated port count
	CSNPRINTERLLIBS size_t Port_EnumCOM(char *buffer, size_t length);
	// Enumerate usb port
	//  buffer
	//      The buffer to save enumerated port list
	//  length
	//      The buffer size
	//  return
	//      Enumerated port count
	CSNPRINTERLLIBS size_t Port_EnumUSB(char *buffer, size_t length);
	// Enumerate parallel port
	//  buffer
	//      The buffer to save enumerated port list
	//  length
	//      The buffer size
	//  return
	//      Enumerated port count
	CSNPRINTERLLIBS size_t Port_EnumLPT(char *buffer, size_t length);
	// Enumerate printer
	//  buffer
	//      The buffer to save enumerated port list
	//  length
	//      The buffer size
	//  return
	//      Enumerated port count
	CSNPRINTERLLIBS size_t Port_EnumPRN(char *buffer, size_t length);

	// Open com port
	// name 
	//      Open com name,Can be obtained by EnumCOM
	//      for example: COM1£¬COM2£¬COM3£¬...COM11...
	// baudrate 
	//      baudrate
	//      Normally choose: 9600,19200,38400,57600,115200.
	//		default 9600
	//      Need to keep the same as printer baudrate, suggest using high baudrate to get better printing speed.
	// flowcontrol
	//      flowcontrol, The values are defined as follows:
	//      value    define
	//      0     	 NO
	//      1     	 DsrDtr
	//      2     	 CtsRts
	//		3	  	 Xon/Xoff
	// parity 
	//      Parity bit, The values are defined as follows:
	//      value    define
	//      0     no parity
	//      1     odd parity
	//      2     even parity
	//      3     mark parity
	//      4     space parity
	// databits
	//      databits, value range is [4,8]
	// stopbits 
	//      Parity bit, The values are defined as follows:
	//      value    define
	//      0     1bit stopbits
	//      1     1.5bit stopbits
	//      2     2bit stopbits
	//
	// return 
	//      Return handle, If open success, return non-zero value, else return zero.
	//
	// remarks
	//      If serial port was occupied, open serial will fail
	//      If baud rate don't match with printer baud rate, it won't print.
	CSNPRINTERLLIBS void * Port_OpenCOMIO(const char *name, unsigned int baudrate = 9600, const int flowcontrol = 0, const int parity = 0, const int databis = 8, const int stopbits = 0);
	// Open usb port
	// name 
	//      Open usb name,Can be obtained by EnumUSB
	// return 
	//      Return handle, If open success, return non-zero value, else return zero.
	// remarks
	//      USB printer connect to computer, if device manager appear "USB Printing Support", then can open USE this function.
	CSNPRINTERLLIBS void * Port_OpenUSBIO(const char *name);
	// Open parallel port
	// name 
	//      Open parallel name,Can be obtained by EnumUSB
	//		for example£ºLPT1,LPT2,LPT3...
	// return 
	//      Return handle, If open success, return non-zero value, else return zero.
	// remarks
	//      Parallel port just support one way communication, just can write, but can't read
	//      All the query status function, are invalid for parallel port.
	CSNPRINTERLLIBS void * Port_OpenLPTIO(const char *name);
	// Open printer port
	// name 
	//      Open parallel name,Can be obtained by EnumUSB
	// return 
	//      Return handle, If open success, return non-zero value, else return zero.
	// remarks
	//      Most drivers only support one-way communication, so query status function are invalid
	//      Need call Close to realy send data to printer.
	CSNPRINTERLLIBS void * Port_OpenPRNIO(const char *name);
	//      Open Tcp
	// ip 
	//      IP Addres or printer name
	//      For example: 192.168.1.87
	// port 
	//      Port Number
	//      Fixed value: 9100
	// return 
	//      Return handle, If open success, return non-zero value, else return zero.
	// remarks
	//      PC and printer need in the same network segment, so they can connect
	CSNPRINTERLLIBS void * Port_OpenTCPIO(const char *ip, const unsigned short port);
	// Set the printer communication port
	// handle 
	//      Port handle obtained through OpenXXX()
	// return 
	//      Returns true on success, false on failure.
	CSNPRINTERLLIBS bool Port_SetPort(void *handle);
	// Close the printer communication port
	// handle 
	//      Port handle obtained through OpenXXX()
	CSNPRINTERLLIBS void Port_ClosePort(void *handle);
	/****************************************Print operation function*****************************************/
	// Reset printer
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_Reset();
	// Print test page
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_SelfTest();
	// printer feed one Lines
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_FeedLine();
	// printer feed n dot(0.125mm)
	// n
	//		n dot
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_FeedHot(int n);
	// printer feed n line
	// n
	//		n line.
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_Feed_N_Line(int n);
	// Feed the paper to the next label
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      The API is only for printers with non-standard label instructions, that is, the label machines that execute ESC/POS instructions.Model: lpm-261
	CSNPRINTERLLIBS bool Pos_FeedNextLable();
	// Set alignment
	// value
	//      print alignment, value are defined as follow:
	//      value define
	//      0     align left
	//      1     align center
	//      2     align right
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      This API is only used to align the instruction operation, and the API that needs to be aligned is used to supplement.
	CSNPRINTERLLIBS bool Pos_Align(int value);
	// Set the line height
	// value
	//      Set the line height(hot size£º0.125mm)£¬value range is[0,255]
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_SetLineHeight(int value);
	// Print text
	// prnText
	//		Text that needs to be printed
	// nLan
	//		Type of text encoding for printing, each value is defined as follows:
	//      value    define
	//      0     	 GBK
	//      1     	 UTF-8
	//      3     	 BIG-5
	//		4	  	 SHIFT-JIS
	//		5	  	 EUC-KR
	// nOrgx
	//		The value of the printed text position is defined as follows:
	//      value    define
	//      -1    	 align left
	//      -2    	 align center
	//      -3    	 align right
	//		>=0	  	 Starting from the n hot
	// nWidthTimes
	//		Multiple of width magnification£¬value range is[0,7]
	// nHeightTimes
	//		Higher magnification£¬value range is[0,7]
	// FontType
	//		Type of font to be printed, each value is defined as follows:
	//      value    define
	//      0	  	 12*24
	//		1	  	 9*17
	// nFontStyle
	//		Type of font to be printed, each value is defined as follows:
	//      value		define
	//      0x00		normal
	//		0x08		bold
	//		0x80    	1 hot underline
	//		0x100   	2 hot underline
	//		0x200		convert, only efficient in line start
	//		0x400		inverse, white in black
	//		0x1000		every character rotates clockwise 90¡ã
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      NFontStyle values can be used with & to allow multiple styles to occur simultaneously.
	CSNPRINTERLLIBS bool Pos_Text(const wchar_t *prnText, int nLan, int nOrgx, int nWidthTimes, int nHeightTimes, int FontType, int nFontStyle);

	// buzzer beeps
	// nBeepCount
	//      Beep times
	// nMillis
	//      Beep time each time = 100 * nBeemMillis ms
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Please confirm whether the printer has a buzzer function.
	CSNPRINTERLLIBS bool Pos_Beep(unsigned char nCount, unsigned char nMillis);
	// Open cashbox 
	// nId
	//      0 means: pulse was sent to cashbox to output pin 2 
	//		1 means: pulse was sent to cashbox to output pin 5 	
	// nHightTime
	//      High electric level
	// nLowTime 
	//      Low electric level
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Please confirm whether the printer has the function of opening the money box.
	CSNPRINTERLLIBS bool Pos_KiskOutDrawer(int nId, int nHightTime = 20, int nLowTime = 60);
	// Perform a full cut
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Please confirm whether the printer has full cut function.
	CSNPRINTERLLIBS bool Pos_FullCutPaper();
	// Perform a half cut
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Please confirm whether the printer has half cut function.
	CSNPRINTERLLIBS bool POS_HalfCutPaper();
	// Print the barcode
	// BarcodeData
	//		Printed bar code content
	// nBarcodeType
	//		Type of barcode printed, each value is defined as follows:
	//      value		define
	//      0x41     	UPC-A
	//      0x42     	UPC-E
	//      0x43     	EAN13
	//      0x44     	EAN8
	//      0x45     	CODE39
	//      0x46     	ITF
	//      0x47     	CODABAR
	//      0x48     	CODE93
	// nOrgx
	//		The value of the printed bar code position is defined as follows:
	//      value    define
	//      -1    	 align left
	//      -2    	 align center
	//      -3    	 align right
	//		>=0	  	 Starting from the n hot
	// nUnitWidth
	//		Bar code width£¬value range is[1,6]
	// nUnitHeight
	//		Bar code height£¬value range is[1,255]
	// nFontStyle
	//		The font type of readable character (HRI) is defined as follows:
	//      value    define
	//      0     	 12*24
	//      1     	 9*17
	// FontPosition
	//		The printing positions of readable characters (HRI) are defined as follows:
	//      value    define
	//      0     	 Doesn¡¯t print
	//      1     	 Only print in barcode upward
	//      2     	 Only print in barcode downward
	//		3	  	 Print both barcode upward and downward
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      nUnitWidth If the maximum width of the printer is exceeded, it is not printed.
	CSNPRINTERLLIBS bool Pos_Barcode(const char * BarcodeData, int nBarcodeType, int nOrgx, int nUnitWidth, int nUnitHeight, int nFontStyle, int FontPosition);
	// Print QR code 
	// QrcodeData
	//		QR code character string
	// nWidth
	// 		Unit width of each module for QR code,value range is[1,6]
	// 		Set module width properly can make QR code nicer 
	// nVersion
	// 		QR code version size, this value is about QR code size.  value range is[0,16]
	// 		Set as 0 to calculate QR code version size automatically 
	//		Pls set proper value for this value if you want the QR code size to be fixed.
	// nErrlevenl
	//		Error correction level£¬value range is[1,4]
	// return 
	//		Returns true on successful write, false on failed write.
	// remarks
	//		If the printed qr code exceeds the print boundary, it is not printed.
	CSNPRINTERLLIBS bool Pos_Qrcode(const wchar_t *QrcodeData, int nWidth = 2, int nVersion = 0, int nErrlevenl = 4);
	// Print QR code £¨ESC/POS£©
	// QrcodeData
	//		QR code character string.
	// nWidth
	// 		Unit width of each module for QR code,value range is[1,16]
	// 		Set module width properly can make QR code nicer 
	// nErrlevenl
	//		Error correction level£¬value range is[1,4]
	// return 
	//		Returns true on successful write, false on failed write.
	// remarks
	//		If the printed qr code exceeds the print boundary, it is not printed.
	CSNPRINTERLLIBS bool Pos_EscQrcode(const wchar_t *QrcodeData, int nWidth = 4, int nErrlevenl = 4);
	// Print Double QR code
	// QrcodeData1
	//      QR code 1 character string.
	// QR1Position
	//      QR code 1 start print Position
	// QR1Version
	//      QR code 1 version size, this value is about QR code size,·¶Î§[0£¬16]
	// QR1Ecc
	//      QR code 1 Error correction level£¬value range is[0,3]
	// QrcodeData2
	//      QR code 2 character string.
	// QR2Position
	//      QR code 2 start print Position
	// QR2Version
	//      QR code 2 version size, this value is about QR code size,·¶Î§[0£¬16]
	// QR2Ecc
	//      QR code 2 Error correction level£¬value range is[0,3]
	// ModuleSize
	//      QrCode size.[1£¬8]
	// remarks
	//      If the printing fails
	//      please check whether the location of the second qr code printing overlaps with the first one or exceeds the printing boundary after printing.
	CSNPRINTERLLIBS bool Pos_DoubleQrcode(const wchar_t *QrcodeData1,int QR1Position,int QR1Version, int QR1Ecc, const wchar_t *QrcodeData2, int QR2Position, int QR2Version, int QR2Ecc, int ModuleSize);
	// Print picture 
	// FileName
	//		Image path.
	// nWidth
	//		Specifies the width (pixels) for the printer to print the image
	//		The max. width doesn¡¯t exceed 384 dots of 2 inches printer(58mm printer)
	//		The max. width doesn¡¯t exceed 576 dots of 3 inches printer(80mm printer)
	// nBinaryAlgorithm
	//		Two-valued conversion method 
	//		value    define
	//		0     	uses shake method, which has good efficiency to colorful pictures. 
	//		1     	uses average threshold value method, which has good efficiency to text pictures. 
	// return 
	//		Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_ImagePrint(const wchar_t *FileName, int nWidth = 384, int nBinaryAlgorithm = 0);
	// Print NV LOGO
	// n
	//		Print the N Logo,value range is[1,9]
	// nMode
	//		Specify the mode of printing the Logo, and the value is defined as follows:
	//      value    define
	//      0     	normal
	//      1     	double width
	//      2     	double height
	//		3	  	double width | double height
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      If there is no pre-loaded Logo in the printer, it will not print.Please use the printer tool to preload
	CSNPRINTERLLIBS bool Pos_PrintNVLogo(unsigned short nLogo, unsigned short nWidth = 0);

	/*******************************************The query module*********************************************/
	// Select printer error
	// nTimeout
	//		Single time to query status exceeds time.
	// return
	//		Return the error value, and the value is defined as follows:
	//      value		define
	//      1			pirnter good
	//      -1			off-line
	//      -2			not close cover
	//      -3			paper shortage
	//      -4			cut error
	//      -5			hyperpyrexia
	//      -6			failed
	// remarks
	//      The API cannot return more than one exception at a time.If you need to get more than one exception, 
	//		implement it yourself using the Pos_QueryStstus function.Do not insert query operations during printing.
	CSNPRINTERLLIBS int Pos_QueryPrinterErr(unsigned long nTimeout = 3000);

	// Query status 
	// rBuffer
	//		Stores the state returned by the printer
	// type
	//		The data table for the query.value range is[1,4],See the documentation for the specific form.
	// nTimeout
	//		Single time to query status exceeds time.
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Pos_QueryStstus(char *rBuffer, int type, unsigned long nTimeout);

	/*******************************************Set up the module*********************************************/
	// Set the printer baud rate
	// nBaudrate
	//		Set the printer baud rate.
	//		for example£º9600 19200 38400 57600 115200
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      If you connect with a serial port, restart OpenCOM after setting the baud rate.
	CSNPRINTERLLIBS bool Pos_SetPrinterBaudrate(int nBaudrate);
	// Set basic printer parameters
	// nFontStyle
	//		Set the font specification, each value is defined as follows:
	//		value      fout
	//		0		   9*17
	//		1		   12*24
	//		2		   9*24
	//		3		   16*18
	// nDensity
	//		Set the concentration, each value is defined as follows:
	//		value      fout
	//		0		light
	//		1		normal
	//		2		little Dark
	//		3		Dark
	// nLine 
	//      Set the feeding mode, each value is defined as follows:
	//		value      mode
	//		0		   0x0A
	//		1		   0x0D
	// nBeep 
	//      Whether the buzzer is enabled, each value is defined as follows:
	//		value      on/off
	//		0			off
	//		1			on
	// nCut 
	//      Whether the cutter is enabled, each value is defined as follows:
	//		value      on/off
	//		0			off
	//		1			on
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Font specification only sets non-double-byte text, while Chinese, Japanese, Korean and other fonts are 24*24 and cannot be modified.
	//		Buzzer switch please confirm whether the model used has buzzer function
	//		Please confirm whether the machine is equipped with cutting function
	CSNPRINTERLLIBS bool Pos_SetPrinterBasic(int nFontStyle, int nDensity, int nLine, int nBeep, int nCut);

	/*******************************************PAGE module*********************************************/
	// Select page mode
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Make sure the printer has page-mode functionality, which does not print directly. You need to call Page_PrintPage after filling in the data.
	CSNPRINTERLLIBS bool Page_SelectPageMode();
	// Print the content in page mode
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      Make sure the printer has page-mode functionality, which does not print directly. You need to call Page_PrintPage after filling in the data.
	CSNPRINTERLLIBS bool Page_PrintPage();
	// Exit page mode
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_ExitPageMode();
	// Set the vertical absolute print position
	// nPosition
	//      Print position
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetVerticalAbsolutePrintPosition(unsigned short nPosition);
	// Set the horizontal absolute print position
	// nPosition
	//      Print position
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetHorizontalAbsolutePrintPosition(unsigned short nPosition);
	// Set the vertical relative to the print position
	// nPosition
	//      Print position
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetVerticalRelativePrintPosition(unsigned short nPosition);
	// Set the horizontal relative print position
	// nPosition
	//      Print position
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetHorizontalRelativePrintPosition(unsigned short nPosition);
	// Set the printing direction in page mode
	// nDirection
	//      Print the direction of the region, and each value is defined as follows:
	//      0    from left to right
	//      1    from the bottom up
	//      2    from right to left 
	//      3    from top to bottom
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetPageModeDrawDirection(unsigned short nPosition);
	// Set the page area in page mode
	// x
	//      Transverse starting position
	//
	// y
	//      Longitudinal starting position
	//
	// w
	//      Print width
	//
	// h
	//      Print height
	//
	// return 
	//      Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_SetPageArea(unsigned short x, unsigned short y, unsigned short w, unsigned short h);
	// UNICODE text in page mode
	// prnText
	//		UNICODE String
	// nOrgx
	//		The value of the printed text position is defined as follows:
	//      value    define
	//      -1    	 align left
	//      -2    	 align center
	//      -3    	 align right
	//		>=0	  	 Starting from the n hot
	// nWidthTimes
	//		Multiple of width magnification£¬value range is[0,7]
	// nHeightTimes
	//		Higher magnification£¬value range is[0,7]
	// FontType
	//		Type of font to be printed, each value is defined as follows:
	//      value    define
	//      0	  	 12*24
	//		1	  	 9*17
	// nFontStyle
	//		Type of font to be printed, each value is defined as follows:
	//      value		define
	//      0x00		normal
	//		0x08		bold
	//		0x80    	1 hot underline
	//		0x100   	2 hot underline
	//		0x200		convert, only efficient in line start
	//		0x400		inverse, white in black
	//		0x1000		every character rotates clockwise 90¡ã
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      NFontStyle values can be used with & to allow multiple styles to occur simultaneously.
	CSNPRINTERLLIBS bool Page_UnicodeText(const char *prnText, int nOrgx, int nWidthTimes, int nHeightTimes, int FontType, int nFontStyle);
	// Print text in page mode
	// prnText
	//		Text that needs to be printed
	// nLan
	//		Type of text encoding for printing, each value is defined as follows:
	//      value    define
	//      0     	 GBK
	//      1     	 UTF-8
	//      3     	 BIG-5
	//		4	  	 SHIFT-JIS
	//		5	  	 EUC-KR
	// nOrgx
	//		The value of the printed text position is defined as follows:
	//      value    define
	//      -1    	 align left
	//      -2    	 align center
	//      -3    	 align right
	//		>=0	  	 Starting from the n hot
	// nWidthTimes
	//		Multiple of width magnification£¬value range is[0,7]
	// nHeightTimes
	//		Higher magnification£¬value range is[0,7]
	// FontType
	//		Type of font to be printed, each value is defined as follows:
	//      value    define
	//      0	  	 12*24
	//		1	  	 9*17
	// nFontStyle
	//		Type of font to be printed, each value is defined as follows:
	//      value		define
	//      0x00		normal
	//		0x08		bold
	//		0x80    	1 hot underline
	//		0x100   	2 hot underline
	//		0x200		convert, only efficient in line start
	//		0x400		inverse, white in black
	//		0x1000		every character rotates clockwise 90¡ã
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      NFontStyle values can be used with & to allow multiple styles to occur simultaneously.
	CSNPRINTERLLIBS bool Page_Text(const wchar_t *prnText, int nLan, int nOrgx, int nWidthTimes, int nHeightTimes, int FontType, int nFontStyle);
	// Print the barcode in page mode
	// BarcodeData
	//		Printed bar code content
	// nBarcodeType
	//		Type of barcode printed, each value is defined as follows:
	//      value		define
	//      0x41     	UPC-A
	//      0x42     	UPC-E
	//      0x43     	EAN13
	//      0x44     	EAN8
	//      0x45     	CODE39
	//      0x46     	ITF
	//      0x47     	CODABAR
	//      0x48     	CODE93
	// nOrgx
	//		The value of the printed bar code position is defined as follows:
	//      value    define
	//      -1    	 align left
	//      -2    	 align center
	//      -3    	 align right
	//		>=0	  	 Starting from the n hot
	// nUnitWidth
	//		Bar code width£¬value range is[1,6]
	// nUnitHeight
	//		Bar code height£¬value range is[1,255]
	// nFontStyle
	//		The font type of readable character (HRI) is defined as follows:
	//      value    define
	//      0     	 12*24
	//      1     	 9*17
	// FontPosition
	//		The printing positions of readable characters (HRI) are defined as follows:
	//      value    define
	//      0     	 Doesn¡¯t print
	//      1     	 Only print in barcode upward
	//      2     	 Only print in barcode downward
	//		3	  	 Print both barcode upward and downward
	// return 
	//      Returns true on successful write, false on failed write.
	// remarks
	//      nUnitWidth If the maximum width of the printer is exceeded, it is not printed.
	CSNPRINTERLLIBS bool Page_Barcode(const char * BarcodeData, int nBarcodeType, int nOrgx, int nUnitWidth, int nUnitHeight, int nFontStyle, int FontPosition);
	// Print QR code in page mode
	// QrcodeData
	//		QR code character string.
	// nWidth
	// 		Unit width of each module for QR code,value range is[1,16]
	// 		Set module width properly can make QR code nicer 
	// nErrlevenl
	//		Error correction level£¬value range is[1,4]
	// return 
	//		Returns true on successful write, false on failed write.
	// remarks
	//		If the printed qr code exceeds the print boundary, it is not printed.
	CSNPRINTERLLIBS bool Page_Qrcode(const wchar_t *QrcodeData, int nWidth = 4, int nErrlevenl = 4);
	// Print picture in page mode
	// FileName
	//		Image path.
	// nWidth
	//		Specifies the width (pixels) for the printer to print the image
	//		The max. width doesn¡¯t exceed 384 dots of 2 inches printer(58mm printer)
	//		The max. width doesn¡¯t exceed 576 dots of 3 inches printer(80mm printer)
	// nBinaryAlgorithm
	//		Two-valued conversion method 
	//		value    define
	//		0     	uses shake method, which has good efficiency to colorful pictures. 
	//		1     	uses average threshold value method, which has good efficiency to text pictures. 
	// return 
	//		Returns true on successful write, false on failed write.
	CSNPRINTERLLIBS bool Page_ImagePrint(const wchar_t *FileName, int nWidth = 384, int nBinaryAlgorithm = 0);


#ifdef __cplusplus
}
#endif

#endif // !_PRINTERLIBS_H_
