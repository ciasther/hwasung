// Call_Dll.h : main header file for the CALL_DLL application
//

#if !defined(AFX_CALL_DLL_H__4DB0CC63_600F_481F_87BB_2C7F3EBDF788__INCLUDED_)
#define AFX_CALL_DLL_H__4DB0CC63_600F_481F_87BB_2C7F3EBDF788__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CCall_DllApp:
// See Call_Dll.cpp for the implementation of this class
//


class CCall_DllApp : public CWinApp
{
public:
	CCall_DllApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCall_DllApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CCall_DllApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CALL_DLL_H__4DB0CC63_600F_481F_87BB_2C7F3EBDF788__INCLUDED_)
