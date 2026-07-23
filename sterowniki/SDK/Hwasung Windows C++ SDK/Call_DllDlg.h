// Call_DllDlg.h : header file
//

#include "afxwin.h"
#include "RSPort.h"
#if !defined(AFX_CALL_DLLDLG_H__5D24B44B_B6BF_4883_9483_EB452F9B3E07__INCLUDED_)
#define AFX_CALL_DLLDLG_H__5D24B44B_B6BF_4883_9483_EB452F9B3E07__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CCall_DllDlg dialog

class CCall_DllDlg : public CDialog
{
// Construction
public:
	CCall_DllDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CCall_DllDlg)
	enum { IDD = IDD_CALL_DLL_DIALOG };
	CListBox	m_ModelList;
//	CComboBox	m_ModelName;


	CComboBox *Models;
	
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCall_DllDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CCall_DllDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	virtual void OnCancel();
	afx_msg void OnButton1();
	afx_msg void OnButton2();
	afx_msg void OnButton3();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	bool m_bSerialConnected;
	CRSPort *m_comm;
		CString m_str_comport;
	CString m_str_baudrate;
	void print_status(int status);
	void ver(int ver);
	CComboBox Combo1;
	CComboBox Combo2;
	CComboBox Combo3;
	CComboBox Combo4;
	CComboBox Combo5;
	CString Text1;
	CString Text2;
	afx_msg void OnBnClickedButton4();
	afx_msg void OnBnClickedButton5();
	afx_msg void OnBnClickedButton6();
	afx_msg void OnBnClickedButton7();
	afx_msg void OnBnClickedButton8();
	afx_msg void OnBnClickedButton9();
	afx_msg void OnBnClickedButton10();
	CStatic VER;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CALL_DLLDLG_H__5D24B44B_B6BF_4883_9483_EB452F9B3E07__INCLUDED_)
