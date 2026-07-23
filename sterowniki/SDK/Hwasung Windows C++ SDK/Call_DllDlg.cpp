// Call_DllDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Call_Dll.h"
#include "Call_DllDlg.h"


#include <windows.h>
#include <Winbase.h>




#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



// HWASUNG DLL EXPORT FUNCTIONS
long __stdcall UsbOpen(LPCTSTR SelPrinter);
long __stdcall PrintStr(LPCTSTR data); 
long __stdcall PrintCmd(short data);
long __stdcall PrintPacket(unsigned char *PacketBuf, unsigned long PacketLength);
long __stdcall NewRealRead();
long __stdcall DummyRealRead();
void __stdcall UsbClose(); 



/////////////////////////////////////////////////////////////////////////////
// CCall_DllDlg dialog

CCall_DllDlg::CCall_DllDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CCall_DllDlg::IDD, pParent)
	, Text1(_T(""))
	, Text2(_T(""))
{
	//{{AFX_DATA_INIT(CCall_DllDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CCall_DllDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CCall_DllDlg)
	//  DDX_Control(pDX, IDC_COMBO1, m_ModelName);
	//}}AFX_DATA_MAP
	DDX_Control(pDX, IDC_COMBO1, Combo1);
	DDX_Control(pDX, IDC_COMBO2, Combo2);
	DDX_Control(pDX, IDC_COMBO3, Combo3);
	DDX_Control(pDX, IDC_COMBO4, Combo4);
	DDX_Control(pDX, IDC_COMBO5, Combo5);
	DDX_Text(pDX, IDC_EDIT1, Text1);
	DDX_Text(pDX, IDC_EDIT2, Text2);
	DDX_Control(pDX, IDC_VER, VER);
}

BEGIN_MESSAGE_MAP(CCall_DllDlg, CDialog)
	//{{AFX_MSG_MAP(CCall_DllDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, OnButton1)
	ON_BN_CLICKED(IDC_BUTTON2, OnButton2)
	ON_BN_CLICKED(IDC_BUTTON3, OnButton3)
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON4, &CCall_DllDlg::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON5, &CCall_DllDlg::OnBnClickedButton5)
	ON_BN_CLICKED(IDC_BUTTON6, &CCall_DllDlg::OnBnClickedButton6)
	ON_BN_CLICKED(IDC_BUTTON7, &CCall_DllDlg::OnBnClickedButton7)
	ON_BN_CLICKED(IDC_BUTTON8, &CCall_DllDlg::OnBnClickedButton8)
	ON_BN_CLICKED(IDC_BUTTON9, &CCall_DllDlg::OnBnClickedButton9)
	ON_BN_CLICKED(IDC_BUTTON10, &CCall_DllDlg::OnBnClickedButton10)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CCall_DllDlg message handlers

BOOL CCall_DllDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon



	Combo1.AddString(_T("HMK-060"));
	Combo1.AddString(_T("HMK-080"));
	Combo1.AddString(_T("HMK-081"));
	Combo1.AddString(_T("HMK-825"));
	Combo1.AddString(_T("HMK-830"));
	Combo1.AddString(_T("HMK-072"));
	Combo1.AddString(_T("HMK-054"));
	Combo1.AddString(_T("HMK-056"));
	Combo1.AddString(_T("HP-380"));
	Combo1.AddString(_T("HP-083"));
	Combo1.AddString(_T("HP-283"));
	Combo1.SetCurSel(0);

	Combo2.AddString(_T("USB"));
	Combo2.AddString(_T("COM1"));
	Combo2.AddString(_T("COM2"));
	Combo2.AddString(_T("COM3"));
	Combo2.AddString(_T("COM4"));
	Combo2.AddString(_T("COM5"));
	Combo2.AddString(_T("COM6"));
	Combo2.AddString(_T("COM7"));
	Combo2.SetCurSel(0);

	Combo3.AddString(_T("9600"));
	Combo3.AddString(_T("19200"));
	Combo3.AddString(_T("38400"));
	Combo3.AddString(_T("57600"));
	Combo3.AddString(_T("115200"));
	Combo3.SetCurSel(0);

	Combo4.AddString(_T("UPC-E")); 
	Combo4.AddString(_T("EAN13")); 
	Combo4.AddString(_T("EAN8")); 
	Combo4.AddString(_T("CODE39")); 
	Combo4.AddString(_T("ITF(I of 2/5)"));
	Combo4.AddString(_T("CODABAR")); 
	Combo4.AddString(_T("CODE128 A")); 
	Combo4.AddString(_T("CODE128 B")); 
	Combo4.AddString(_T("CODE128 C")); 
	Combo4.SetCurSel(0);

	Combo5.AddString(_T("Version 1"));
	Combo5.AddString(_T("Version 3"));
	Combo5.AddString(_T("Version 5"));
	Combo5.AddString(_T("Version 9"));
	Combo5.SetCurSel(0);

	m_bSerialConnected = false;

	
	SetDlgItemText(IDC_EDIT1, (CString)"0123456789");
	SetDlgItemText(IDC_EDIT2, (CString)"0123456789");


	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CCall_DllDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CCall_DllDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}



void CCall_DllDlg::OnCancel() 
{
	// TODO: Add extra cleanup here
	
	CDialog::OnCancel();

// USB port close
	UsbClose();

}


void CCall_DllDlg::OnButton1() 
{
	   

	if (Combo2.GetCurSel() ==0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
		}
		else{
			//t.Format("%s", "Open Success!");
			ret = NewRealRead();
			print_status(ret);	
		}	
	}
	else
	{
		
	Combo2.GetLBText(Combo2.GetCurSel(),m_str_comport);
	Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
	
	CString m_strAddress = m_str_baudrate;
	DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
	m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
	if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[3] = { 0x10, 0x04, 0x02 };
			byte rcv[1];
			m_comm->WriteCommPort(send,sizeof(send));
			Sleep(100);
			m_comm->ReadCommPort(rcv, 1);
			print_status(rcv[0]);	

			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}

}

void CCall_DllDlg::OnButton2() 
{
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{			

			byte MyData[1];
			CString send_str;
			
	
			//PrintPacket(send, 3);

			PrintCmd( 0x1B);							//Text Align center
			PrintCmd( 0x61);
			PrintCmd( 0x01);
			//PrintPacket(send, 3);
			send_str = "Starbucks Coffee Germany\n";
			PrintStr(send_str);

			send_str = "Frankfrut am Main\n";
			PrintStr(send_str);

			send_str = "Kaiserstra";
			PrintStr(send_str);
			PrintCmd(0x1A);
			PrintCmd(0x78);
			PrintCmd(0x01);							//Extended ascii mode 

			MyData[0] = 225;
			PrintCmd(MyData[0]);

			PrintCmd(0x1A);
			PrintCmd(0x78);
			PrintCmd(0x00);							//Extended ascii mode disable

			send_str = "e\n";
			PrintStr(send_str);

			send_str = "Tele:\n";
			PrintStr(send_str);

			send_str = "VAT:  6417373R\n\n";
			PrintStr(send_str);

			PrintCmd( 0x1D);							//Text Align center
			PrintCmd(0x4c);
			PrintCmd(0x12);
			PrintCmd( 0x00);
			//PrintPacket(send, 4);

			send_str = " 100003 Claire 0\n";
			PrintStr(send_str);

			send_str = "----------------------------------------\n";
			PrintStr(send_str);
			send_str = "Chk 806             13Oct'16 15:48\n";
			PrintStr(send_str);
			send_str = "----------------------------------------\n";
			PrintStr(send_str);

			PrintCmd(0x1D);							//left margin
			PrintCmd( 0x4c);
			PrintCmd( 0x32);
			PrintCmd( 0x00);
			//PrintPacket(send, 4);

			PrintCmd(0x1D);							//Font width 2x
			PrintCmd(0x21);
			PrintCmd(0x10);
			//PrintPacket(send, 3);

			send_str = "To Go\n";
			PrintStr(send_str);

			PrintCmd( 0x1D);							//Font width normal
			PrintCmd( 0x21);
			PrintCmd( 0x00);
			//PrintPacket(send, 3);

			send_str = "Gr Carml Macchiato                4.65\n";
			PrintStr(send_str);
			send_str = "  Decaf\n";
			PrintStr(send_str);
			send_str = "Gr Latte                          3.95\n";
			PrintStr(send_str);
			send_str = "  Hazelnut                        0.05\n";
			PrintStr(send_str);
			send_str = "Tl Chai Tea Latte                 3.45\n";
			PrintStr(send_str);
			send_str = "Visa                             13.77\n";
			PrintStr(send_str);
			send_str = "XXXXXXXXXXXX4258\n\n";
			PrintStr(send_str);
			send_str = "Subtotal                    EURO 12.55\n";
			PrintStr(send_str);
			send_str = "Tax 9.75%                   EURO  1.22\n";
			PrintStr(send_str);
			send_str = "Total                       EURO 13.77\n";
			PrintStr(send_str);
			send_str = "Change Due                  EURO  0.00\n\n";
			PrintStr(send_str);
			send_str = "========================================\n";
			PrintStr(send_str);
			send_str = "Thank you. Please visit us again\n";
			PrintStr(send_str);
			send_str = "For more information visit\n";
			PrintStr(send_str);
			send_str = "www.starbucks.de\n\n\n\n\n\n";
			PrintStr(send_str);			

			PrintCmd( 0x1A);
			PrintCmd( 0x78);
			PrintCmd( 0x00);							//Extended ascii mode off 
			//PrintPacket(send, 3);

			PrintCmd( 0x1B);							//Text Align left
			PrintCmd( 0x61);
			PrintCmd( 0x00);
			//PrintPacket(send, 3);

			PrintCmd( 0x1b);							//Full Cut
			PrintCmd( 0x69);
			//PrintPacket(send, 2);

		}	
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
		
	
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);        // initial Comm port
		if (m_comm->IsCommPortOpen() ) //Port Open Check
		{
			byte send[5];
			byte MyData[1];
			CString send_str;
			
			

			send[0] = 0x1B;							//Text Align center
			send[1] = 0x61;
			send[2] = 0x01;
			m_comm->WriteCommPort(send, 3);
			send_str = "Starbucks Coffee Germany\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send_str = "Frankfrut am Main\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send_str = "Kaiserstra";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1A;
			send[1] = 0x78;
			send[2] = 0x01;							//Extended ascii mode 
			m_comm->WriteCommPort(send, 3);

			MyData[0] = 225;
			m_comm->WriteCommPort(MyData, sizeof(MyData));

			send[0] = 0x1A;
			send[1] = 0x78;
			send[2] = 0x00;							//Extended ascii mode disable
			m_comm->WriteCommPort(send, 3);

			send_str = "e\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send_str = "Tele:\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send_str = "VAT:  6417373R\n\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1D;							//Text Align center
			send[1] = 0x4c;
			send[2] = 0x12;
			send[3] = 0x00;
			m_comm->WriteCommPort(send, 4);

			send_str = " 100003 Claire 0\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send_str = "----------------------------------------\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Chk 806             13Oct'16 15:48\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "----------------------------------------\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1D;							//left margin
			send[1] = 0x4c;
			send[2] = 0x32;
			send[3] = 0x00;
			m_comm->WriteCommPort(send, 4);

			send[0] = 0x1D;							//Font width 2x
			send[1] = 0x21;
			send[2] = 0x10;
			m_comm->WriteCommPort(send, 3);

			send_str = "To Go\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1D;							//Font width normal
			send[1] = 0x21;
			send[2] = 0x00;
			m_comm->WriteCommPort(send, 3);

			send_str = "Gr Carml Macchiato                4.65\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "  Decaf\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Gr Latte                          3.95\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "  Hazelnut                        0.05\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Tl Chai Tea Latte                 3.45\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Visa                             13.77\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "XXXXXXXXXXXX4258\n\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Subtotal                    EURO 12.55\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Tax 9.75%                   EURO  1.22\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Total                       EURO 13.77\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Change Due                  EURO  0.00\n\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "========================================\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Thank you. Please visit us again\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "For more information visit\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "www.starbucks.de\n\n\n\n\n\n";
			m_comm->WriteCommPort(send_str, send_str.GetLength());			

			send[0] = 0x1A;
			send[1] = 0x78;
			send[2] = 0x00;							//Extended ascii mode off 
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1B;							//Text Align left
			send[1] = 0x61;
			send[2] = 0x00;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1b;							//Full Cut
			send[1] = 0x69;
			m_comm->WriteCommPort(send, 2);


				m_comm->CloseCommPort();
				m_comm = NULL;
			
		}
		else
		{
			AfxMessageBox(_T("Serial ERROR!"));
		}

	}

}

void CCall_DllDlg::OnButton3() 
{
	// TODO: Add your control notification handler code here
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{				
					
			CString send_str;

			PrintCmd(0x1B);							// PAGE MODE
			PrintCmd(0x4C);			
			
			PrintCmd(0x1B);							// PAGE TOWARD
			PrintCmd(0x54);
			PrintCmd(0x00);
			
			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			PrintCmd(0x00); //xL
			PrintCmd(0x00); //xH
			PrintCmd(0x00); //yL
			PrintCmd(0x00); //yH
			PrintCmd(0xA0); //dxL
			PrintCmd(0x00); //dxH
			PrintCmd(0xD9); //dyL
			PrintCmd(0x00); //dyH

			//location x = 0mm      
			//location y = 0mm
			//dx =  20mm
			//dy =  27.125mm         
			

			send_str = "ABCDEFGHIJKLM";
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);			
			PrintCmd(0x70); //xL
			PrintCmd(0x00); //xH
			PrintCmd(0x90); //yL
			PrintCmd(0x00); //yH
			PrintCmd(0xA0); //dxL
			PrintCmd(0x00); //dxH
			PrintCmd(0xD9); //dyL
			PrintCmd(0x00); //dyH


			//location x = 14mm
			//location y = 18mm
			//dx =  20mm
			//dy =  27.125mm         
			
			send_str = "1234567890123";
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);				
			PrintCmd(0xA0); //xL
			PrintCmd(0x00); //xH
			PrintCmd(0x60); //yL
			PrintCmd(0x00); //yH
			PrintCmd(0xA0); //dxL
			PrintCmd(0x00); //dxH
			PrintCmd(0xD9); //dyL
			PrintCmd(0x00); //dyH

			//location x = 20mm
			//location y = 12mm
			//dx =  20mm
			//dy =  27.125mm      
			
			send_str = "123ABC456DEF7";
			PrintStr(send_str);

			PrintCmd(0x1B);							// PAGE AREA PRINT
			PrintCmd(0x0C);
			
			PrintCmd(0x1B);							// PAGE AREA CLEAR AND TO STANDARD MODE
			PrintCmd(0x53);
		}
	}
	else
	{

		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
	
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte MyData[8];
			byte send[5];
			CString send_str;

			send[0] = 0x1B;							// PAGE MODE
			send[1] = 0x4C;			
			m_comm->WriteCommPort(send, 2);

			send[0] = 0x1B;							// PAGE TOWARD
			send[1] = 0x54;
			send[2] = 0x00;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);

			MyData[0] = 0x00; //xL
			MyData[1] = 0x00; //xH
			MyData[2] = 0x00; //yL
			MyData[3] = 0x00; //yH
			MyData[4] = 0xA0; //dxL
			MyData[5] = 0x00; //dxH
			MyData[6] = 0xD9; //dyL
			MyData[7] = 0x00; //dyH

			//location x = 0mm      
			//location y = 0mm
			//dx =  20mm
			//dy =  27.125mm         
			m_comm->WriteCommPort(MyData, 8);

			send_str = "ABCDEFGHIJKLM";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			MyData[0] = 0x70; //xL
			MyData[1] = 0x00; //xH
			MyData[2] = 0x90; //yL
			MyData[3] = 0x00; //yH
			MyData[4] = 0xA0; //dxL
			MyData[5] = 0x00; //dxH
			MyData[6] = 0xD9; //dyL
			MyData[7] = 0x00; //dyH


			//location x = 14mm
			//location y = 18mm
			//dx =  20mm
			//dy =  27.125mm         
			m_comm->WriteCommPort(MyData, 8);
			send_str = "1234567890123";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);			
			MyData[0] = 0xA0; //xL
			MyData[1] = 0x00; //xH
			MyData[2] = 0x60; //yL
			MyData[3] = 0x00; //yH
			MyData[4] = 0xA0; //dxL
			MyData[5] = 0x00; //dxH
			MyData[6] = 0xD9; //dyL
			MyData[7] = 0x00; //dyH

			//location x = 20mm
			//location y = 12mm
			//dx =  20mm
			//dy =  27.125mm      
			m_comm->WriteCommPort(MyData, 8);
			send_str = "123ABC456DEF7";
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// PAGE AREA PRINT
			send[1] = 0x0C;
			m_comm->WriteCommPort(send, 2);

			send[0] = 0x1B;							// PAGE AREA CLEAR AND TO STANDARD MODE
			send[1] = 0x53;
			m_comm->WriteCommPort(send, 2);

			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}

}

void CCall_DllDlg::print_status(int status)
{
	switch (status)
	{
	case 0:
		SetDlgItemText(IDC_STATUS, (CString)"Normal Status");
		break;
	case 1:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out");
		break;
	case 2:
		SetDlgItemText(IDC_STATUS, (CString)"Head open");
		break;
	case 3:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Head open");
		break;
	case 4:
		SetDlgItemText(IDC_STATUS, (CString)"Paper Jam");
		break;
	case 5:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Paper Jam");
		break;
	case 6:
		SetDlgItemText(IDC_STATUS, (CString)"Head open && Paper Jam");
		break;
	case 7:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Head open && Paper Jam");
		break;
	case 8:
		SetDlgItemText(IDC_STATUS, (CString)"Near End");
		break;
	case 9:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Near end");
		break;
	case 10:
		SetDlgItemText(IDC_STATUS, (CString)"Head open && Near end");
		break;
	case 11:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Head open && Near end");
		break;
	case 12:
		SetDlgItemText(IDC_STATUS, (CString)"Paper Jam && Near end");
		break;
	case 13:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Paper Jam && Near end");
		break;
	case 14:
		SetDlgItemText(IDC_STATUS, (CString)"Head open && Paper Jam && Near end");
		break;
	case 15:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Head open && Paper Jam && Near end");
		break;
	case 16:
		SetDlgItemText(IDC_STATUS, (CString)"Print Running");
		break;
	case 32:
		SetDlgItemText(IDC_STATUS, (CString)"Cutter Jam");
		break;
	}
}

void CCall_DllDlg::OnBnClickedButton4()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{	
			
			CString send_str;
			
			PrintCmd(0x1B);							// PAGE MODE
			PrintCmd(0x4C);
			
			PrintCmd(0x1B);							// PAGE TOWARD
			PrintCmd(0x54);
			PrintCmd(0x01);
			

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0010";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "Thermal Printer Ticket Sample";						// Text
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0104";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "Hwasung System Thermal Printer";						// Text
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0136";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "Sample Print";						// Text
			PrintStr(send_str);

			PrintCmd(0x1D);							// DOUBLE WIDTH SIZE
			PrintCmd(0x21);
			PrintCmd(0x00);
			

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0216";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "2019-10-24 3:45 PM";						// Text
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
		
			send_str = "0280";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "Page Mode Ticket Sample";						// Text
			PrintStr(send_str);

			PrintCmd(0x1D);							// DOUBLE HEIGHT SIZE
			PrintCmd(0x21);
			PrintCmd(0x10);			

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0104";						// X Location
			PrintStr(send_str);
			send_str = "0304";						// Y Location
			PrintStr(send_str);
			send_str = "Page Mode";						// Text
			PrintStr(send_str);
			
			PrintCmd(0x1D);							// NORMAL SIZE
			PrintCmd(0x21);
			PrintCmd(0x00);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);

			send_str = "0168";						// X Location
			PrintStr(send_str);
			send_str = "0304";						// Y Location
			PrintStr(send_str);
			send_str = "Sample Print";						// Text
			PrintStr(send_str);

			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0416";						// X Location
			PrintStr(send_str);
			send_str = "1160";						// Y Location
			PrintStr(send_str);
			send_str = "Thermal Printer Ticket Sample";						// Text
			PrintStr(send_str);

			//--------- BARCODE --------------------------------
			PrintCmd(0x1B);							// Text Location
			PrintCmd(0x57);
			
			send_str = "0376";						// X Location
			PrintStr(send_str);
			send_str = "0688";						// Y Location
			PrintStr(send_str);
			PrintCmd(0x1D);							// barcode height
			PrintCmd(0x68);
			PrintCmd(0x28);
			
			PrintCmd(0x1D);							// barcode type
			PrintCmd(0x6B);
			PrintCmd(0x05);
			
			send_str = "010001200307311439";						// barcode data
			PrintStr(send_str);
			PrintCmd(0x00);	
			
			PrintCmd(0x1B);							// PAGE AREA PRINT
			PrintCmd(0x0C);
		
			PrintCmd(0x1B);							// PAGE AREA CLEAR AND TO STANDARD MODE
			PrintCmd(0x53);
			
		}

	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
		
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];
			CString send_str;
			
			send[0] = 0x1B;							// PAGE MODE
			send[1] = 0x4C;
			m_comm->WriteCommPort(send, 2);

			send[0] = 0x1B;							// PAGE TOWARD
			send[1] = 0x54;
			send[2] = 0x01;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0010";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Thermal Printer Ticket Sample";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0104";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Hwasung System Thermal Printer";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0136";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Sample Print";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1D;							// DOUBLE WIDTH SIZE
			send[1] = 0x21;
			send[2] = 0x00;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0216";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "2019-10-24 3:45 PM";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0280";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Page Mode Ticket Sample";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1D;							// DOUBLE HEIGHT SIZE
			send[1] = 0x21;
			send[2] = 0x10;
			m_comm->WriteCommPort(send, 3);  

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0104";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "0304";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Page Mode";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			
			send[0] = 0x1D;							// NORMAL SIZE
			send[1] = 0x21;
			send[2] = 0x00;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0168";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "0304";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Sample Print";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0416";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "1160";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "Thermal Printer Ticket Sample";						// Text
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			//--------- BARCODE --------------------------------
			send[0] = 0x1B;							// Text Location
			send[1] = 0x57;
			m_comm->WriteCommPort(send, 2);
			send_str = "0376";						// X Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send_str = "0688";						// Y Location
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send[0] = 0x1D;							// barcode height
			send[1] = 0x68;
			send[2] = 0x28;
			m_comm->WriteCommPort(send, 3);

			send[0] = 0x1D;							// barcode type
			send[1] = 0x6B;
			send[2] = 0x05;
			m_comm->WriteCommPort(send, 3);
			send_str = "010001200307311439";						// barcode data
			m_comm->WriteCommPort(send_str, send_str.GetLength());
			send[0] = 0x00;	
			m_comm->WriteCommPort(send, 1);

			send[0] = 0x1B;							// PAGE AREA PRINT
			send[1] = 0x0C;
			m_comm->WriteCommPort(send, 2);

			send[0] = 0x1B;							// PAGE AREA CLEAR AND TO STANDARD MODE
			send[1] = 0x53;
			m_comm->WriteCommPort(send, 2);

			
				m_comm->CloseCommPort();
				m_comm = NULL;
			
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}


void CCall_DllDlg::OnBnClickedButton5()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if (Combo2.GetCurSel() == 0)
	{
		
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{	
			
			PrintCmd(0x1B);							
			PrintCmd(0x69);
		}
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
	
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];

			send[0] = 0x1B;							
			send[1] = 0x69;
			m_comm->WriteCommPort(send, 2);
			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}


void CCall_DllDlg::OnBnClickedButton6()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{	
			
			PrintCmd(0x1B);							
			PrintCmd(0x6D);
		}
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
	
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];

			send[0] = 0x1B;
			send[1] = 0x6D;
			m_comm->WriteCommPort(send, 2);
			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}


void CCall_DllDlg::OnBnClickedButton7()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{	
			
			PrintCmd(0x13);							
			PrintCmd(0x69);
		}
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
		
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];

			send[0] = 0x13;
			send[1] = 0x69;
			m_comm->WriteCommPort(send, 2);
			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}


void CCall_DllDlg::OnBnClickedButton8()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{	
		CString send_str,temp;
			
			if (Combo4.GetCurSel() + 1 >= 7)
			{
				PrintCmd(0x1D);
				PrintCmd(0x6B);
				PrintCmd(0x07);
				
				switch (Combo4.GetCurSel() + 1){
				case 7:
					send_str = "g";						
					PrintStr(send_str);
					send_str = Text1.GetString();						
					PrintStr(send_str);
					PrintCmd(0x00);
					
					break;
				case 8:
					send_str = "h";						
					PrintStr(send_str);
					send_str = Text1.GetString();					
					PrintStr(send_str);
					PrintCmd(0x00);
										
					break;
				case 9:
					send_str = "i";						
					PrintStr(send_str);
					send_str = Text1.GetString();						
					PrintStr(send_str);
					PrintCmd(0x00);
										
					break;
				}
			}
			else
			{
				if (Combo4.GetCurSel() + 1 == 1 || Combo4.GetCurSel() + 1 == 3)
				{
					if (GetDlgItemText(IDC_EDIT1,temp)  < 7)
					{
						SetDlgItemText(IDC_EDIT1, (CString)"0123456");
					}
					GetDlgItemText(IDC_EDIT1, temp);
					PrintCmd(0x1D);
					PrintCmd(0x6B);
					PrintCmd(Combo4.GetCurSel() + 1);
					
					send_str = temp.Left(7);
					PrintStr(send_str);
					PrintCmd(0x00);
					

				}
				else if (Combo4.GetCurSel() + 1 == 2)
				{
					if (GetDlgItemText(IDC_EDIT1, temp) < 12)
					{
						SetDlgItemText(IDC_EDIT1, (CString)"123456789012");
					}
					GetDlgItemText(IDC_EDIT1, temp);
					PrintCmd(0x1D);
					PrintCmd(0x6B);
					PrintCmd(Combo4.GetCurSel() + 1);
					
					send_str = temp.Left(12);
					PrintStr(send_str);
					PrintCmd(0x00);
					
				}
				else if (Combo4.GetCurSel() + 1 == 5)
				{
					if (GetDlgItemText(IDC_EDIT1, temp) % 2 == 0)
					{
						PrintCmd(0x1D);
						PrintCmd(0x6B);
						PrintCmd(Combo4.GetCurSel() + 1);
						
						send_str = temp;
						PrintStr(send_str);
						PrintCmd(0x00);
						
					}
					else
					{
						GetDlgItemText(IDC_EDIT1, temp);
						PrintCmd(0x1D);
						PrintCmd(0x6B);
						PrintCmd(Combo4.GetCurSel() + 1);
						
						send_str = temp;
						PrintStr(send_str);
						send_str = "0";
						PrintStr(send_str);
						PrintCmd(0x00);
						
					}
				}
				else
				{
					GetDlgItemText(IDC_EDIT1, temp);
					PrintCmd(0x1D);
					PrintCmd(0x6B);
					PrintCmd(Combo4.GetCurSel() + 1);
					
					send_str = temp;
					PrintStr(send_str);
					PrintCmd(0x00);
					
				}
			}
		}
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
	
		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];
			CString send_str,temp;
			
			if (Combo4.GetCurSel() + 1 >= 7)
			{
				send[0] = 0x1D;
				send[1] = 0x6B;
				send[2] = 0x07;
				m_comm->WriteCommPort(send, 3);
				switch (Combo4.GetCurSel() + 1){
				case 7:
					send_str = "g";						
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send_str = Text1.GetString();						
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);
					break;
				case 8:
					send_str = "h";						
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send_str = Text1.GetString();					
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);					
					break;
				case 9:
					send_str = "i";						
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send_str = Text1.GetString();						
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);					
					break;
				}
			}
			else
			{
				if (Combo4.GetCurSel() + 1 == 1 || Combo4.GetCurSel() + 1 == 3)
				{
					if (GetDlgItemText(IDC_EDIT1,temp)  < 7)
					{
						SetDlgItemText(IDC_EDIT1, (CString)"0123456");
					}
					GetDlgItemText(IDC_EDIT1, temp);
					send[0] = 0x1D;
					send[1] = 0x6B;
					send[2] = Combo4.GetCurSel() + 1;
					m_comm->WriteCommPort(send, 3);
					send_str = temp.Left(7);
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);

				}
				else if (Combo4.GetCurSel() + 1 == 2)
				{
					if (GetDlgItemText(IDC_EDIT1, temp) < 12)
					{
						SetDlgItemText(IDC_EDIT1, (CString)"012345678901");
					}
					GetDlgItemText(IDC_EDIT1, temp);
					send[0] = 0x1D;
					send[1] = 0x6B;
					send[2] = Combo4.GetCurSel() + 1;
					m_comm->WriteCommPort(send, 3);
					send_str = temp.Left(12);
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);
				}
				else if (Combo4.GetCurSel() + 1 == 5)
				{
					if (GetDlgItemText(IDC_EDIT1, temp) % 2 == 0)
					{
						send[0] = 0x1D;
						send[1] = 0x6B;
						send[2] = Combo4.GetCurSel() + 1;
						m_comm->WriteCommPort(send, 3);
						send_str = temp;
						m_comm->WriteCommPort(send_str, send_str.GetLength());
						send[0] = 0x00;
						m_comm->WriteCommPort(send, 1);
					}
					else
					{
						GetDlgItemText(IDC_EDIT1, temp);
						send[0] = 0x1D;
						send[1] = 0x6B;
						send[2] = Combo4.GetCurSel() + 1;
						m_comm->WriteCommPort(send, 3);
						send_str = temp;
						m_comm->WriteCommPort(send_str, send_str.GetLength());
						send_str = "0";
						m_comm->WriteCommPort(send_str, send_str.GetLength());
						send[0] = 0x00;
						m_comm->WriteCommPort(send, 1);
					}
				}
				else
				{
					GetDlgItemText(IDC_EDIT1, temp);
					send[0] = 0x1D;
					send[1] = 0x6B;
					send[2] = Combo4.GetCurSel() + 1;
					m_comm->WriteCommPort(send, 3);
					send_str = temp;
					m_comm->WriteCommPort(send_str, send_str.GetLength());
					send[0] = 0x00;
					m_comm->WriteCommPort(send, 1);
				}
			}

			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}


void CCall_DllDlg::OnBnClickedButton9()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	int str = Combo5.GetCurSel();
	int cnt;
	CString temp;
	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(),t);

		// printer target model name
		ret = UsbOpen(t);
	
		if (ret < 0) {
			t.Format("%s", "Open Failed!");
			AfxMessageBox(t);
		}
		else{
				CString send_str;
			
			cnt = GetDlgItemText(IDC_EDIT2,temp);
			PrintCmd(0x1A);
			PrintCmd(0x42);
			PrintCmd(0x02);
			
			PrintCmd(cnt);
			
			switch (str){
			case 0:
				PrintCmd(0x01);
				
				break;
			case 1:
				PrintCmd(0x03);
				
				break;
			case 2:
				PrintCmd(0x05);
				
				break;
			case 3:
				PrintCmd(0x09);
				
				break;
			}
			send_str = temp;
			PrintStr(send_str);
		}
	}
	else
	{
		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);

		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[5];
			CString send_str;
			
			cnt = GetDlgItemText(IDC_EDIT2,temp);
			send[0] = 0x1A;
			send[1] = 0x42;
			send[2] = 0x02;
			m_comm->WriteCommPort(send, 3);
			send[0] = cnt;
			m_comm->WriteCommPort(send, 1);
			switch (str){
			case 0:
				send[0] = 0x01;
				m_comm->WriteCommPort(send, 1);
				break;
			case 1:
				send[0] = 0x03;
				m_comm->WriteCommPort(send, 1);
				break;
			case 2:
				send[0] = 0x05;
				m_comm->WriteCommPort(send, 1);
				break;
			case 3:
				send[0] = 0x09;
				m_comm->WriteCommPort(send, 1);
				break;
			}
			send_str = temp;
			m_comm->WriteCommPort(send_str, send_str.GetLength());

			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}
void CCall_DllDlg::ver(int ver)
{
	/*CString rcv_ver;
	rcv_ver = 
	switch (status)
	{
	case 0:
		SetDlgItemText(IDC_STATUS, (CString)"Normal Status");
		break;
	case 1:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out");
		break;
	case 2:
		SetDlgItemText(IDC_STATUS, (CString)"Head open");
		break;
	case 3:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Head open");
		break;
	case 4:
		SetDlgItemText(IDC_STATUS, (CString)"Paper Jam");
		break;
	case 5:
		SetDlgItemText(IDC_STATUS, (CString)"Paper out && Paper Jam");
	}*/
}

void CCall_DllDlg::OnBnClickedButton10()
{
	CString ver_rcv;

	if (Combo2.GetCurSel() == 0)
	{
		CString t;
		long ret;
		Combo1.GetLBText(Combo1.GetCurSel(), t);

		// printer target model name
		ret = UsbOpen(t);

		if (ret < 0) {
			t.Format("%s", "Open Failed!");
		}
		else{
			PrintCmd(0x1D);
			PrintStr("IA");

			ver_rcv = ver_rcv + (char)DummyRealRead();
			ver_rcv = ver_rcv + (char)DummyRealRead();
			ver_rcv = ver_rcv + (char)DummyRealRead();
			ver_rcv = ver_rcv + (char)DummyRealRead();
			SetDlgItemText(IDC_VER, (CString)ver_rcv);
		}
	}
	else
	{

		Combo2.GetLBText(Combo2.GetCurSel(), m_str_comport);
		Combo3.GetLBText(Combo3.GetCurSel(), m_str_baudrate);
		

		CString m_strAddress = m_str_baudrate;
		DWORD m_wordAddress = (DWORD)_ttoi((LPCTSTR)m_strAddress);
		m_comm = new CRSPort(m_str_comport, m_wordAddress, (BYTE)8, (BYTE)NOPARITY, (BYTE)ONESTOPBIT);         // initial Comm port
		if (m_comm->IsCommPortOpen()) //Port Open Check
		{
			byte send[3] = { 0x1d, 0x49, 0x41 };
			byte rcv[4];
			m_comm->WriteCommPort(send, sizeof(send));
			Sleep(100);
			m_comm->ReadCommPort(rcv, 4);
			ver_rcv = ver_rcv + (char)rcv[0];
			ver_rcv = ver_rcv + (char)rcv[1];
			ver_rcv = ver_rcv + (char)rcv[2];
			ver_rcv = ver_rcv + (char)rcv[3];
			//AfxMessageBox(ver_rcv);
			
			
			SetDlgItemText(IDC_VER, (CString)ver_rcv);
			m_comm->CloseCommPort();
			m_comm = NULL;
		}
		else
		{
			AfxMessageBox(_T("ERROR!"));
		}
	}
}

