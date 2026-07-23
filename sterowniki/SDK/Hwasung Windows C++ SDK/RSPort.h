#pragma once

#define        NUL        0x00
#define        SOH        0x01
#define        STX        0x02
#define        ETX        0x03
#define        EOT        0x04
#define        ENQ        0x05
#define        ACK        0x06
#define        NAK        0x15
#define        XON        0x11
#define        XOFF    0x13
#define        ESC        0x1b
#define        CR        0x0d
#define        LF        0x0a

const    int    DEF_MAX_SERIAL_PORT = 6;            ///< Serial Port ¼ö·®
const    int ERR_SERIAL_PORT_SUCCESS = 0;        // Success
const    int ERR_PORT_OPEN_FAIL = 1;
const    int ERR_TIME_OUT = 2;

class CRSPort
{
public:
	BOOL           m_Connect;
	HANDLE         m_idComDev;
public:
	int ReadCommPort(unsigned char *message, DWORD length);
	int WriteCommPort(BYTE message[], DWORD dwLength);
	int WriteCommPort(CString message, DWORD dwLength);
	bool IsCommPortOpen();
	CRSPort(CString m_portName);
	CRSPort(CString m_portName, DWORD BaudRate, BYTE ByteSize, BYTE Parity, BYTE StopBits);
	virtual ~CRSPort();
void CloseCommPort(void);
protected:
	
	BOOL SetupConnection(void);
	void initComport(CString m_portName);
	OVERLAPPED     osWrite;
	OVERLAPPED     osRead;
	DCB            dcb_setup;

};

