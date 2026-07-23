#include "stdafx.h"
#include "RSPort.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#define new DEBUG_NEW
#endif



CRSPort::CRSPort(CString m_portName)
{
	dcb_setup.BaudRate = CBR_19200;
	dcb_setup.ByteSize = 8;
	dcb_setup.Parity = NOPARITY;
	dcb_setup.StopBits = ONESTOPBIT;
	initComport(m_portName);
}
CRSPort::CRSPort(CString m_portName, DWORD BaudRate, BYTE ByteSize, BYTE Parity, BYTE StopBits)
{
	dcb_setup.BaudRate = BaudRate;
	dcb_setup.ByteSize = ByteSize;
	dcb_setup.Parity = Parity;
	dcb_setup.StopBits = StopBits;
	initComport(m_portName);
}

CRSPort::~CRSPort()
{
	CloseCommPort();
}

void CRSPort::initComport(CString m_portName)
{
	COMMTIMEOUTS  commTimeOuts;

	m_idComDev = CreateFile((LPCTSTR)m_portName, GENERIC_READ | GENERIC_WRITE,
		0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED, NULL);

	if (m_idComDev == (HANDLE)-1)
	{
		CloseHandle(m_idComDev);
		m_Connect = FALSE;
		//AfxMessageBox(_T("WARNING : 포트를 여는데 실패하였습니다.")); 
	}
	else
	{
		SetCommMask(m_idComDev, EV_RXCHAR);
		SetupComm(m_idComDev, 4096, 4096);
		PurgeComm(m_idComDev, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);
		commTimeOuts.ReadIntervalTimeout = -1;
		commTimeOuts.ReadTotalTimeoutMultiplier = 0;
		commTimeOuts.ReadTotalTimeoutConstant = 1000;
		commTimeOuts.WriteTotalTimeoutMultiplier = 0;
		commTimeOuts.WriteTotalTimeoutConstant = 1000;
		SetCommTimeouts(m_idComDev, &commTimeOuts);
		m_Connect = SetupConnection();
		osWrite.Offset = 0;
		osWrite.OffsetHigh = 0;
		osRead.Offset = 0;
		osRead.OffsetHigh = 0;
		osRead.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
		osWrite.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	}
}

BOOL CRSPort::SetupConnection()
{
	BOOL fRetVal;
	DCB  dcb;

	dcb.DCBlength = sizeof(DCB);
	GetCommState(m_idComDev, &dcb);
	dcb.BaudRate = dcb_setup.BaudRate;
	dcb.ByteSize = dcb_setup.ByteSize;
	dcb.Parity = dcb_setup.Parity;
	dcb.StopBits = dcb_setup.StopBits;
	dcb.fOutxDsrFlow = 0;
	dcb.fDtrControl = DTR_CONTROL_ENABLE;
	dcb.fOutxCtsFlow = 0;
	dcb.fRtsControl = RTS_CONTROL_ENABLE;
	dcb.fInX = dcb.fOutX = 0; // XON/XOFF
	dcb.XonChar = 0x11; // ASCII_XON;
	dcb.XoffChar = 0x13; // ASCII_XOFF;
	dcb.XonLim = 100;
	dcb.XoffLim = 100;
	dcb.fBinary = TRUE;
	dcb.fParity = TRUE;
	fRetVal = SetCommState(m_idComDev, &dcb);
	return fRetVal;
}

void CRSPort::CloseCommPort()
{
	if (m_Connect == FALSE) return;
	CloseHandle(m_idComDev);
	CloseHandle(osRead.hEvent);
	CloseHandle(osWrite.hEvent);
}

int CRSPort::WriteCommPort(BYTE message[], DWORD dwLength)
{
	/*int ret_code;
	ret_code = WriteFile(m_idComDev, message, dwLength, &dwLength, &osWrite);
	return ret_code;*/

	BOOL        fWriteStat;
	DWORD       dwBytesWritten;
	DWORD       dwErrorFlags;
	DWORD   	dwError;
	DWORD       dwBytesSent = 0;
	COMSTAT     ComStat;
	char        szError[128];

	fWriteStat = WriteFile(m_idComDev, message, dwLength, &dwLength, &osWrite);

	if (!fWriteStat)	// 써야할 바이트를 다 쓰지 못했다
	{
		if (GetLastError() == ERROR_IO_PENDING)	// I/O Pending에 의한 경우
		{
			// I/O가 완료되기를 기다린다
			while (!GetOverlappedResult(m_idComDev, &osWrite, &dwBytesWritten, TRUE))
			{
				dwError = GetLastError();
				if (dwError == ERROR_IO_INCOMPLETE)
				{
					// 보낸 전체 바이트 수를 체크
					dwBytesSent += dwBytesWritten;
					continue;
				}
				else
				{
					// 에러 발생
					wsprintf(szError, (CString)"<CE-%u>", dwError);
					printf("%s\r\n", szError);
					//WriteTTYBlock( hWnd, szError, lstrlen( szError ) ) ;
					ClearCommError(m_idComDev, &dwErrorFlags, &ComStat);
					break;
				}
			}

			dwBytesSent += dwBytesWritten;

			if (dwBytesSent != dwLength)	// 보내야 할 바이트와 보낸 바이트가 일치하지 않는 경우
				wsprintf(szError, (CString)"\nProbable Write Timeout: Total of %ld bytes sent", dwBytesSent);
			else	// 성공적으로 모두 보낸 경우
				wsprintf(szError, (CString)"\n%ld bytes written", dwBytesSent);

			//OutputDebugString((LPWSTR)szError);
		}
		else // I/O Pending 외의 다른 에러
		{
			ClearCommError(m_idComDev, &dwErrorFlags, &ComStat);
			return FALSE;
		}
	}


	return TRUE;

}
int CRSPort::WriteCommPort(CString message, DWORD dwLength)
{
	//int ret_code;

	int nLength = message.GetLength();
	int nBytes = WideCharToMultiByte(CP_ACP, 0, (LPWSTR)(LPCTSTR)message, nLength, NULL, 0, NULL, NULL);
	char* VoicePath = new char[nBytes + 1];
	memset(VoicePath, 0, nLength + 1);
	WideCharToMultiByte(CP_OEMCP, 0, (LPWSTR)(LPCTSTR)message, nLength, VoicePath, nBytes, NULL, NULL);
	VoicePath[nBytes] = 0;

	/*ret_code = WriteFile(m_idComDev, VoicePath, dwLength, &dwLength, &osWrite);
	return ret_code;*/


	BOOL        fWriteStat;
	DWORD       dwBytesWritten;
	DWORD       dwErrorFlags;
	DWORD   	dwError;
	DWORD       dwBytesSent = 0;
	COMSTAT     ComStat;
	char        szError[128];

	fWriteStat = WriteFile(m_idComDev, message, dwLength, &dwLength, &osWrite);

	if (!fWriteStat)	// 써야할 바이트를 다 쓰지 못했다
	{
		if (GetLastError() == ERROR_IO_PENDING)	// I/O Pending에 의한 경우
		{
			// I/O가 완료되기를 기다린다
			while (!GetOverlappedResult(m_idComDev, &osWrite, &dwBytesWritten, TRUE))
			{
				dwError = GetLastError();
				if (dwError == ERROR_IO_INCOMPLETE)
				{
					// 보낸 전체 바이트 수를 체크
					dwBytesSent += dwBytesWritten;
					continue;
				}
				else
				{
					// 에러 발생
					wsprintf(szError, (CString)"<CE-%u>", dwError);
					printf("%s\r\n", szError);
					//WriteTTYBlock( hWnd, szError, lstrlen( szError ) ) ;
					ClearCommError(m_idComDev, &dwErrorFlags, &ComStat);
					break;
				}
			}

			dwBytesSent += dwBytesWritten;

			if (dwBytesSent != dwLength)	// 보내야 할 바이트와 보낸 바이트가 일치하지 않는 경우
				wsprintf(szError, (CString)"\nProbable Write Timeout: Total of %ld bytes sent", dwBytesSent);
			else	// 성공적으로 모두 보낸 경우
				wsprintf(szError, (CString)"\n%ld bytes written", dwBytesSent);

			//OutputDebugString((LPWSTR)szError);
		}
		else // I/O Pending 외의 다른 에러
		{
			ClearCommError(m_idComDev, &dwErrorFlags, &ComStat);
			return FALSE;
		}
	}


	return TRUE;





}

int CRSPort::ReadCommPort(unsigned char *message, DWORD length)
{
	COMSTAT  ComStat;
	DWORD    dwErrorFlags;
	DWORD    dwLength;
	DWORD    dwReadLength = 0;

	CStringA strTemp;
	strTemp.Format("%s", message);

	if (m_Connect == FALSE)  return 0;
	else
	{
		ClearCommError(m_idComDev, &dwErrorFlags, &ComStat);
		dwLength = min((DWORD)length, ComStat.cbInQue);
		ReadFile(m_idComDev, message, dwLength, &dwReadLength, &osRead);
	}
	if (dwReadLength == 0)
	{
		CStringA str;
		str.Format("%s", message);

		if (strTemp != str)
		{
			return str.GetLength();
		}
	}


	return dwReadLength;
}

bool CRSPort::IsCommPortOpen()
{
	if (m_Connect){
		return true;
	}
	return false;
}
